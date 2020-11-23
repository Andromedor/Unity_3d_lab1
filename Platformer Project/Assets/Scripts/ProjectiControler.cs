using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectiControler : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D info)
    {
        EnemyControlerBase enemy = info.GetComponent<EnemyControlerBase>();
        if (enemy != null)
            enemy.TakeDamage(_damage, DamageType.Projectile);
        Destroy(gameObject);
    }
}
    