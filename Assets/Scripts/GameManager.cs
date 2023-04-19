using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class GameManager : MonoBehaviour
{
    static Dictionary<String, GameObject> loadedPrefabs = new();

    public const float HardRoomMultiplier = 1.5f;

    public static GameObject[] rooms;

    private void Start()
    {
        HouseFactory.GenerateHouse();
    }

    public class RoomObject
    {
        public Vector2Int placement;
        public GameObject enemyPrefab;
        public int enemyAmount;
        public GameObject roomPrefab;
        public RoomTag tags;
        public GameManager.RoomType roomType;

        public RoomObject(RoomType roomType, RoomTag flag, Vector2Int placement)
        {
            this.roomType = roomType;
            this.tags = flag;
            this.placement = placement;

            this.roomPrefab = GetPrefab(roomType);
            this.enemyAmount = GetBaseEnemyAmount(roomType);

            if ((this.tags & RoomTag.Hard) != 0)
            {
                this.enemyAmount = (int)(HardRoomMultiplier * this.enemyAmount) + 1;
            }

            if ((this.tags & RoomTag.BigEnemies) != 0)
            {
                this.enemyPrefab = GetPrefab(EnemyType.Big);
            }
        }

    }
    
    class RandomEnum
    {
        static System.Random _R = new System.Random();
       public  static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }
    }
    class HouseFactory
    {
        private const float EnemyEdgeMargin = 1f; //The minimum distance an enemy can be placed from edge of a room
        private const float SmashableSpawnRate = 0.6f; // F.x. Barrels or vases

        public static void GenerateHouse()
        {
            var rooms = GetRoomsInHouseTemplate(RandomEnum.RandomEnumValue<HouseTemplate>());
            for (int i = 0; i < rooms.Length; i++)
            {
                GameManager.rooms[i] = CreateRoom(rooms[i]);
            }

        }
        public static GameObject CreateRoom(RoomObject room)
        {
            var roomObject = Instantiate(room.roomPrefab);
            roomObject.transform.position = (Vector3Int)room.placement;
            PlaceEnemies(room);
            RoomInfo roomInfo;
            
            if (!roomObject.TryGetComponent<RoomInfo>(out roomInfo))
            {
                return roomObject;
                throw new Exception("No RoomInfo on: " + roomObject + " Created from: " + room);
            }

            for (int i = 0; i < roomInfo.placeableSpawnPoint.Length; i++)
            {
                if (UnityEngine.Random.value < SmashableSpawnRate)
                {
                    Instantiate(GetPrefab(PlaceableType.Smashable)).transform.position = roomInfo.placeableSpawnPoint[i] + (Vector3Int)room.placement;
                }
                
            }

            for (int i = 0; i < roomInfo.tableTopDecorationSpawnPoint.Length; i++)
            {
                if (UnityEngine.Random.value < SmashableSpawnRate)
                {
                    Instantiate(GetPrefab(DecorationType.TableTop)).transform.position = roomInfo.tableTopDecorationSpawnPoint[i] + (Vector3Int)room.placement;
                }

            }

            return roomObject;
        }

        private static void PlaceEnemies(RoomObject room)
        {
            var roomSize = GetRoomTypeSize(room.roomType);

            for (int i = 0; i < room.enemyAmount; i++)
            {
                var enemyPosition = new Vector3(
                    Math.Clamp(UnityEngine.Random.value * roomSize.x, EnemyEdgeMargin, roomSize.x - EnemyEdgeMargin),
                    Math.Clamp(UnityEngine.Random.value * roomSize.y, EnemyEdgeMargin, roomSize.y - EnemyEdgeMargin)
                        );

                Instantiate(room.enemyPrefab).transform.position = (Vector3Int)room.placement + enemyPosition;
            }
                

        }
    }
    
    public static Vector2 GetRoomTypeSize(RoomType roomType)
    {
        switch (roomType)
        {
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
    public static RoomObject[] GetRoomsInHouseTemplate(HouseTemplate template)
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
    public static int GetBaseEnemyAmount(RoomType roomType)
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
    public static string[] GetPrefabPaths(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Hallway:
                return new string[] 
                { 
                    "Assets/Prefabs/Rooms/Hallway/Dev" 
                };

            case RoomType.BigRoom:
                return new string[] 
                {
                    "Assets/Prefabs/Rooms/BigRoom/Dev" 
                };

            case RoomType.GiantRoom:
                return new string[] 
                {
                    "Assets/Prefabs/Rooms/GiantRoom/Dev",
                };

            case RoomType.ElevatorRoom:
                return new string[] 
                {
                    "Assets/Prefabs/Rooms/ElevatorRoom/Dev" 
                };
        }

        throw new ArgumentException("Invalid RoomType");
    }
    public static string[] GetPrefabPaths(PlaceableType placeableType)
    {
        switch (placeableType)
        {
            case PlaceableType.Treasure:
                return new string[] 
                { 
                    "Assets/Prefabs/Placeables/Treasure/Chest0",
                };

            case PlaceableType.Smashable:
                return new string[] 
                { 
                    "Assets/Prefabs/Placeables/Smashable/Barrel0", 
                    "Assets/Prefabs/Placeables/Smashable/Barrel1",
                };
        }

        throw new ArgumentException("Invalid PlaceableType");
    }
    public static string[] GetPrefabPaths(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Small:
                return new string[] 
                { 
                    "Assets/Prefabs/Enemies/Small/SmallEnemy0", 
                };

            case EnemyType.Big:
                return new string[] 
                { 
                    "Assets/Prefabs/Enemies/Big/BigEnemy0" 
                };
        }

        throw new ArgumentException("Invalid EnemyType");
    }
    public static string[] GetPrefabPaths(DecorationType decorationType)
    {
        switch (decorationType)
        {
            case DecorationType.Table:
                return new string[] 
                { 
                    "Assets/Prefabs/Decorations/Table/Table0" 
                };

            case DecorationType.Chair:
                return new string[] 
                {
                    "Assets/Prefabs/Decorations/Chair/"//Missing
                }; 

            case DecorationType.TableTop:
                return new string[] {
                    "Assets/Prefabs/Decorations/TableTop/BottleBlue",
                    "Assets/Prefabs/Decorations/TableTop/BottleGreen",
                    "Assets/Prefabs/Decorations/TableTop/BottleRed",
                    "Assets/Prefabs/Decorations/TableTop/BottleYellow",
                    "Assets/Prefabs/Decorations/TableTop/FlowerVase0",
                    "Assets/Prefabs/Decorations/TableTop/Light0",
                    "Assets/Prefabs/Decorations/TableTop/Teapot",
                    "Assets/Prefabs/Decorations/TableTop/WaterTub",
                };
        }

        throw new ArgumentException("Invalid DecorationType");
    }

    /* AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA WHY
     * Overloads are resolved at compile time, which means T is unknown, therefore the following code doesn't work :)=)))))))))))))))))))))))))))))))))
     * Goofy ass solution is making overloads manually
    public static GameObject GetPrefab<T>(T type)
    {

        string[] paths = GetPrefabPaths(type);
        string path = paths[UnityEngine.Random.Range(0, paths.Length)];

        if (path == "")
        {
            throw new NullReferenceException();
        }

        if (!loadedPrefabs.TryGetValue(path, out _)) loadedPrefabs.Add(path, Resources.Load<GameObject>(path));

        return loadedPrefabs[path];
    }
    * Literally only the paramater is being changed, a generic method must be possible somehow
    */
    public static GameObject GetPrefab(RoomType type)
    {

        string[] paths = GetPrefabPaths(type);
        string path = paths[UnityEngine.Random.Range(0, paths.Length)];

        if (path == "")
        {
            throw new NullReferenceException();
        }

        if (!loadedPrefabs.TryGetValue(path, out _))
        {
            var prefab = Resources.Load<GameObject>(path);
            loadedPrefabs.Add(path, prefab);
        }

        return loadedPrefabs[path];
    }
    public static GameObject GetPrefab(PlaceableType type)
    {

        string[] paths = GetPrefabPaths(type);
        string path = paths[UnityEngine.Random.Range(0, paths.Length)];

        if (path == "")
        {
            throw new NullReferenceException();
        }

        if (!loadedPrefabs.TryGetValue(path, out _)) loadedPrefabs.Add(path, Resources.Load<GameObject>(path));

        return loadedPrefabs[path];
    }
    public static GameObject GetPrefab(EnemyType type)
    {

        string[] paths = GetPrefabPaths(type);
        string path = paths[UnityEngine.Random.Range(0, paths.Length)];

        if (path == "")
        {
            throw new NullReferenceException();
        }

        if (!loadedPrefabs.TryGetValue(path, out _)) loadedPrefabs.Add(path, Resources.Load<GameObject>(path));

        return loadedPrefabs[path];
    }
    public static GameObject GetPrefab(DecorationType type)
    {

        string[] paths = GetPrefabPaths(type);
        string path = paths[UnityEngine.Random.Range(0, paths.Length)];

        if (path == "")
        {
            throw new NullReferenceException();
        }

        if (!loadedPrefabs.TryGetValue(path, out _)) loadedPrefabs.Add(path, Resources.Load<GameObject>(path));

        return loadedPrefabs[path];
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
        //TreasureTrove,
        //EnemyNest
    }
    public enum EnemyType
    {
        Small,
        Big,
    }        
    public enum PlaceableType
    {
        Treasure,
        Smashable,
    }    
    public enum RoomType
    {
        Empty,
        Hallway,
        BigRoom,
        GiantRoom,
        ElevatorRoom,
    }
    public enum DecorationType
    {
        Table,
        Chair,
        TableTop,
    }

}
