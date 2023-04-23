using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] HitscanAttack _attack;
    [SerializeField] Movement _movement;
    [SerializeField] Behavior _behavior;

    [SerializeField] GameObject _player;

    public Vector2 toPlayer { get; private set; }
    HitscanAttack attack => _attack;
    Movement movement => _movement;
    Behavior behavior => _behavior;

    public void Start()
    {
        _player = GameObject.Find("GameManager").GetComponent<GameManager>().player;
        attack.Enable(this);
        movement.Enable(this);
        behavior.Enable(this);

        toPlayer = _player.transform.position - transform.position;
        behavior.Begin();
    }

    public void Update()
    {
        toPlayer = _player.transform.position - transform.position;
    }

    [Serializable]
    class Behavior : Actor.Extension
    {
        [SerializeField] Vector2 _aggroRange;
        [SerializeField] Vector2 _attackRange;

        private Enemy _enemy;
        private State _look;
        private State _move;
        private State _attack;

        public bool inAttack => _enemy.attack.isInProgress;
        public Enemy enemy => _enemy;
        public override Actor actor => _enemy;
        public Vector2 toPlayer => enemy.toPlayer;

        public void Enable(Enemy enemy)
        {
            _enemy = enemy;
            _look = new(OnLook, DuringLook, AfterLook);
            _move = new(OnMove, DuringMove, AfterMove);
            _attack = new(OnAttack, DuringAttack, AfterAttack);

            _look.When(
                () =>
                    Mathf.Abs(toPlayer.x) < _aggroRange.x &&
                    Mathf.Abs(toPlayer.y) < _aggroRange.y,
                _move
            );
            _move.When(
                () =>
                    Mathf.Abs(toPlayer.x) > _aggroRange.x ||
                    Mathf.Abs(toPlayer.y) > _aggroRange.y,
                _look
            );
            _move.When(
                () =>
                    Mathf.Abs(toPlayer.x) < _attackRange.x &&
                    Mathf.Abs(toPlayer.y) < _attackRange.y,
                _attack
            );
            _attack.When(
                () =>
                    !inAttack && (
                        Mathf.Abs(toPlayer.x) > _aggroRange.x ||
                        Mathf.Abs(toPlayer.y) > _aggroRange.y
                    ),
                _look
            );
            _attack.When(
                () =>
                    !inAttack && (
                        Mathf.Abs(toPlayer.x) > _attackRange.x ||
                        Mathf.Abs(toPlayer.y) > _attackRange.y
                    ),
                _move
            );
        }

        public void Begin() => Run(_look);
        public void End() => Abort();

        public void OnLook() => Debug.Log("OnLook");
        public object DuringLook() => null;
        public void AfterLook() { }
        public void OnMove()
        {
            Debug.Log("OnMove");
            enemy.movement.Begin((int)toPlayer.x);
        }
        public object DuringMove() => null;
        public void AfterMove()
        {
            enemy.movement.End();
        }
        public void OnAttack() { AudioManager.instance.PlaySound("BlobWindup"); }
        public object DuringAttack()
        {
            enemy.attack.Begin();
            return null;
        }
        public void AfterAttack() { }
    }

    public override void Damage(Entity source, int damage)
    {
        base.Damage(source, damage);
        AudioManager.instance.PlaySound("Punch");
        AudioManager.instance.PlaySound("BlobTakeDamage");
    }

    //protected class EnemyAttack : HitscanAttack
    //{
    //    private Enemy _enemy;

    //    public override Entity entity => _enemy;
    //}
}
