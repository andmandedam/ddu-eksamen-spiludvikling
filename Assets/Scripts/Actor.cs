using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
    public static LayerMask PLATFORM_LAYER = 10;

    [Header("Actor")]
    [SerializeField] Animator _animator;
    [SerializeField] Collider2D _feetCollider;
    [SerializeField] float _staticDrag;
    [SerializeField] float _dynamicDrag;
    [SerializeField] LayerMask _platformLayer;

    private HashSet<object> _wantDynamic = new();
    private HashSet<PassthroughTrigger> _passthroughTriggers = new();
    private bool _falling;
    private bool _grounded;
    private bool _wasGrounded;

    public Animator animator => _animator;
    public Collider2D feetCollider => _feetCollider;
    public float staticDrag => _staticDrag;
    public float dynamicDrag => _dynamicDrag;
    public bool falling => _falling;
    public bool grounded => _grounded;
    public LayerMask platformLayer => _platformLayer;
    public virtual Vector2 facing => transform.right; // Since the game is two dimentional, the direction the sprite is facing is transform.right instead of transform.forward

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

    public void Passthrough()
    {
        foreach (var trigger in _passthroughTriggers)
        {
            trigger.AllowPassthroughFor(bodyCollider);
            trigger.AllowPassthroughFor(feetCollider);
        }
    }

    public virtual void FixedUpdate()
    {
        _falling = rigidbody.velocity.y < 0;
        _wasGrounded = _grounded;
        _grounded = feetCollider.IsTouchingLayers(platformLayer);
        if (_grounded && !_wasGrounded) 
        {
            OnLand();
        }
		animator.SetBool("falling", _falling);
		animator.SetBool("grounded", _grounded);
    }

    public virtual void OnLand()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PassthroughTrigger trigger))
        {
            _passthroughTriggers.Add(trigger);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PassthroughTrigger trigger))
        {
            _passthroughTriggers.Remove(trigger);
        }
    }

    public new abstract class Extension : Entity.Extension
    {
        public abstract Actor actor { get; }
        public override Entity entity => actor;
    }
}

