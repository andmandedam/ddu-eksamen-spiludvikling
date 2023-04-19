using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class GameManager : MonoBehaviour
{
    public float hardRoomMultiplier;
    public GameObject[] rooms;
    [Header("  Spawning")]
    [SerializeField] float smashableSpawnChance;
    [SerializeField] float treasureSpawnChance;

    [SerializeField] float hallwayBaseEnemyAmount;
    [SerializeField] float bigRoomBaseEnemyAmount;
    [SerializeField] float giantRoomBaseEnemyAmount;
    [SerializeField] float elevatorRoomBaseEnemyAmount;
    


    [Header("  Enemies")]
    [SerializeField] GameObject[] smallEnemies;
    [SerializeField] GameObject[] bigEnemies;

    [Header("  Placeables")]
    [SerializeField] GameObject[] treasures;
    [SerializeField] GameObject[] smashables;
    
    [Header("  Rooms")]
    [SerializeField] GameObject[] hallways;
    [SerializeField] GameObject[] bigRooms;
    [SerializeField] GameObject[] giantRooms;
    [SerializeField] GameObject[] elevatorRooms;

    [Header("  Decorations")]
    [SerializeField] GameObject[] tables;
    [SerializeField] GameObject[] chairs;
    [SerializeField] GameObject[] tableTops;

    Dictionary<Enum, GameObject[]> prefabDictionary = new Dictionary<Enum, GameObject[]>();
    Dictionary<Enum, float> enemySpawnStatsDictionary = new Dictionary<Enum, float>();
    Dictionary<Enum, float> placeableStatsDictionary = new Dictionary<Enum, float>();


    void Start()
    {
        prefabDictionary.Add(EnemyType.Small, smallEnemies);
        prefabDictionary.Add(EnemyType.Big, bigEnemies);
        prefabDictionary.Add(PlaceableType.Treasure, treasures);
        prefabDictionary.Add(PlaceableType.Smashable, smashables);
        prefabDictionary.Add(RoomType.Hallway, hallways);
        prefabDictionary.Add(RoomType.BigRoom, bigRooms);
        prefabDictionary.Add(RoomType.GiantRoom, giantRooms);
        prefabDictionary.Add(RoomType.ElevatorRoom, elevatorRooms);

        enemySpawnStatsDictionary.Add(RoomType.Hallway, hallwayBaseEnemyAmount);
        enemySpawnStatsDictionary.Add(RoomType.BigRoom, bigRoomBaseEnemyAmount);
        enemySpawnStatsDictionary.Add(RoomType.GiantRoom, giantRoomBaseEnemyAmount);
        enemySpawnStatsDictionary.Add(RoomType.ElevatorRoom, elevatorRoomBaseEnemyAmount);

        placeableStatsDictionary.Add(PlaceableType.Treasure, treasureSpawnChance);
        placeableStatsDictionary.Add(PlaceableType.Smashable, smashableSpawnChance);

        HouseFactory.GenerateHouse(this);
    }

    public class RoomObject
    {
        public Vector2Int placement;
        public GameObject enemyPrefab;
        public int enemyAmount;
        public GameObject roomPrefab;
        public RoomTag tags;
        public GameManager.RoomType roomType;

        public RoomObject(RoomType roomType, RoomTag flag, Vector2Int placement, GameManager gameManager)
        {
            this.roomType = roomType;
            this.tags = flag;
            this.placement = placement;

            this.roomPrefab = ChooseRandomElement(gameManager.prefabDictionary[roomType]);
            this.enemyAmount = (int)gameManager.enemySpawnStatsDictionary[roomType];
            if ((this.tags & RoomTag.Hard) != 0)
            {
                this.enemyAmount = (int)(gameManager.hardRoomMultiplier * this.enemyAmount) + 1;
            }

            if ((this.tags & RoomTag.BigEnemies) != 0)
            {
                this.enemyPrefab = ChooseRandomElement(gameManager.prefabDictionary[EnemyType.Big]);
                this.enemyAmount = (int)(this.enemyAmount * 0.5);
            } else {
                this.enemyPrefab = ChooseRandomElement(gameManager.prefabDictionary[EnemyType.Small]);            
            }
        }
    }
    
    public static T ChooseRandomElement<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
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

        public static void GenerateHouse(GameManager gameManager)
        {
            RoomObject[] rooms = GetRoomsInHouseTemplate(RandomEnum.RandomEnumValue<HouseTemplate>(), gameManager); // Chose random HouseTemplate
            gameManager.rooms = new GameObject[rooms.Length];
            for (int i = 0; i < rooms.Length; i++)
            {
                gameManager.rooms[i] = CreateRoom(rooms[i], gameManager);
            }
        }
        public static GameObject CreateRoom(RoomObject room, GameManager gameManager)
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
                    Instantiate(ChooseRandomElement(gameManager.prefabDictionary[PlaceableType.Smashable])).transform.position = 
                        roomInfo.placeableSpawnPoint[i] + (Vector3Int)room.placement;
                }
            }

            for (int i = 0; i < roomInfo.tableTopDecorationSpawnPoint.Length; i++)
            {
                if (UnityEngine.Random.value < SmashableSpawnRate)
                {
                    Instantiate(ChooseRandomElement(gameManager.prefabDictionary[DecorationType.TableTop])).transform.position = 
                        roomInfo.tableTopDecorationSpawnPoint[i] + (Vector3Int)room.placement;
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
    public static RoomObject[] GetRoomsInHouseTemplate(HouseTemplate template, GameManager gameManager)
    {
        switch (template)
        {
            case HouseTemplate.RegularHouse:
                return new RoomObject[]
                {
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(0,0), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.None, new Vector2Int(8,0), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,0), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,4), gameManager),
                    new RoomObject(RoomType.ElevatorRoom, RoomTag.None, new Vector2Int(16,8), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(0,4), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.Hard, new Vector2Int(18,8), gameManager),
                    new RoomObject(RoomType.Hallway, RoomTag.Loot | RoomTag.Hard, new Vector2Int(26,8), gameManager),
                    new RoomObject(RoomType.BigRoom, RoomTag.None, new Vector2Int(18,0), gameManager),
                };

            case HouseTemplate.Outside:
                return new RoomObject[]
                {
                    new RoomObject(RoomType.GiantRoom, RoomTag.None, new Vector2Int(0,0), gameManager),
                };
        }
        throw new ArgumentException("Invalid HouseTemplate");
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
