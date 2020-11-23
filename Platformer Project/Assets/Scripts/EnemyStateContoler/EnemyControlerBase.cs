using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyControlerBase : MonoBehaviour
{
    protected Rigidbody2D _enemyRB;
    protected Animator _enemyAnimator;
    [Header("Canvas")]
    [SerializeField] GameObject _canvas;

    [Header("HP")]
    [SerializeField] protected int _maxHp;
    [SerializeField] protected Slider _hpSlider;
    protected int _currentHp;

    [Header("StateCganges")]
    [SerializeField] private float _maxStateTime;
    [SerializeField] private float _minStateTime;
    [SerializeField] private EnemyState[] _avaibleState;
    protected EnemyState _currentState;
    protected float _lastStateChage;
    protected float _timeToNextChange;
   
    [Header("Movment")]
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private Transform _groundCheak;
    [SerializeField] private LayerMask _whatIsGround;
    protected Vector2 _startPoint;
    protected bool _faceRight = true;

    [Header("Damage dealer")]
    [SerializeField] private DamageType _collisionDamageType;
    [SerializeField] protected int _collisionDamage;
    [SerializeField] protected float _collisionTimeDelay;
    private float _lastDamageTime;

    protected virtual void Start()
    {
        _startPoint = transform.position;
        _enemyRB = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
        _currentHp = _maxHp;
        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _maxHp;
    }

    // Update is called once per frame
   protected virtual void FixedUpdate()
    {
        if (_currentState == EnemyState.Death)
            return;

        if (IsGroundEnding())
            Flip();

        if (_currentState == EnemyState.Move)
            Move();
    }
    protected virtual void Update()
    {
        if (_currentState == EnemyState.Death)
            return;

        if (Time.time - _lastStateChage > _timeToNextChange)
            GetRandomState();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentState == EnemyState.Death)
            return;

        TryToDamage(collision.collider);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_range * 2, 0.5f, 0));
    }
    #region PublicVethod
    public virtual void TakeDamage(int damage, DamageType type = DamageType.Casuel, Transform player =null) 
    {
        if (_currentState == EnemyState.Death)
            return;

        _currentHp -= damage;
        Debug.Log(String.Format("Enemy {0} take damage {1} and his currentHp = {2}", gameObject, damage, _currentHp));

        if (_currentHp <= 0)
        {
           // _currentHp = 0;
            
            ChangeState(EnemyState.Death);
            //return;
            
        }
        _hpSlider.value = _currentHp;
    }
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
    protected virtual void EndState()
    {
        if (_currentState == EnemyState.Death)
            OnDeath();

        ResetState();
    }
    
    #endregion
    protected virtual void ResetState()
    {
        _enemyAnimator.SetBool(EnemyState.Move.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Death.ToString(), false);
       
    }
    protected virtual void DisableEnemy()
    {
        _enemyRB.velocity = Vector2.zero;
        _enemyRB.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }
    protected virtual void ChangeState(EnemyState state)
    {
     
        if (_currentState == EnemyState.Death)
            return;

        ResetState();
        _currentState = EnemyState.Idle;

      
        if(state != EnemyState.Idle)
        _enemyAnimator.SetBool(state.ToString(), true);

        _currentState = state;
        _lastStateChage = Time.time;

        switch (_currentState)
        {
            case EnemyState.Idle:
                _enemyRB.velocity = Vector2.zero;
                break;
            case EnemyState.Death:
               DisableEnemy();
                break;
        }
    
             
    }
   
    protected void GetRandomState()
    {
        if (_currentState == EnemyState.Death)
            return;

        int state = UnityEngine.Random.Range(0, _avaibleState.Length);

        if (_currentState == EnemyState.Idle && _avaibleState[state] == EnemyState.Idle)
        {
            GetRandomState();
        }

        _timeToNextChange = UnityEngine.Random.Range(_minStateTime, _maxStateTime);
        ChangeState(_avaibleState[state]);
    }
    protected virtual void TryToDamage(Collider2D enemy)
    {
        if (_currentState == EnemyState.Death)
            return;

        if ((Time.time - _lastDamageTime) < _collisionTimeDelay)
            return;

        PlayerItems player = enemy.GetComponent<PlayerItems>();
        if (player != null)
        {
            player.TakeTanage(_collisionDamage, _collisionDamageType, transform);
            _lastDamageTime = Time.time;
        }
    }
    protected virtual void Move()
    {
        _enemyRB.velocity = transform.right * new Vector2(_speed, _enemyRB.velocity.y);
    }
    protected void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _canvas.transform.Rotate(0, 180, 0);
    }
    private bool IsGroundEnding()
    {
        return !Physics2D.OverlapPoint(_groundCheak.position, _whatIsGround);
    }

}
public enum EnemyState{
    Idle,
    Move,
    Shoot,
    Strike,
    PowerStrike,
    Hurt,
    Death,
}
