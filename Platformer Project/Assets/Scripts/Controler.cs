using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Controler : MonoBehaviour
{
    public event Action<bool> OnGetHurt = delegate { };
    private PlayerItems _playerItems;
    [Header("Horizontal")]
    [SerializeField] private float _speed = 5;

    private bool _isFacingRight = true;
    private bool _cantMove = true;

    [Header("Jump")]
    [SerializeField] private float _jumpForce ;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Transform _isGrounderCheck;
    private bool _isGrounder = false;

    [Header("Ceil")]
    [SerializeField] private Collider2D _headсollider2D;
    [SerializeField] private float _checkRadiusСeiling;
    [SerializeField] private Transform _isGrounderCheckUp;
    bool _canStand = false;

    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private int _powerDamage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;
    private bool _isStrike;
    [Header("Casting")]
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireBallSpeed;
    [SerializeField] private int _castCost;
    
    private bool _isCasting;

    [Header("PowerStrike")]
    [SerializeField] private float _chargeTime;
    public float ChargeTime => _chargeTime;
    [SerializeField] private int _powerStriceCost;

    [SerializeField] private float _pushForce;
    private float _lastHurtTime;

    private Rigidbody2D _rb;
    private Animator _animator;

    [Header("Audio")] 
    [SerializeField] private InGameSound _runClip;
    [SerializeField] private InGameSound _runClip2;
    private AudioSource _audioSource;
    private InGameSound _currentSound;

    void Start()
    {
       
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerItems = GetComponent<PlayerItems>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        _isGrounder = Physics2D.OverlapCircle(_isGrounderCheck.position, _checkRadius, _ground);
        if (_animator.GetBool("Hurt") && _isGrounder && Time.time - _lastHurtTime > 0.5f)
        {
            EndHurt();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_isGrounderCheck.position, _checkRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_isGrounderCheckUp.position, _checkRadiusСeiling);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_strikePoint.position, _strikeRange);
    }
    public void Move(float move, bool jump, bool сeiling)
    {
        if (!_cantMove)
            return;

        #region Jump
       
        _rb.velocity = new Vector2(move * _speed, _rb.velocity.y);
        _animator.SetFloat("AirSpeedY", _rb.velocity.y);
        if (jump && _isGrounder)
        {
           
            _rb.AddForce(Vector2.up * _jumpForce);
        }
        #endregion
        #region Ceil
        _canStand = !Physics2D.OverlapCircle(_isGrounderCheckUp.position, _checkRadiusСeiling, _ground);
        if (сeiling)
        {
            _animator.SetTrigger("Roll");
            _headсollider2D.enabled = false;
        }
        else if (!сeiling && _canStand)
        {
            _headсollider2D.enabled = true;
        }
        _animator.SetBool("Grounded", _isGrounder);
        #endregion
        #region Move
       
        if ((move > 0 && !_isFacingRight) || (move < 0 && _isFacingRight))
        {
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0, 180, 0);
        }

       
        #endregion
        _animator.SetBool("Crawling", !_headсollider2D.enabled);
        _animator.SetBool("Jump", !_isGrounder);
        _animator.SetFloat("Speed", Mathf.Abs(move));

        if(_isGrounder && _rb.velocity.x != 0 && !_audioSource.isPlaying)
        {
            PlayAudio(_runClip);
        }
        else if(!_isGrounder || _rb.velocity.x == 0)
        {
            StopAudio(_runClip);
        }
    }
    public void PlayAudio(InGameSound sound)
    {
        if(_currentSound !=null && (_currentSound == sound || _currentSound.Priority > sound.Priority))
        {
            return;
        }
        _currentSound = sound;
        _audioSource.clip = _currentSound.AudioClip;
        _audioSource.loop = _currentSound.Loop;
        _audioSource.pitch = _currentSound.Pitch;
        _audioSource.volume = _currentSound.Volume;
        _audioSource.Play();
    }
    public void StopAudio(InGameSound sound)
    {
        if (_currentSound == null || _currentSound != sound)
            return;
        _audioSource.Stop();
        _audioSource.clip = null;
        _currentSound = null;
    }
    public void StaryCasting()
    {
        if (_isCasting || !_playerItems.ChangeMP(-_castCost))
            return;
        PlayAudio(_runClip2);
         _isCasting = true;
        _animator.SetBool("Casting", true);
    }
    private void CastFire()
    {
      GameObject fireBall =   Instantiate(_fireBall, _firePoint.position, Quaternion.identity);
        fireBall.GetComponent<Rigidbody2D>().velocity = transform.right * _fireBallSpeed;
        fireBall.GetComponent<SpriteRenderer>().flipX = !_isFacingRight;
        Destroy(fireBall, 3f);
    }
    private void EndCasting()
    {
        StopAudio(_runClip2);
        _isCasting = false;
        _animator.SetBool("Casting", false);
    }
    public void StartStriker(float holdTime)
    {
     
        if (_isStrike)
            return;
        if(holdTime >= _chargeTime)
        {
            if (!_playerItems.ChangeMP(-_powerStriceCost))
                return;
           // _animator.SetBool("PowerStrike", true);
            _animator.SetTrigger("Attack3");
         
            _cantMove = false;
        }
        else 
        {
          //  _animator.SetBool("Strike", true);
           _animator.SetTrigger("Attack1");
        }
       
        _isStrike = true; 
    }
    public void GetHurt(Vector2 position)
    {
        _lastHurtTime = Time.time;
        _cantMove = false;
        OnGetHurt(false);

        Vector2 pushDirection = new Vector2();
        pushDirection.x = position.x > transform.position.x ? -1 : 1;
        pushDirection.y = 1;
       // _animator.SetTrigger("Hurted");
        _animator.SetBool("Hurt", true);
        _rb.AddForce(pushDirection * _pushForce, ForceMode2D.Impulse);
    }
    //private void ResetPlayer()
    //{
    //    _animator.SetBool("Strike", false);
    //    _animator.SetBool("PowerStike", false);
    //    _animator.SetBool("Casting", false);
    //    _animator.SetBool("Hurt", false);
    //    _isCasting = false;
    //    _isStrike = false;
    //    _cantMove = true;
    //}
    private void EndHurt()
    {
        _cantMove = true;
        _animator.SetBool("Hurt", false);
        OnGetHurt(true);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!_strikeCollider.enabled)
    //    {
    //        return;
    //    }
    //    EnemyControlerBase enemy = collision.collider.GetComponent<EnemyControlerBase>();
    //    if (enemy == null || _damagedEnemies.Contains(enemy))
    //        return;
    //    enemy.TakeDamage(_powerStrikeDamage, DamageType.PowerStrike);
    //    _damagedEnemies.Add(enemy);
    //}
    private void Strike()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _strikeRange, _enemies);
        for(int i = 0; i < enemies.Length; i++)
        {
            EnemyControlerBase enemy = enemies[i].GetComponent<EnemyControlerBase>();
            enemy.TakeDamage(_damage);
        }
    }
    private void EndStrike()
    {
        _isStrike = false;
       // _animator.SetBool("Strike", false);
       
    }
    private void PoverStrike()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _strikeRange, _enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyControlerBase enemy = enemies[i].GetComponent<EnemyControlerBase>();
            enemy.TakeDamage(_powerDamage);
        }
    }
    private void EndPoverStrice()
    {
       // _animator.SetBool("PowerStrike", false);
        _cantMove = true;
        _isStrike = false;
    }
   
}