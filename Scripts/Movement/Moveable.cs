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
*   22.4.22 JA created 
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace Assets.Scripts.Player
{

    //[CustomEditor(typeof(Moveable))]
    //public class MoveableEditor : Editor
    //{
    //    private Moveable _moveable;
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();
    //        _moveable = (Moveable)target;

    //        _moveable.AccelCurve = EditorGUILayout.CurveField("Accel",
    //            _moveable.AccelCurve, Color.yellow, new Rect(0, 0.05f, 1, 1));

    //        _moveable.DragCurve = EditorGUILayout.CurveField("Drag",
    //            _moveable.DragCurve, Color.cyan, new Rect(0, 0.05f, 1, 1));

    //        if (GUI.changed)
    //        {
    //            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    //            //_moveable._dragCurve = _moveable.DragCurve;
    //        }
    //    }
    //}
    [RequireComponent(typeof(Rigidbody2D))]
    class Moveable : MonoBehaviour
    {
        [SerializeField][Range(0, 50)] private float _moveSpeedMax = 15f;
        [SerializeField][Range(0, 50)] private float _moveAccel = 10f;
        [SerializeField][Range(0, 50)] private float _moveDrag = 15f;
        [SerializeField] AnimationCurve AccelCurve;
        [SerializeField] AnimationCurve DragCurve;

        private float MoveSpeedMax { get { return _moveSpeedMax * 2; } set { value = _moveSpeedMax; } }

        private float MoveAccel { get { return _moveAccel; } set { value = _moveAccel; } }

        private float MoveDrag { get { return _moveDrag /10; } set { value = _moveDrag; } }

        private Vector2 _moveInput;
        [HideInInspector] Rigidbody2D _rBody;
        [SerializeField] private Vector2 _velocity = Vector2.zero;

        private void Awake()
        {
            if(_rBody == null) _rBody = GetComponent<Rigidbody2D>();
        }
        public void DoMovement()
        {
            UpdateMoveInput();
            UpdateMovement();
        }

        private void UpdateMoveInput()
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");
            _moveInput.Normalize();
        }

        //Movement with acceleration and drag adjusted by curves 
        private void UpdateMovement()
        {
            float sqrMag = _velocity.sqrMagnitude;

            ApplyDrag(sqrMag);
            if (_moveInput == Vector2.zero)
                ApplyDrag(sqrMag);
            else
                _velocity += _moveInput * Mathf.Lerp(0, MoveAccel, 
                    LerpDist(sqrMag, MoveSpeedMax, MoveAccel, AccelCurve)) * Time.deltaTime;

            if (_velocity == Vector2.zero) return;

            if (sqrMag > MoveSpeedMax)
            {
                _velocity = _velocity * (MoveSpeedMax/ sqrMag);
            }

            _rBody.MovePosition(_rBody.position + _velocity * Time.fixedDeltaTime);
        }

        private void ApplyDrag(float sqrMag)
        {
            if (_velocity == Vector2.zero)
                return;
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, LerpDist(sqrMag, MoveSpeedMax, MoveDrag, DragCurve));
        }
        //See Rotateable.cs
        private float LerpDist(float diff, float ratio, float speed, AnimationCurve curve)
        {
            diff = Mathf.Abs(diff);
            float distNormalized = (ratio / diff) / ratio;
            return Mathf.Clamp01(Mathf.Clamp(curve.Evaluate(diff / ratio), 0.01f, 1f) * distNormalized * speed);
        }
    }
}
