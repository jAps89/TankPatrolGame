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
*   22.04.2022      JA      created
*   03.06.2022      DB      Edited
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFinder : MonoBehaviour
{
    public int Room1 { private get; set; }
    public int Room2 { private get; set; }
    public bool IsActive { private get; set; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collis");
        if (collision == null || !IsActive) return;
        RefLib.sDoorManager.AddDoor(Room1, collision.transform.parent.GetComponent<IDoor>());
        print(collision.transform.parent.name);
        IsActive = false;
    }
}
