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
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _curHealth;


    public new Rigidbody2D rigidbody => _rigidbody;

    public int maxHealth { get => _maxHealth; protected set => _maxHealth = value; }
    public int curHealth { get => _curHealth; protected set => _curHealth = value; }

    public Collider2D bodyCollider => _bodyCollider;
    public Collider2D feetCollider => _feetCollider;

    public virtual float staticDrag => 10;
    public virtual float dynamicDrag => 0;
    public virtual int grav => 1;

    public abstract LayerMask platformLayer { get; }

    public bool grounded => feetCollider.IsTouchingLayers(platformLayer);

    private HashSet<object> _wantDynamic = new();

    public void RequestDynamicDrag(object obj)
    {
        _wantDynamic.Add(obj);
        UpdateDragState();
    }

    public void RequestStaticDrag(object obj)
    {
        _wantDynamic.Remove(obj);
        UpdateDragState();
    }

    private void UpdateDragState()
    {
        if (_wantDynamic.Count == 0 && grounded)
        {
            rigidbody.drag = staticDrag;
        }
        else
        {
            rigidbody.drag = dynamicDrag;
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Damage(Entity source, int damage)
    {
        if (source == this) return;

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
