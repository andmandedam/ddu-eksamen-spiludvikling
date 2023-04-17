using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum RoomType
    {
        Hallway,
        BigRoom,
        GiantRoom,
        ElevatorRoom,
    }
    
    public static Vector2Int GetRoomTypeSize(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Hallway:
                return new Vector2Int(16, 8);

            case RoomType.BigRoom:
                return new Vector2Int(8, 4);

            case RoomType.GiantRoom:
                return new Vector2Int(32, 16);

            case RoomType.ElevatorRoom:
                return new Vector2Int(2, 4);
        }

        throw new ArgumentException("Invalid RoomType");
    }

    class HouseFactory
    {
        public void CreateRoom(Vector2Int placement, RoomType roomType, Sprite WallTexture)
        {

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
