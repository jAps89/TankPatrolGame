/*****************************************************************************
* Project: CMN5201gpr-0322-Game
* File   : PlayerCore.cs
* Date   : 03.06.2022
* Author : Alexander Sigmund (AS)
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*
******************************************************************************/

using UnityEngine;
using Assets.Scripts.MapGeneration;

public class PlayerCore : MonoBehaviour
{

    private PlayerInformation _playerInformation;
    public PlayerInformation PlayerInformation { get => _playerInformation; }

    [SerializeField] private EntityStats _playerStats;
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private Heatmeter _heatmeter;
    [SerializeField] private OverdriveManager _overdriveManager;
    [SerializeField] private AudioManager _audioManager;

    private float _posX, _posY;
    private bool triggered = false;

    private void Awake()
    {
        _playerInformation = new PlayerInformation();

        if (_playerStats == null) _playerStats = GetComponent<EntityStats>();
        if (_heatmeter == null) _heatmeter = GetComponent<Heatmeter>();
        if (_weaponManager == null) _weaponManager = GetComponent<WeaponManager>();
        if (_overdriveManager == null) _overdriveManager = GetComponent<OverdriveManager>();

        if(_audioManager == null)_audioManager = GameObject.Find("/AudioManager")?.GetComponent<AudioManager>();

        _playerInformation.Heatmeter = _heatmeter;
        _playerInformation.WeaponManager = _weaponManager;
        _playerInformation.PlayerStats = _playerStats;
        _playerInformation.OverdriveManager = _overdriveManager;
        _playerInformation.AudioManager = _audioManager;
    }

    private void Update()
    {
        _posX = transform.position.x;
        _posY = transform.position.y;

        if (_audioManager == null) return;

        if (PlayerInBossRoom() && !triggered)
        {
            _audioManager.ChangeMelody(_audioManager.MusicBossRoom);
            triggered = true;
        } else if(triggered && !PlayerInBossRoom())
        {
            _audioManager.ChangeMelody(_audioManager.MusicLevel);
            triggered = false;
        }
    }

    public bool PlayerInBossRoom()
    {
        if(BSPMap.s_allRooms == null || BSPMap.s_allRooms.Count == 0) return false;

        return GlobalValues.sIsPlayerActive && BSPMap.s_allRooms[BSPMap.s_allRooms.Count-1].X <= _posX && BSPMap.s_allRooms[BSPMap.s_allRooms.Count-1].Y <= _posY;
    }
}
