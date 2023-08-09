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
using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    [SerializeField] GameObject _doorGO;
    [SerializeField] DoorTrigger _doorTrigger;
    [SerializeField] bool _isOpenByTrigger;

    DmgFlash _flash;
    Collider2D _doorCollider;

    private void Awake()
    {
        if (_doorTrigger == null) return;
        _flash = _doorGO.GetComponent<DmgFlash>();
        _doorCollider = _doorGO.GetComponent<Collider2D>();
        _doorTrigger.IsOpen = _isOpenByTrigger;
    }
    public void OpenDoor() 
    {
        _flash.StartFlash(false);
        _doorCollider.enabled = false;
        _isOpenByTrigger = false;
    }
    public void CloseDoor()
    {
        _doorCollider.enabled = true;
        _flash.StartFlash(true);
    }
    public void UnlockDoor()
    {
        _doorTrigger.IsOpen = true;
    }
}

public interface IDoor
{
    public void OpenDoor();
    public void CloseDoor();
    public void UnlockDoor();
}
