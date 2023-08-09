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

[CreateAssetMenu(fileName = "New ColorPalette", menuName = "ColorPalette", order = 100)]
public class ColorPalette : ScriptableObject
{
    [SerializeField] Color[] colorsFloor;
    [SerializeField] Color[] colorsObstacle;
    [SerializeField] Color[] colorsWall;
    [SerializeField] Color[] colorsDoor;
    [SerializeField] Color[] colorsEnemy;
    [SerializeField] Color[] colorsBarrel;
    [SerializeField] GameObject[] _floors;
    [SerializeField] GameObject[] _obstacles;

    public Color[] ColorsFloor { get { return colorsFloor; } }
    public Color[] ColorsObstacle { get { return colorsObstacle; } }
    public Color[] ColorsWall { get { return colorsWall; } }
    public Color[] ColorsDoor { get { return colorsDoor; } }
    public Color[] ColorsEnemy { get { return colorsEnemy; } }
    public Color[] ColorsBarrel { get { return colorsBarrel; } }
    public GameObject[] Floors { get { return _floors; } }
    public GameObject[] Obstacles { get { return _obstacles; } }
}
