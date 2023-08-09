
//TODO:
//Steuerung nach Richtung.

using AngleExtension;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField][Range(0, 50)] private float mTurnSpeed = 20f;
        [SerializeField][Range(0, 50)] private float mMoveSpeedMax = 15f;
        [SerializeField][Range(0, 50)] private float mMoveAccel = 10f;
        [SerializeField][Range(0, 50)] private float mMoveDrag = 15f;
        [SerializeField] private bool isCamFollow = true;
        [SerializeField] private AnimationCurve mAccelCurve;
        [SerializeField] private AnimationCurve mRotationCurve;
        [SerializeField] private AnimationCurve mDragCurve;
        public AnimationCurve mCurve;

        private float pTurnSpeed { get { return mTurnSpeed /3; } set { value = mTurnSpeed; } }

        private float pMoveSpeedMax { get { return mMoveSpeedMax * 2; } set { value = mMoveSpeedMax; } }

        private float pMoveAccel { get { return mMoveAccel; } set { value = mMoveAccel; } }

        private float pMoveDrag { get { return mMoveDrag /10; } set { value = mMoveDrag; } }

        //CharacterController cControl;

        private Vector2 moveInput;
        [SerializeField] private Vector2 mVelocity = Vector2.zero;
        private Vector2 mousPos;
        private IWeapon[] mWeapons;
        private float deltaT;
        [SerializeField] private float targetAngle;
        private Rigidbody2D rBody;
        private Transform cam;




        
        private void Awake()
        {
            //ReferenceLib.sPlayerCtrl = this;
            //cControl = GetComponent<CharacterController>();
            rBody = GetComponent<Rigidbody2D>();
            mWeapons = GetComponentsInChildren<IWeapon>();
            cam = Camera.main.transform;
            //currTarget = transform.up.ToVector2().GetAngle();

        }
        private void Update()
        {
            CameraZoom();
            DoWeapons();


        }
        private void FixedUpdate()
        {
            mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            deltaT = Time.fixedDeltaTime;
            DoMovement();

            if (isCamFollow)
                cam.position = new Vector3(transform.position.x, transform.position.y, -10);
        }

        private void DoMovement()
        {
            UpdateRotation();
            UpdateMoveInput();
            UpdateMovement();
        }

        private void UpdateMoveInput()
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
        }

        private void UpdateMovement()
        {
            float sqrMag = mVelocity.sqrMagnitude;

            ApplyDrag(sqrMag);  ///////////
            if (moveInput == Vector2.zero)
                ApplyDrag(sqrMag);
            else
                mVelocity += moveInput * Mathf.Lerp(0, pMoveAccel, LerpDist(sqrMag, pMoveSpeedMax, pMoveAccel, mAccelCurve)) * deltaT; //pMoveAccel * deltaT;

            if (mVelocity == Vector2.zero) return;

            if (sqrMag > pMoveSpeedMax)
            {
                mVelocity = mVelocity * (pMoveSpeedMax/ sqrMag);
            }

            //if (mVelocity.x == float.NaN || mVelocity.y == float.NaN) mVelocity = Vector2.zero;
            rBody.MovePosition(rBody.position + mVelocity * deltaT);
        }

        private void ApplyDrag(float sqrMag)
        {
            if (mVelocity == Vector2.zero)
                return;
            mVelocity = Vector2.Lerp(mVelocity, Vector2.zero, LerpDist(sqrMag, pMoveSpeedMax, pMoveDrag, mDragCurve));  //speedRatio * deltaT * mMoveDrag); //  mVelocity - mVelocity * pMoveDrag/5 * deltaT;
        }

        [SerializeField]
        private float currAngle;

        private void UpdateRotation()
        {
            Vector2 lookDir = mousPos - rBody.position;
            float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

            if (lookAngle != targetAngle)
                targetAngle = AngleWrap(LerpAngle(targetAngle, lookAngle));

            float angleDiff = Mathf.DeltaAngle(targetAngle, rBody.rotation);
            if (angleDiff == 0) return;

            currAngle = AngleWrap(Mathf.LerpAngle(rBody.rotation, targetAngle, LerpDist(angleDiff, 180, pTurnSpeed, mRotationCurve)));

            rBody.rotation = currAngle; //NaN ?
        }

        private float LerpAngle(float currTarget, float newTarget)
        {
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currTarget, newTarget));

            return Mathf.LerpAngle(currTarget, newTarget, LerpDist(angleDiff, 180, pTurnSpeed, mRotationCurve)); /////
        }

        private float LerpDist(float _diff, float _ratio, float _speed, AnimationCurve _curve)
        {
            _diff = Mathf.Abs(_diff);
            float distUnified = (_ratio / _diff) / _ratio;
            return Mathf.Clamp01(_curve.Evaluate(_diff / _ratio) * distUnified * _speed);
        }

        private float AngleWrap(float _angle)
        {
            return _angle < 0 ? 360 + _angle : _angle > 360 ? _angle - 360 : _angle;
        }

        public float AngleDifferenceToTarget(Transform _target, bool _isAbsolut)
        {
            float ownAngle = transform.up.ToVector2().GetAngle();
            float angleToTarget = (_target.ToVector2() - rBody.position).GetAngle();
            float AngleDiff = ownAngle - angleToTarget;
            if (AngleDiff > 180) AngleDiff = -180 + AngleDiff % 180;

            return _isAbsolut ? Mathf.Abs(AngleDiff) : AngleDiff;
        }

        private void CameraZoom()
        {
            Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 2;
        }

        private IShoot _shoot;
        private void DoWeapons()
        {
            if (_shoot == null) _shoot = GetComponent<IShoot>();
            if (_shoot == null) return;

            if (Input.GetButton("Fire1"))
            {
                _shoot.Shoot();
            }
        }
    }

    internal interface IWeapon
    {
        public void Fire();

        //Stopfire();
    }
}