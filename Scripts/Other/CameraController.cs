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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraController : MonoBehaviour
    {
        Transform _camera;
        float _input;

        private void Awake()
        {
            _camera = Camera.main.transform;
            _input = Camera.main.orthographicSize;
        }

        public void DoCamera()
        {
            CameraMovement();
            CameraZoom();
        }

        private void CameraMovement()
        {
            Vector3 v3Temp = new Vector3(transform.position.x, transform.position.y, -10);
            _camera.position = Vector3.Lerp(new Vector3(_camera.position.x, _camera.position.y, -10), v3Temp, Time.deltaTime * 3);
        }

        [SerializeField] float _zoomMin = 5f, _zoomMax = 8f;
        private void CameraZoom()
        {
            _input -= Input.GetAxis("Mouse ScrollWheel") * 2;
            _input = Math.Clamp(_input, _zoomMin, _zoomMax);
            Camera.main.orthographicSize = _input;
        }
    }
}
