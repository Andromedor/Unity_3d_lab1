using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    private ServiceManager _serviceManager;
    [SerializeField] private int _maxHP;
    private int _currentHp;
    [SerializeField] private int _maxMP;
    private int _currentMP;
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _mplider;
    Controler _playerMovement ;
    Vector2 _startPosition;

    private bool _canBeDamage = true;
    void Start()
    {   
        _playerMovement = GetComponent<Controler>();
        _playerMovement.OnGetHurt += OnGetHurt;
        _currentHp = _maxHP;
        _currentMP = _maxMP;

        _hpSlider.value = _maxHP;
        _hpSlider.maxValue = _maxHP;

        _mplider.value = _maxMP;
        _mplider.maxValue = _maxMP;

        _startPosition = transform.position;
        _serviceManager = ServiceManager.Instance;
       
    }

    // Update is called once per frame
  
    public void TakeTanage(int damge, DamageType type = DamageType.Casuel, Transform enemy = null)
    {
        if (!_canBeDamage)
            return;

        _currentHp -= damge;
       
         if (_currentHp <= 0)
        {
            Ondeath();
        }
        switch (type)
        {
            case DamageType.PowerStrike:
                _playerMovement.GetHurt(enemy.position);
                break;
        }
        _hpSlider.value = _currentHp;
    }
    private void OnGetHurt(bool canBeDamage)
    {
        _canBeDamage = canBeDamage;
    }
    public void RestoreHP(int hp)
    {
        _currentHp += hp;
        if (_currentHp > _maxHP)
        {
            _currentHp = _maxHP;
        }
        _hpSlider.value = _currentHp;
       
    }
    public bool ChangeMP(int value)
    {
     
        if (value < 0 && _currentMP < Mathf.Abs(value))
            return false;
       
       _currentMP += value;
        if (_currentMP > _maxMP)
            _currentMP = _maxMP;

        _mplider.value = _currentMP;
        return true;
    }
    public void Ondeath()
    {
        // Destroy(gameObject);
        _serviceManager.Restart();
    }
}
