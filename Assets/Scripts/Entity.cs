using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;

    public new Rigidbody2D rigidbody => _rigidbody;
    public abstract Collider2D bodyCollider { get; }
    public abstract Collider2D feetCollider { get; }
    public abstract float staticDrag { get; }
    public abstract float dynamicDrag { get; }
    public abstract LayerMask platformLayer { get; }
    public abstract LayerMask passthroughPlatformLayer { get; }


    public bool grounded => feetCollider.IsTouchingLayers(platformLayer);
}
