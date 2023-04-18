using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static Dictionary<String, Sprite> loadedSprites = new();
    public class RoomObject
    {
        private Vector2Int placement;
        private GameObject enemyPrefab;
        private int baseEnemyAmount;
        private Sprite backgroundSprite;
        private RoomTag tags;
        private GameManager.RoomType roomType;

        public RoomObject(RoomType roomType, RoomTag flag, Vector2Int placement)
        {
            this.roomType = roomType;
            this.tags = flag;
            this.placement = placement;

            this.backgroundSprite = GetSpriteFromRoomType(roomType);
            this.baseEnemyAmount = GetBaseEnemyAmountFromRoomType(roomType);

        }

    }

    
    public static Vector2Int GetRoomTypeSize(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Hallway:
                return new Vector2Int(8, 4);

            case RoomType.BigRoom:
                return new Vector2Int(16, 8);

            case RoomType.GiantRoom:
                return new Vector2Int(32, 16);

            case RoomType.ElevatorRoom:
                return new Vector2Int(2, 4);
        }

        throw new ArgumentException("Invalid RoomType");
    }

    public RoomObject[] GetRoomsInHouseTemplate(HouseTemplate template)
    {
        switch (template)
        {
            case HouseTemplate.RegularHouse:
                return new RoomObject[]
                {
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(0,0)),
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(8,0)),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,0)),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,4)),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,8)),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(0,4)),
                    new RoomObject(RoomType.Hallway, RoomTag.Hard, new Vector2Int(18,8)),
                    new RoomObject(RoomType.Hallway, RoomTag.Loot | RoomTag.Hard, new Vector2Int(26,8)),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(18,0)),
                };

            case HouseTemplate.Outside:
                return new RoomObject[]
                {
                    new RoomObject(RoomType.GiantRoom, RoomTag.None, new Vector2Int(0,0)),
                };
                /* Coming soon
            case HouseTemplate.TreasureTrove:
                return new RoomObject[]
                {

                };

            case HouseTemplate.EnemyNest:
                return new RoomObject[]
                {

                };
                */
        }

        throw new ArgumentException("Invalid HouseTemplate");
    }
    public static int GetBaseEnemyAmountFromRoomType(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Hallway:
                return 1;

            case RoomType.BigRoom:
                return 4;

            case RoomType.GiantRoom:
                return 6;

            case RoomType.ElevatorRoom:
                return 0;
        }

        throw new ArgumentException("Invalid RoomType");
    }
    public static string[] GetSpritePathsFromRoomType(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Hallway:
                return new string[] { "Assets/Prefabs/Rooms/Hallway/Dev"};

            case RoomType.BigRoom:
                return new string[] { "Assets/Prefabs/Rooms/BigRoom/Dev" };

            case RoomType.GiantRoom:
                return new string[] { "Assets/Prefabs/Rooms/GiantRoom/Dev" };

            case RoomType.ElevatorRoom:
                return new string[] { "Assets/Prefabs/Rooms/ElevatorRoom/Dev" };
        }

        throw new ArgumentException("Invalid RoomType");
    }

    public static Sprite GetSpriteFromRoomType(RoomType roomType)
    {
        var paths = GetSpritePathsFromRoomType(roomType);
        string path = paths[UnityEngine.Random.Range(0, paths.Length)];

        if (!loadedSprites.TryGetValue(path, out _)) loadedSprites.Add(path, Resources.Load<Sprite>(path));

        return loadedSprites[path];
    }
    public enum RoomTag
    {
        None = 0,
        Boss = 1 << 0,
        Loot = 1 << 1,
        Hard = 1 << 2,
        BigEnemies = 1 << 3,
    }

    public enum HouseTemplate
    {
        RegularHouse,
        Outside,
        TreasureTrove,
        EnemyNest
    }
    public enum RoomType
    {
        Empty,
        Hallway,
        BigRoom,
        GiantRoom,
        ElevatorRoom,
    }

    class HouseFactory
    {
        public void GenerateHouse(HouseTemplate template)
        {
            
        }
        public static void CreateRoom(RoomObject room)
        {

        }


    }
}
