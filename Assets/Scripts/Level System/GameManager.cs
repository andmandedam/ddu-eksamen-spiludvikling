using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum RoomType
    {
        LongRoom,
        ShortRoom,
        GiantRoom,
        ElevatorRoom,
    }

    public Vector2Int[] roomTypeSize = {
        new Vector2Int(12, 4),
        new Vector2Int(8, 4),
        new Vector2Int(24, 8),
        new Vector2Int(4, 8)
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
