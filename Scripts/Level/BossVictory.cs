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
using UnityEngine.SceneManagement;

public class BossVictory : MonoBehaviour
{
    private void Awake()
    {
        RefLib.sPlayer.gameObject.GetComponent<EntityStats>().OnDeath += OnDeath;
        gameObject.GetComponent<EntityStats>().OnDeath += OnVictory;
    }
    public void OnDeath()
    {
        GlobalValues.sIsEnemyAggro = false;
        ResetScene();
    }
    public void OnVictory()
    {
        GlobalValues.sIsEnemyAggro = false;
        //Invoke("Victory", 1f);
        Victory();
    }
    void ResetScene()
    {
        ResetStuff();
        RefLib.sPlayerCtrl.SwitchRestartMenu();
    }
    void Victory()
    {
        ResetStuff();
        RefLib.sPlayerCtrl.SwitchVictoryMenu();
    }

    public static void ResetStuff()
    {
        GameObject.Find("/LevelGenerator").GetComponent<LevelGenerator>().ClearLevel();
        //BSPMap.s_allRooms.Clear();
        //BSPMap.s_allHallWays.Clear();
        //BSPMap.s_doorsBetweenRooms.Clear();
    }
}
