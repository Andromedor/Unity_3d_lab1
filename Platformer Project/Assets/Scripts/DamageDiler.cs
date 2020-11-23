using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageDiler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int _damage;
    [SerializeField] private float _timeDeley;
    private PlayerItems _player;
    private DateTime _lastEndCouner;
   
    
    private void OnTriggerEnter2D(Collider2D info)
    {
        if ((DateTime.Now - _lastEndCouner).TotalSeconds < 0.1f)
            return;
        _lastEndCouner = DateTime.Now;
        _player = info.GetComponent<PlayerItems>();
        if(_player != null)
        { 
            _player.TakeTanage(_damage);
        }
    }
    private void OnTriggerExit2D(Collider2D info)
    {
        if(_player = info.GetComponent<PlayerItems>())
        {
            _player = null;
        }
    }
    private void Update()
    {
        if(_player != null && (DateTime.Now - _lastEndCouner).TotalSeconds > _timeDeley)
        {
            _player.TakeTanage(_damage);
            _lastEndCouner = DateTime.Now; 
        }
    }
}
