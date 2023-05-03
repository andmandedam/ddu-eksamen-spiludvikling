using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistantObject : MonoBehaviour
{
    public Player.PlayerControls playerControls;
    public static PersistantObject instance;
    void Awake()
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
        playerControls = new Player.PlayerControls();
        playerControls.Initialize();
    }
}
