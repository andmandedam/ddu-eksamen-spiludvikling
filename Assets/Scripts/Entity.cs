using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{
    public new Rigidbody2D rigidbody;

    public abstract Collider2D bodyCollider { get; }
    public abstract Collider2D feetCollider { get; }
    public abstract float staticDrag { get; }
    public abstract float dynamicDrag { get; }
    public abstract LayerMask platformLayer { get; }
    public abstract LayerMask passthroughPlatformLayer { get; }


    public bool grounded => feetCollider.IsTouchingLayers(platformLayer);
}
