using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcherController : EnemyControlerBase
{
    protected PlayerItems _player;

    [Header("Shooting")]
    [SerializeField] private GameObject _projectilPrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] public float _angerRange;

    protected bool _isAngry;
    protected bool _attacking;

    

    protected override void Start()
    {
        base.Start();
        _player = FindObjectOfType<PlayerItems>();
        StartCoroutine(ScanForPlayer());
    }
    protected override void Update()
    {
        if (_isAngry)
            return;
        base.Update();
    }
    
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);

        switch (state)
        {
            case EnemyState.Shoot:
                _attacking = true;
                _enemyRB.velocity = Vector2.zero;
                break;
        }
    }
    protected override void EndState()
    {
       
        switch (_currentState)
        {
            case EnemyState.Shoot:
                _attacking = false;
                break;
        }
        base.EndState();
    }
    protected override void ResetState()
    {
        base.ResetState();
        _enemyAnimator.SetBool(EnemyState.Shoot.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Hurt.ToString(), false);
    }

    protected virtual void DoStateAction()
    {
        switch (_currentState) {
            case EnemyState.Shoot:
                Shoot();
                break;
        }
    }
    protected IEnumerator ScanForPlayer()
    {
        while (true)
        {
            ChekPlayerInRange();
            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual void ChekPlayerInRange()
    {
        if (_player == null || _attacking)
        {
            return;
        }
        if (Vector2.Distance(transform.position, _player.transform.position) < _angerRange)
        {
            _isAngry = true;
            TurnToPlayer();
            ChangeState(EnemyState.Shoot);
        }
        else
            _isAngry = false;

    }
    protected void TurnToPlayer()
    {
        if (_player.transform.position.x - transform.position.x > 0 && !_faceRight)
        {
            Flip();
        }
        else if (_player.transform.position.x - transform.position.x < 0 && _faceRight)
        {
            Flip();
        }

    } 
    protected void Shoot()
    {
        GameObject arrow = Instantiate(_projectilPrefab, _shootPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = transform.right * _arrowSpeed;
        arrow.GetComponent<SpriteRenderer>().flipX = !_faceRight;
        Destroy(arrow, 5f);
    }
}
