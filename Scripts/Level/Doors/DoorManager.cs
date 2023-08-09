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
using Assets.Scripts.MapGeneration;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] GameObject _doorFinderGO;
    List<IDoor>[] _doorsByRoom;
    DoorFinder _doorFinder;

    private void Awake()
    {
        RefLib.sDoorManager = this;
        
    }

    private void Start()
    {
        _doorFinder = _doorFinderGO.GetComponent<DoorFinder>();
        FindDoors();
        OpenDoors(0);
    }

    public void OpenDoors(int room)
    {
        foreach(IDoor door in _doorsByRoom[room])
            door.UnlockDoor();
    }

    public void AddDoor(int room, IDoor door)
    {
        _doorsByRoom[room].Add(door);
    }

    void FindDoors()
    {
        List<Room> rooms = BSPMap.s_allRooms;
        _doorsByRoom = new List<IDoor>[rooms.Count];
        for (int i = 0; i < rooms.Count; i++)
            foreach (RoomDoorConnection doorsR in BSPMap.s_doorsBetweenRooms)
            {
                _doorFinder.Room1 = i;
                _doorFinder.IsActive = true;
                if (doorsR.Room1.X == rooms[i].X && doorsR.Room1.Y == rooms[i].Y)
                {
                    _doorFinderGO.transform.position = new Vector3(doorsR.DoorRoom1.x, doorsR.DoorRoom1.y, 0);
                    _doorFinderGO.transform.position = new Vector3(doorsR.DoorRoom2.x, doorsR.DoorRoom2.y, 0);
                }
                if (doorsR.Room2.X == rooms[i].X && doorsR.Room2.Y == rooms[i].Y)
                {
                    _doorFinderGO.transform.position = new Vector3(doorsR.DoorRoom1.x, doorsR.DoorRoom1.y, 0);
                    _doorFinderGO.transform.position = new Vector3(doorsR.DoorRoom2.x, doorsR.DoorRoom2.y, 0);
                }
            }
    }
    //void GetHallWays()
    //{
    //    foreach (Transform child in _map.transform)
    //        if (child.transform.name.Contains("Hallways"))
    //            _hallways = child;
    //}
    //void GetDoors()
    //{
    //    List<GameObject> doors = new List<GameObject>();
    //    foreach (Transform child in _hallways)
    //        if (child.transform.CompareTag("Door"))
    //            doors.Add(child.gameObject);


    //    foreach (RoomDoorConnection doorsR in BSPMap.s_doorsBetweenRooms)
    //    {
    //        string door1 = ((int)doorsR.DoorRoom1.x).ToString() + "|" +  ((int)doorsR.DoorRoom1.y).ToString();
    //        string door2 = ((int)doorsR.DoorRoom2.x).ToString() + "|" +  ((int)doorsR.DoorRoom2.y).ToString(); /////

    //        foreach (GameObject door in doors)
    //        {
    //            if (door.name.Contains(door1))
    //                _doorsDic.Add(doorsR.DoorRoom1, door.GetComponent<IDoor>());
    //            else if (door.name.Contains(door2))
    //                _doorsDic.Add(doorsR.DoorRoom2, door.GetComponent<IDoor>());
    //        }
    //    }
    //    List<Room> rooms = BSPMap.s_allRooms;
    //    _doorsByRoom = new List<IDoor>[rooms.Count];
    //    for (int i = 0; i < rooms.Count; i++)
    //        foreach (RoomDoorConnection doorsR in BSPMap.s_doorsBetweenRooms)
    //        {
    //            if (doorsR.Room1.X == rooms[i].X && doorsR.Room1.Y == rooms[i].Y)
    //            {
    //                _doorsByRoom[i].Add(_doorsDic[doorsR.DoorRoom1]);
    //                _doorsByRoom[i].Add(_doorsDic[doorsR.DoorRoom2]);
    //            }
    //            if (doorsR.Room2.X == rooms[i].X && doorsR.Room2.Y == rooms[i].Y)
    //            {
    //                _doorsByRoom[i].Add(_doorsDic[doorsR.DoorRoom1]);
    //                _doorsByRoom[i].Add(_doorsDic[doorsR.DoorRoom2]);
    //            }
    //        }
    //}
}
