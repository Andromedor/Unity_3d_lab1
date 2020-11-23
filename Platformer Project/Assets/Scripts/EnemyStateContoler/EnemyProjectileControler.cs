using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileControler : MonoBehaviour
{
    [SerializeField] private int _damage;
    private float _lastEndCouner;
    private void OnTriggerEnter2D(Collider2D info)
    {
        if (Time.time - _lastEndCouner < 0.2f)
            return;
        _lastEndCouner = Time.time;
      PlayerItems player = info.GetComponent<PlayerItems>();
        if (player != null)
        {
            player.TakeTanage(_damage);
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
