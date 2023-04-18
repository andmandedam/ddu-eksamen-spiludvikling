using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D parentCollider;
    [SerializeField] private float buffer;

    public void AllowPassthroughFor(Collider2D collider)
    {
        Physics2D.IgnoreCollision(parentCollider, collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.position.y < parentCollider.transform.position.y + buffer)
        {
            AllowPassthroughFor(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Physics2D.IgnoreCollision(parentCollider, collider, false);
    }
}
