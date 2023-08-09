/*****************************************************************************
* Project: CMN5201gpr-0322-Game
* File   : Rotateable.cs
* Date   : 17.04.22
* Author : Jan Apsel (JA)
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*   17.04.22    JA	Created
******************************************************************************/

using System;
using AngleExtension;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player
{
    //[RequireComponent(typeof(Rigidbody2D))]
    public class Rotateable : MonoBehaviour
    {
        [SerializeField] private Transform _parentT;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private bool _isConstrained;

        [HideInInspector] public float ConstrStart;
        [HideInInspector] public float ConstrEnd;
        [HideInInspector] Rigidbody2D _rBody;
        [SerializeField] AnimationCurve _rotationCurve;

        [SerializeField][Range(0, 360)] private float _parentOffset;
        private float _constrS, _constrE, _ownAngle, _parentAngle;
        private bool _isWideConstrain;

        private void OnValidate()
        {
            if(_rBody == null) _rBody = GetComponent<Rigidbody2D>();
            UpdateAngles();
        }
        private void Awake()
        {
            if (_rBody == null) _rBody = GetComponent<Rigidbody2D>();

            UpdateAngles();
        }

        public void UpdateAngles()
        {
            
            UpdateOwnAngle();
            UpdateParentAngle();
            UpdateConstrains();
        }
        public void CenterConstrains()
        {
            float range = Mathf.Abs(ConstrStart - ConstrEnd) / 2;
            ConstrStart = AngleWrap(180 - range);
            ConstrEnd = AngleWrap(180 + range);
        }

        private void UpdateOwnAngle()
        {
            _ownAngle = transform.eulerAngles.z;
        }

        private void UpdateParentAngle()
        {
            if(_parentT != null)
                _parentAngle = AngleWrap(_parentT.eulerAngles.z + 180f + _parentOffset);
            else _parentAngle = AngleWrap (180f + _parentOffset);
        }

        private void UpdateConstrains()
        {
            _constrS = AngleWrap(ConstrStart + _parentAngle);
            _constrE = AngleWrap(ConstrEnd + _parentAngle);

            _isWideConstrain = IsWideConstrain();
        }

        private bool isIncludeZero;

        //Returns angle or closest constrain angle if angle is inside constrain
        private float ConstrainAngle(float cStart, float cEnd, float angle)
        {
            if (cStart < cEnd) // no 0
            {
                isIncludeZero = false;
                if (angle > cStart && angle < cEnd)
                    return angle;
                return ClosestAngle(angle, cStart, cEnd);
            }
            if (cStart > cEnd) //include 0
            {
                isIncludeZero = true;
                if (angle > cStart || angle < cEnd)
                    return angle;
                return ClosestAngle(angle, cStart, cEnd);
            }
            return angle;
        }

        private float _currTargetAngle;
        public void RotateTowardsTargetT(Transform target)
        {
            if (target == null) return;
            RotateTowardsTargetV2(target.ToVector2());
        }
        float targetAngle;
        float currAngle;

        [SerializeField] bool isShowLog = false;
        //Lerp rotates towards target angle with cases for path to
        //target angle including zero on 360 degree scale.
        //Has a minor bug.
        public void RotateTowardsTargetV2(Vector2 target)
        {
            UpdateOwnAngle();
            if (_isConstrained)
            {
                UpdateParentAngle();
                UpdateConstrains();
            }

            Vector2 targetDir = target - transform.position.ToVector2();
            targetAngle = AngleWrap(Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f);
            if (_isConstrained)
                targetAngle = ConstrainAngle(_constrS, _constrE, targetAngle);
            currAngle = 0;

            _currTargetAngle = targetAngle;
            //if (targetAngle != _currTargetAngle)
            //    _currTargetAngle = AngleWrap(LerpAngle(_currTargetAngle, targetAngle));

            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(_currTargetAngle, _ownAngle));

            if (!_isWideConstrain || !_isConstrained)
            {
                currAngle = Mathf.LerpAngle(_ownAngle, _currTargetAngle, 
                    LerpDist(angleDiff, 180, _turnSpeed, _rotationCurve));
            }
            else
            {
                float tempTargetAngle = _currTargetAngle;
                float tempOwnAngle = _ownAngle;
                if (isIncludeZero)
                {
                    if (_ownAngle <= _constrE && _ownAngle >= 0f) _ownAngle += 360;
                    if (tempTargetAngle <= _constrE && tempTargetAngle >= 0f) tempTargetAngle += 360;
                    if (tempOwnAngle <= _constrE) tempOwnAngle += 360;
                    if (tempTargetAngle <= _constrE) tempTargetAngle += 360;
                }
                currAngle = AngleWrap(Mathf.Lerp(tempOwnAngle, tempTargetAngle, 
                    LerpDist(Mathf.Clamp(angleDiff, 0.01f, 180), 180, _turnSpeed, _rotationCurve)));

                if (isShowLog)
                    print("ownangle:" + tempOwnAngle + " targewt:" + tempTargetAngle + " currangle:" + currAngle);
            }
            transform.eulerAngles = Vector3.forward * currAngle;
        }
        //Normalizes lerp change by distance for the use of a curve instead
        private float LerpDist(float diff, float ratio, float speed, AnimationCurve curve)
        {
            diff = Mathf.Clamp(Mathf.Abs(diff), 0.01f, ratio); ////---
            float distNormalized = (ratio / diff) / ratio;
            return Mathf.Clamp01(Mathf.Clamp(curve.Evaluate(diff / ratio), 0.01f, 1f) * distNormalized * speed);
        }

        private float ClosestAngle(float angle, float targetA, float targetB)
        {
            return Mathf.Abs(Mathf.DeltaAngle(targetA, angle)) < Mathf.Abs(Mathf.DeltaAngle(targetB, angle)) ? targetA : targetB;
        }

        private bool IsWideConstrain()
        {
            if (Mathf.Abs(-ConstrStart + ConstrEnd) > 179.9f)
                return true;
            return false;
        }

        public float AngleDifferenceToTarget(Transform target, bool isAbsolut)
        {
            float ownAngle = transform.up.ToVector2().GetAngle();
            float angleToTarget = (target.ToVector2() - transform.position.ToVector2()).GetAngle();
            float angleDiff = ownAngle - angleToTarget;
            if (angleDiff > 180) angleDiff = -180 + angleDiff % 180;

            return isAbsolut ? Mathf.Abs(angleDiff) : angleDiff;
        }

        private float AngleWrap(float _angle)
        {
            return _angle < 0 ? 360 + _angle : 
                   _angle > 360 ? _angle - 360 : _angle;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_isConstrained)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, DegreeToV3Relative(AngleWrap(ConstrStart + _parentAngle)));
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, DegreeToV3Relative(AngleWrap(ConstrEnd + _parentAngle)));
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, DegreeToV3Relative(_parentAngle));

                Handles.color = Color.Lerp(Color.red, Color.clear, 0.7f);
                Handles.DrawSolidArc(transform.position, Vector3.forward, DegreeToV3Relative(ConstrStart + _parentAngle) 
                    - transform.position, Mathf.Abs(ConstrStart - ConstrEnd), 2.5f);
            }
        }
#endif

        private Vector3 DegreeToV3Relative(float degree)
        {
            return transform.position + degToV2(degree + 90f).ToVector3().normalized * 2.5f;
        }

        private Vector2 degToV2(float degree)
        {
            return new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
        }
    }
}
