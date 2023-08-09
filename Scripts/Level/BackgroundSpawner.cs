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

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField]GameObject _background;

    [SerializeField] int _sizeX = 20, _sizeY = 20;
    [SerializeField] int _offsetX = 20, _offSetY = 20;

    private void Awake()
    {
        SpawnTiles();
    }

    void SpawnTiles()
    {
        for (int x = 0; x < _sizeX; x++)
            for (int y = 0; y < _sizeY; y++)
            {
                GameObject tile = Instantiate(_background);
                tile.transform.SetParent(transform);
                tile.transform.position += new Vector3(x + _offsetX , y + _offSetY, 0);
            }
    }
}
