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

public class DoorTrigger : MonoBehaviour
{
    public bool IsOpen { private get; set; }
    IDoor _door;
    int _doorNum;

    private void Awake()
    {
        _door = transform.root.GetComponent<IDoor>();
        _doorNum = GlobalValues.sDoorTriggerNum;
        GlobalValues.sDoorTriggerNum++;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>() != null)
        {
            if (IsOpen)
                _door.OpenDoor();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>() != null)
        {
            if (IsOpen)
                _door.CloseDoor();
        }
    }
}
