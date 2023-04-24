using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [Header("Droping")]
    [SerializeField] GameObject healthPotPrefab;
    [SerializeField] float healthPotDropChance;

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
        [SerializeField] Vector2 _attackRange;
        [SerializeField] Vector2 _aggroRange;
        [SerializeField] private float _swapChance;
        [SerializeField] private float _stopChance;

        private int _movDir;

        private Enemy _enemy;
        private State idle;
        private State _move;
        private State _attack;

        public bool inAttack => _enemy.attack.isInProgress;
        public Enemy enemy => _enemy;
        public override Actor actor => _enemy;
        public Vector2 toPlayer => enemy.toPlayer;

        public void Enable(Enemy enemy)
        {
            _enemy = enemy;
            idle = new(OnIdle, DuringIdle, AfterIdle);
            _move = new(OnMove, DuringMove, AfterMove);
            _attack = new(OnAttack, DuringAttack, AfterAttack);
            _movDir = UnityEngine.Random.value > 0.5f ? 1 : -1;

            idle.When(
                () =>
                    Mathf.Abs(toPlayer.x) < _aggroRange.x &&
                    Mathf.Abs(toPlayer.y) < _aggroRange.y,
                _move
            );
            _move.When(
                () =>
                    Mathf.Abs(toPlayer.x) > _aggroRange.x ||
                    Mathf.Abs(toPlayer.y) > _aggroRange.y,
                idle
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
                idle
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

        public void Begin() => Run(idle);
        public void End() => Abort();

        public void OnIdle() {}
        public object DuringIdle()
        {
            _movDir *= (UnityEngine.Random.value > _swapChance) ? -1 : 1;
            if (UnityEngine.Random.value > _stopChance)
            {
                enemy.movement.End();
            }
            else
            {
                enemy.movement.Begin(_movDir);
            }
            return new WaitForSeconds(2.5f);
        }
        public void AfterIdle() { }
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

    public override void Die()
    {
        base.Die();
        if (UnityEngine.Random.value < healthPotDropChance)
        {
            OnDeathDrop(healthPotPrefab);
        }
    }

    private void OnDeathDrop(GameObject healthPotPrefab)
    {
        Instantiate(healthPotPrefab).transform.position = transform.position;
    }
    //protected class EnemyAttack : HitscanAttack
    //{
    //    private Enemy _enemy;

    //    public override Entity entity => _enemy;
    //}
}
