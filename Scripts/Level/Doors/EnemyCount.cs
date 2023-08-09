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

public class EnemyCount : MonoBehaviour
{
    int[] _count;
    public int[] Count { get { return _count; } set { _count = value;} }

    private void Awake()
    {
        RefLib.sEnemyCount = this;
    }
    public void CheckCount(int index)
    {
        if(_count[index] <= 0)
        {
            RefLib.sDoorManager.OpenDoors(index);
        }
    }
}
