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
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    public void RestartGame()
    {
        ResetStuff();
        SceneManager.LoadScene(1);
    }
    public void BackToMenu()
    {
        ResetStuff();
        SceneManager.LoadScene(0);
    }
    void ResetStuff()
    {
        //Destroy(transform.root.Find("Enemies"));
    }
}
