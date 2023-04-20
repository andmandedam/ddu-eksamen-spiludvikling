using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] public Transform[] placeableSpawnPoint;
    [SerializeField] public Transform[] hasSurfaceSpawnPoint;
    [SerializeField] public Transform[] onSurfaceSpawnPoint;
    [SerializeField] public Transform[] groundDecorationSpawnPoint;
    [SerializeField] public Transform[] wallDecorationSpawnPoint;
    [SerializeField] public Transform[] roofDecorationSpawnPoint;
    [SerializeField] public Transform[] lightSpawnPoint;
}
