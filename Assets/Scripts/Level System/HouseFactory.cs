using System;
using UnityEngine;

public partial class GameManager
{
    class HouseFactory
    {
        private const float EnemyEdgeMargin = 1f; //The minimum distance an enemy can be placed from edge of a room
        private const float SmashableSpawnRate = 0.6f; // F.x. Barrels or vases
        private const float WallWidth = 4f / 32f;

        public static void GenerateHouse(GameManager gameManager)
        {
            HouseTemplate houseTemplate = Util.RandomEnum.RandomEnumValue<HouseTemplate>();
            RoomObject[] rooms = LevelGenerationInfo.GetRoomsInHouseTemplate(houseTemplate, gameManager); // Chose random HouseTemplate
            gameManager.rooms = new GameObject[rooms.Length];

            for (int i = 0; i < rooms.Length; i++)
            {
                gameManager.rooms[i] = CreateRoom(rooms[i], gameManager);
                gameManager.rooms[i].name = (i + 1).ToString();
            }
            CreateHouseBorder(houseTemplate, gameManager);
        }

        private static void CreateHouseBorder(HouseTemplate houseTemplate, GameManager gameManager)
        {
            Vector2 houseSize = LevelGenerationInfo.GetHouseTemplateSize(houseTemplate);
            GameObject leftEdge = Instantiate(gameManager.levelBorder, gameManager.roomParent);
            leftEdge.transform.localScale = new Vector3(1, houseSize.y, 1);
            leftEdge.transform.position = new Vector3(-1f, houseSize.y / 2, 0);

            GameObject rightEdge = Instantiate(gameManager.levelBorder, gameManager.roomParent);
            rightEdge.transform.localScale = new Vector3(1, houseSize.y, 1);
            rightEdge.transform.position = new Vector3(houseSize.x + 1, houseSize.y / 2, 0);

            GameObject topEdge = Instantiate(gameManager.levelBorder, gameManager.roomParent);
            topEdge.transform.localScale = new Vector3(houseSize.x, 1, 1);
            topEdge.transform.position = new Vector3(houseSize.x / 2, houseSize.y + 1, 0);

            GameObject bottomEdge = Instantiate(gameManager.levelBorder, gameManager.roomParent);
            bottomEdge.transform.localScale = new Vector3(houseSize.x, 2, 1);
            bottomEdge.transform.position = new Vector3(houseSize.x / 2, -1, 1);
        }

        public static GameObject CreateRoom(RoomObject room, GameManager gameManager)
        {
            var roomObject = Instantiate(room.roomPrefab, gameManager.roomParent, true);
            roomObject.transform.position = (Vector3Int)room.placement;
            PlaceEnemies(room, gameManager);

            // Handle Room tags
            if ((room.tags & RoomTag.Exit) != 0)
            {
                GameObject nextLevelDoor = Instantiate(gameManager.nextLevelDoor, roomObject.transform);
                nextLevelDoor.transform.localPosition = new Vector3(LevelGenerationInfo.GetRoomTypeSize(room.roomType).x - WallWidth, nextLevelDoor.transform.localScale.y/2); //
                
                nextLevelDoor.TryGetComponent<NextLevelScript>(out var script);
                script.gameManager = gameManager;
            }

            if ((room.tags & RoomTag.ElevatorBottom) != 0)
            {
                roomObject.TryGetComponent<Elevator>(out var elevatorScript);
                elevatorScript.isBottom = true;
            }

            if ((room.tags & RoomTag.ElevatorTop) != 0)
            {
                roomObject.TryGetComponent<Elevator>(out var elevatorScript);
                elevatorScript.isTop = true;
            }
            
            // Handle Spawnables
            if (!roomObject.TryGetComponent<RoomInfo>(out var roomInfo))
            {
                return roomObject;
                throw new Exception("No RoomInfo on: " + roomObject + " Created from: " + room);
            }
            
            for (int i = 0; i < roomInfo.placeableSpawnPoint.Length; i++)
            {
                if ((room.tags & RoomTag.Loot) != 0)
                {
                    Instantiate(Util.ChooseRandomElement(gameManager.prefabDictionary[PlaceableType.Treasure]), roomObject.transform).transform.position =
                        roomInfo.placeableSpawnPoint[i].position;
                    room.tags ^= RoomTag.Loot;
                    continue;
                }
                if (UnityEngine.Random.value < gameManager.spawnChanceDictionary[PlaceableType.Treasure])
                {
                    Instantiate(Util.ChooseRandomElement(gameManager.prefabDictionary[PlaceableType.Treasure]), roomObject.transform).transform.position =
                        roomInfo.placeableSpawnPoint[i].position;
                    continue;
                }
                if (UnityEngine.Random.value < gameManager.spawnChanceDictionary[PlaceableType.Smashable])
                {
                    Instantiate(Util.ChooseRandomElement(gameManager.prefabDictionary[PlaceableType.Smashable]), roomObject.transform).transform.position = 
                        roomInfo.placeableSpawnPoint[i].position;
                }
            }

            for (int i = 0; i < roomInfo.hasSurfaceSpawnPoint.Length; i++)
            {
                if (UnityEngine.Random.value < gameManager.spawnChanceDictionary[DecorationType.HasSurface])
                {
                    Instantiate(Util.ChooseRandomElement(gameManager.prefabDictionary[DecorationType.HasSurface]), roomObject.transform).transform.position =
                    roomInfo.hasSurfaceSpawnPoint[i].position;

                    for (int j = 0; j < roomInfo.onSurfaceSpawnPoint.Length; j++)
                    {
                        Instantiate(Util.ChooseRandomElement(gameManager.prefabDictionary[DecorationType.OnSurface]), roomObject.transform).transform.position =
                            roomInfo.onSurfaceSpawnPoint[j].position;
                    }
                }
            }

            PlaceSpawnables(DecorationType.Ground, roomInfo.groundDecorationSpawnPoint, roomObject, gameManager);
            PlaceSpawnables(DecorationType.Wall, roomInfo.wallDecorationSpawnPoint, roomObject, gameManager);
            PlaceSpawnables(DecorationType.Roof, roomInfo.roofDecorationSpawnPoint, roomObject, gameManager);
            PlaceSpawnables(DecorationType.Light, roomInfo.lightSpawnPoint, roomObject, gameManager);

            return roomObject;
        }
        private static void PlaceSpawnables(Enum @enum, Transform[] decorationSpawnPoint, GameObject roomObject, GameManager gameManager)
        {
            for (int i = 0; i < decorationSpawnPoint.Length; i++)
            {
                if (UnityEngine.Random.value < gameManager.spawnChanceDictionary[@enum])
                {
                    Instantiate(Util.ChooseRandomElement(gameManager.prefabDictionary[@enum]), roomObject.transform).transform.position =
                    decorationSpawnPoint[i].position;
                }
            }
        }
        private static void PlaceEnemies(RoomObject room, GameManager gameManager)
        {
            var roomSize = LevelGenerationInfo.GetRoomTypeSize(room.roomType);

            for (int i = 0; i < room.enemyAmount; i++)
            {
                var enemyPosition = new Vector3(
                    Math.Clamp(UnityEngine.Random.value * roomSize.x, EnemyEdgeMargin, roomSize.x - EnemyEdgeMargin),
                    Math.Clamp(UnityEngine.Random.value * roomSize.y, EnemyEdgeMargin, roomSize.y - EnemyEdgeMargin)
                        );

                Instantiate(room.enemyPrefab, gameManager.enemyParent, true).transform.position = (Vector3Int)room.placement + enemyPosition;
            }
        }
    }

}
