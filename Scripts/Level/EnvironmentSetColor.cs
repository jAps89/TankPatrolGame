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

public class EnvironmentSetColor : MonoBehaviour
{
    [SerializeField] int _index;
    [SerializeField] eEnvType _envType;
    [SerializeField] SpriteRenderer[] _spriteRenderer;
    LevelSettings _settings;
    private void Awake()
    {
    }
    private void Start()
    {
        _settings = RefLib.sLevelSettings;
        SetColor();
    }

    void SetColor()
    {
        Color[] colors = new Color[0];
        if (_settings == null) return;
        switch (_envType)
        {
            case eEnvType.FLOOR: 
                colors = _settings.Palette.ColorsFloor;
                break;
            case eEnvType.OBSTACLE:
                colors = _settings.Palette.ColorsObstacle;
                break;
            case eEnvType.WALL:
                colors = _settings.Palette.ColorsWall;
                break;
            case eEnvType.DOOR:
                colors = _settings.Palette.ColorsDoor;
                break;
            case eEnvType.ENEMY:
                colors = _settings.Palette.ColorsEnemy;
                break;
            case eEnvType.BARREL:
                colors = _settings.Palette.ColorsBarrel;
                break;
        }
        foreach (SpriteRenderer renderer in _spriteRenderer)
        {
            Color tmpColor = colors[_index % colors.Length];
            tmpColor.a = renderer.color.a;
            renderer.color = tmpColor;
        }
    }
}
enum eEnvType
    {
        FLOOR, OBSTACLE, WALL, DOOR, ENEMY, BARREL
    }
