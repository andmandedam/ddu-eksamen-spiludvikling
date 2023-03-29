using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SampleRoomObject", menuName = "LevelDesign/RoomObject", order = 1)]
public class RoomObject : ScriptableObject
{



    [Header("Spawnables")]
    public GameObject enemyPrefab;
    public int baseEnemyAmount;

    [Header("Room Construction")]
    public GameManager.RoomType roomType;
    public Sprite BackgroundSprite;
    public int wallThickness;

    [Header("Tags")]
    public bool isBossRoom;
    public bool isLootRoom;

}
