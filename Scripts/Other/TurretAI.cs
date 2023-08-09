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
using UnityEngine;

namespace Assets.Scripts.Player
{
    //[RequireComponent(typeof(WeaponComp))]
    [RequireComponent(typeof(Rotateable))]
    internal class TurretAI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private Rotateable _Rotateable;
        //private WeaponComp _Weapon;

        [SerializeField] private bool isActive;
        [SerializeField] private bool isShoot;
        private bool isOnTarget = false;

        private void Awake()
        {
            _Rotateable = GetComponent<Rotateable>();
            //_Weapon = GetComponent<WeaponComp>();
        }
        private void Start()
        {
            StartCoroutine(AILoop());
            StartCoroutine(OnTargetLoop());
        }

        private IEnumerator AILoop()
        {
            while (isActive)
            {
                _Rotateable.RotateTowardsTargetT(target);
                //while (!isOnTarget)
                //{
                //    _Rotateable.RotateTowardsTarget(target);
                //    yield return Time.fixedDeltaTime;
                //}
                while (isOnTarget && isShoot)
                {
                    _Rotateable.RotateTowardsTargetT(target);
                    //_Weapon.Fire();
                    yield return Time.fixedDeltaTime;
                }
                yield return Time.fixedDeltaTime;
            }
        }

        private IEnumerator OnTargetLoop()
        {
            while (isActive)
            {
                float targetAngleDiff = _Rotateable.AngleDifferenceToTarget(target, true);
                isOnTarget = targetAngleDiff < 2;
                yield return Time.fixedDeltaTime;
            }
        }
    }
}
