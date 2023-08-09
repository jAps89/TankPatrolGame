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
*   25.4.22 JA created 
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.MapGeneration;
using UnityEngine;

public class PlayerRoomPosition : MonoBehaviour
{
    Transform _player;
    [SerializeField] bool isActive = true;
    [SerializeField] int _showRoom;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        StartCoroutine(SlowUpdate(2.15f));
    }

    int CheckRoom()
    {
        for (int i = 0; i < BSPMap.s_allRooms.Count; i++)
            if (_player.transform.position.x > BSPMap.s_allRooms[i].X &&
               _player.transform.position.y > BSPMap.s_allRooms[i].Y &&
               _player.transform.position.x < BSPMap.s_allRooms[i].X + BSPMap.s_allRooms[i].Width &&
               _player.transform.position.y < BSPMap.s_allRooms[i].Y + BSPMap.s_allRooms[i].Height)
                return i;
        return -1;
    }
    IEnumerator SlowUpdate(float seconds)
    {
        while(isActive)
        {
            int currRoom = CheckRoom();
            if (currRoom > -1)
            {
                _showRoom = currRoom;
                GlobalValues.sPlayerCurrRoom = currRoom;
            }
            yield return new WaitForSeconds(seconds);
        }
    }
}
