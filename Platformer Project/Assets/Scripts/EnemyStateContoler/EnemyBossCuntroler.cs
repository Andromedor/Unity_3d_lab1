using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossCuntroler : EnemyArcherController
{
   

   // private bool _inAtackRange;
    [Header("Strike")]
    [SerializeField] private Transform _stirkepoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;

    [Header("PowerStrike")]
    [SerializeField] private Collider2D _strikeCollider;
    [SerializeField] private int _powerStrikeDamage;
    [SerializeField] private float _powerStrikeRange;
    [SerializeField] private float _powerStrikeSpeed;

    [Header("Transition")]
    [SerializeField] private float _waitTime;

    private float _curentStrikeRange;
    private bool _fightStarted;
    private bool _inRage;
    private EnemyState _stateOnHold;

    private EnemyState[] _attackState = {EnemyState.Strike/*,EnemyState.PowerStrike,EnemyState.Shoot*/ };
    protected override void Start()
    {
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(_currentState == EnemyState.Move && _attacking)
        {
            TurnToPlayer();
            if (CanAttack())
            {
                ChangeState(_stateOnHold);
            }
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_stirkepoint.position, new Vector3(_strikeRange, _strikeRange, 0));
    }
    #region PulicMethod
    public override void TakeDamage(int damage, DamageType type = DamageType.Casuel, Transform player = null)
    {
        if (_currentState == EnemyState.PowerStrike && type != DamageType.Projectile || _currentState ==EnemyState.Hurt)
            return;

        base.TakeDamage(damage, type, player);

        if(_currentHp <= _maxHp/ 2 && !_inRage)
        {
            _inRage = true;
            ChangeState(EnemyState.Hurt);
        }
    }

    #endregion
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (_currentState)
        {
            case EnemyState.PowerStrike:
            case EnemyState.Strike:
                _attacking = true;
                _curentStrikeRange = state == EnemyState.Strike ? _strikeRange : _powerStrikeRange;
                _enemyRB.velocity = Vector2.zero;
                if (!CanAttack())
                {
                    _stateOnHold = state;
                    _enemyAnimator.SetBool(_currentState.ToString(), false);
                    ChangeState(EnemyState.Move);
                }
                break;
            case EnemyState.Hurt:
                _attacking = false;
                _enemyRB.velocity = Vector2.zero;
                StopCoroutine("BeginNewCircle");
                break;
        }

    }
    protected void Strike()
    {
        Collider2D player = Physics2D.OverlapBox(_stirkepoint.position, new Vector2(_strikeRange, _strikeRange), 0, _enemies);
        if(player != null)
        {
            PlayerItems playerItems = player.GetComponent<PlayerItems>();
            int damage = _inRage ? _damage * 2 : _damage;
            if (playerItems != null)
                playerItems.TakeTanage(damage);
        }
    }
    protected void StrikeWithPower()
    {
        _strikeCollider.enabled = true;
        _enemyRB.velocity = transform.right * _powerStrikeSpeed;
    }

    protected void EndPowerStike()
    {
        _strikeCollider.enabled = false;
        _enemyRB.velocity = Vector2.zero;
    }

    protected override void ChekPlayerInRange()
    {
        if (_player == null || _isAngry)
            return;

        if (Vector2.Distance(transform.position, _player.transform.position) < _angerRange)
        {
            _isAngry = true;
            if (!_fightStarted)
            {
                StopAllCoroutines();
                StartCoroutine(BeginNewCircle());
               
            }
      
        }
        else
            _isAngry = false; 
    }
  
    protected override void TryToDamage(Collider2D enemy)
    {
        if (_currentState == EnemyState.Idle || _currentState == EnemyState.Shoot || _currentState == EnemyState.Hurt)
            return;

        base.TryToDamage(enemy);
    }
    private bool CanAttack()
    {
        return Vector2.Distance(transform.position, _player.transform.position) < _curentStrikeRange;
            
    }
    protected override void DoStateAction()
    {
        base.DoStateAction();
        switch (_currentState)
        {
            case EnemyState.Strike:
                Strike();
                break;
            case EnemyState.PowerStrike:
                StrikeWithPower();
                break;
        }
    }
    protected override void EndState()
    {
       
        switch (_currentState)
        {
            case EnemyState.PowerStrike:
                EndPowerStike();
                _attacking = false;
                break;
            case EnemyState.Strike:
                _attacking = false;
                break;
            case EnemyState.Hurt:
                _enemyAnimator.SetBool("Rage", true);
                _fightStarted = false;
                break;
        }
        base.EndState();

        if (_currentState == EnemyState.Shoot || _currentState == EnemyState.PowerStrike || _currentState == EnemyState.Strike || _currentState == EnemyState.Hurt)
        {
            StartCoroutine(BeginNewCircle());
        }
    }
    protected override void ResetState()
    {
        base.ResetState();
        _enemyAnimator.SetBool(EnemyState.PowerStrike.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Strike.ToString(), false);

    }
    private IEnumerator BeginNewCircle()
    {
        if (_currentState == EnemyState.Death)
            yield break;

        if (_fightStarted)
        {
            _isAngry = false;
            ChekPlayerInRange();

            if (!_isAngry)
            {
                _fightStarted = false;
                StartCoroutine(ScanForPlayer());
                yield break; 
            }
            yield return new WaitForSeconds(_waitTime);
        }
        _fightStarted = true;
        TurnToPlayer();
        ChooseNwxtAttackState();
    }
    protected void ChooseNwxtAttackState()
    {
        int state = Random.Range(0, _attackState.Length);
        ChangeState(_attackState[state]);
    }
}
