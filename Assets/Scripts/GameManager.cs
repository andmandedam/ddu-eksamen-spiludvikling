using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        player = Instantiate(playerPrefab);
        roomParent = new GameObject("House").transform;
        enemyParent = new GameObject("EnemyParent").transform;
        mainCameraScript = Instantiate(mainCameraPrefab).GetComponent<GenericCamera>();
        mainCameraScript.targets = new Transform[] { player.transform };

    }

    public Camera mainCameraPrefab;
    public GenericCamera mainCameraScript;
    public GameObject playerPrefab;
    [NonSerialized] public GameObject player;
    public GameObject[] rooms;
    public GameObject levelBorder;
    public GameObject nextLevelDoor;
    public int levelCompletionScore;

    [Header("  Spawning")]
    public float hardRoomMultiplier;
    Transform roomParent;
    Transform enemyParent;

    [SerializeField] float smashableSpawnChance;
    [SerializeField] float treasureSpawnChance;
    [SerializeField] float hasSurfaceSpawnChance;
    [SerializeField] float groundSpawnChance;
    [SerializeField] float roofSpawnChance;
    [SerializeField] float wallSpawnChance;
    [SerializeField] float lightSpawnChance;

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
    [SerializeField] GameObject[] colliders;
    [SerializeField] GameObject[] hallways;
    [SerializeField] GameObject[] bigRooms;
    [SerializeField] GameObject[] giantRooms;
    [SerializeField] GameObject[] elevatorRooms;

    [Header("  Decorations")]
    [SerializeField] GameObject[] ground;
    [SerializeField] GameObject[] hasSurface;
    [SerializeField] GameObject[] onSurface;
    [SerializeField] GameObject[] roof;
    [SerializeField] GameObject[] wall;
    [SerializeField] GameObject[] lights;

    Dictionary<Enum, GameObject[]> prefabDictionary = new Dictionary<Enum, GameObject[]>();
    Dictionary<Enum, float> enemySpawnStatsDictionary = new Dictionary<Enum, float>();
    Dictionary<Enum, float> spawnChanceDictionary = new Dictionary<Enum, float>();

    private float levelNumber = 0;

    void Start()
    {
        PersistantObject.instance.playerControls.actions.HUD.Disable();
        PersistantObject.instance.playerControls.actions.NinjaOnFoot.Enable();

        Physics2D.IgnoreLayerCollision(3, 3, true);

        prefabDictionary.Add(EnemyType.Small, smallEnemies);
        prefabDictionary.Add(EnemyType.Big, bigEnemies);
        prefabDictionary.Add(PlaceableType.Treasure, treasures);
        prefabDictionary.Add(PlaceableType.Smashable, smashables);
        prefabDictionary.Add(RoomType.Collider, colliders);
        prefabDictionary.Add(RoomType.Hallway, hallways);
        prefabDictionary.Add(RoomType.BigRoom, bigRooms);
        prefabDictionary.Add(RoomType.GiantRoom, giantRooms);
        prefabDictionary.Add(RoomType.ElevatorRoom, elevatorRooms);
        prefabDictionary.Add(DecorationType.HasSurface, hasSurface);
        prefabDictionary.Add(DecorationType.OnSurface, onSurface);
        prefabDictionary.Add(DecorationType.Ground, ground);
        prefabDictionary.Add(DecorationType.Wall, wall);
        prefabDictionary.Add(DecorationType.Roof, roof);
        prefabDictionary.Add(DecorationType.Light, lights);

        enemySpawnStatsDictionary.Add(RoomType.Collider, 0f); //Not an actual room, therefore 0 enemies should spawn.
        enemySpawnStatsDictionary.Add(RoomType.Hallway, hallwayBaseEnemyAmount);
        enemySpawnStatsDictionary.Add(RoomType.BigRoom, bigRoomBaseEnemyAmount);
        enemySpawnStatsDictionary.Add(RoomType.GiantRoom, giantRoomBaseEnemyAmount);
        enemySpawnStatsDictionary.Add(RoomType.ElevatorRoom, elevatorRoomBaseEnemyAmount);

        spawnChanceDictionary.Add(PlaceableType.Treasure, treasureSpawnChance);
        spawnChanceDictionary.Add(PlaceableType.Smashable, smashableSpawnChance);

        spawnChanceDictionary.Add(DecorationType.HasSurface, hasSurfaceSpawnChance);
        spawnChanceDictionary.Add(DecorationType.Ground, groundSpawnChance);
        spawnChanceDictionary.Add(DecorationType.Roof, roofSpawnChance);
        spawnChanceDictionary.Add(DecorationType.Wall, wallSpawnChance);
        spawnChanceDictionary.Add(DecorationType.Light, lightSpawnChance);

        HouseFactory.GenerateHouse(this);
    }
    public void NextLevel()
    {
        Destroy(roomParent.gameObject);
        Destroy(enemyParent.gameObject);
        levelNumber++;
        ScoreUI.instance.IncrementScore(levelCompletionScore);
        roomParent = new GameObject("House" + levelNumber).transform;
        enemyParent = new GameObject("EnemyParent").transform;
        enemyParent.parent = roomParent;
        HouseFactory.GenerateHouse(this);
        player.transform.position = new Vector3(2, 0, 0);
    }

    public enum RoomTag
    {
        None = 0,
        Boss = 1 << 0,
        Loot = 1 << 1,
        Hard = 1 << 2,
        BigEnemies = 1 << 3,
        Exit = 1 << 4,
        ElevatorTop = 1 << 5,
        ElevatorBottom = 1 << 6,
    }
    public enum HouseTemplate
    {
        RegularHouse,
        Outside,
        LargeHouse,
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
        Collider,
        Hallway,
        BigRoom,
        GiantRoom,
        ElevatorRoom,
    }
    public enum DecorationType
    {
        HasSurface,
        Ground,
        OnSurface,
        Wall,
        Roof,
        Light,
    }
}
