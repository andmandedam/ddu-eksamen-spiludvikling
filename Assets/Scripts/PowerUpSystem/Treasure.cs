using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private float powerUpSpawnSpeed = 1f;
    [SerializeField] private GameObject[] PowerUpPrefabs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            return;
        }
        var powerUp = Instantiate(Util.ChooseRandomElement(PowerUpPrefabs));
        Debug.Log("Spawning PowerUp: " +  powerUp);
        powerUp.transform.position = transform.position + Vector3.up;
        powerUp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.value, 1) * powerUpSpawnSpeed);

        Destroy(gameObject);
    }
}
