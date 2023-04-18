using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D parentCollider;

    public void AllowPassthroughFor(Collider2D collider)
    {
        Debug.LogFormat("{0} Allowing passthrough for {1}", this, collider);
        Physics2D.IgnoreCollision(parentCollider, collider, true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.LogFormat("{0} Disallowing passthrough for {1}", this, collider);
        Physics2D.IgnoreCollision(collider, parentCollider, false);
    }
}
