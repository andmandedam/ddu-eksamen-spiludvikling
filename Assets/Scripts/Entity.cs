using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Collider2D _bodyCollider;
    [SerializeField] int _maxHealth;
    [SerializeField] int _curHealth;

    public new Rigidbody2D rigidbody => _rigidbody;
    public Collider2D bodyCollider => _bodyCollider;
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int curHealth { get => _curHealth; set => _curHealth = value; }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Damage(Entity source, int damage)
    {
        if (source == this) return;

        curHealth = curHealth - damage;
        if (curHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Knockback(Entity source, Vector2 dir, Vector2 position)
    {
        rigidbody.AddForceAtPosition(dir, position, ForceMode2D.Impulse);
    }

    // An extension is a state machine, it can only be in one of it states at the time.
    // An extension should be some action that an actor can perform that takes some period of time.
    public abstract class Extension : State.Machine
    {
        public abstract Entity entity { get; }

        public override MonoBehaviour runner => entity;
        public Rigidbody2D rigidbody => entity.rigidbody;
        public Transform transform => entity.transform;
    }
}
