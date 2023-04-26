using System;
using UnityEngine;

public partial class GameManager
{
    class LevelGenerationInfo
    {
    public static Vector2 GetRoomTypeSize(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Collider:
                return new Vector2(2, 4);

            case RoomType.Hallway:
                return new Vector2(8, 4);

            case RoomType.BigRoom:
                return new Vector2(16, 8);

            case RoomType.GiantRoom:
                return new Vector2(32, 16);

            case RoomType.ElevatorRoom:
                return new Vector2(2, 4);
        }
        throw new ArgumentException("Invalid RoomType");
    }    
    public static Vector2 GetHouseTemplateSize(HouseTemplate template)
    {
        switch (template)
        {
            case HouseTemplate.RegularHouse:
                return new Vector2(34f, 12f);

            case HouseTemplate.Outside:
                return new Vector2(32f, 16f);

            case HouseTemplate.LargeHouse:
                return new Vector2(84f, 16f);
        }
        throw new ArgumentException("Invalid RoomType");
    }
    public static RoomObject[] GetRoomsInHouseTemplate(HouseTemplate template, GameManager gameManager)
    {
        switch (template)
        {
            case HouseTemplate.RegularHouse:
                return new RoomObject[]
                {
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(0,0), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(8,0), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.ElevatorBottom, new Vector2Int(16,0), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,4), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.ElevatorTop, new Vector2Int(16,8), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(0,4), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.Hard, new Vector2Int(18,8), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.Loot | RoomTag.Hard, new Vector2Int(26,8), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.Exit, new Vector2Int(18,0), gameManager),
                };
                
            case HouseTemplate.Outside:
                return new RoomObject[]
                {
                    new RoomObject(RoomType.GiantRoom, RoomTag.Exit, new Vector2Int(0,0), gameManager),
                };

            case HouseTemplate.LargeHouse:
                return new RoomObject[] 
                {
                    new RoomObject(RoomType.Collider, RoomTag.None, new Vector2Int(0,0), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.ElevatorBottom, new Vector2Int(0,4), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.ElevatorTop, new Vector2Int(0,8), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(2,0), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.Loot, new Vector2Int(2,4), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.Hard | RoomTag.Loot, new Vector2Int(2,8), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(10,0), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(18,8), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(18,12), gameManager),
                    new RoomObject(RoomType.GiantRoom, RoomTag.None, new Vector2Int(26,0), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(58,0), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(58,8), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.ElevatorBottom, new Vector2Int(74,0), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(74,4), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.ElevatorTop, new Vector2Int(74,8), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.Exit, new Vector2Int(76,0), gameManager),
                };
                
        }
        throw new ArgumentException("Invalid HouseTemplate");
        }
    }

}
