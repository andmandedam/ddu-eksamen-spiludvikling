using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Entity : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] private Collider2D _feetCollider;
    

    public new Rigidbody2D rigidbody => _rigidbody;
 
    public virtual int maxHealth { get; protected set; }
    public virtual int curHealth { get; protected set; }

    public Collider2D bodyCollider => _bodyCollider;
    public Collider2D feetCollider => _feetCollider;
    
    public abstract float staticDrag { get; }
    public abstract float dynamicDrag { get; }
    
    public abstract LayerMask platformLayer { get; }

    
    public bool grounded => feetCollider.IsTouchingLayers(platformLayer);

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Damage(Entity source, int damage)
    {
        curHealth = curHealth - damage;
        if (curHealth < 0)
        {
            Die();
        }
    }

    public virtual void Knockback(Entity source, Vector2 dir, Vector2 position)
    {
        rigidbody.AddForceAtPosition(dir, position, ForceMode2D.Impulse);
    }
}
