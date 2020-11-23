using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBanditControler : EnemyControlerBase
{
    public override void TakeDamage(int damage, DamageType type = DamageType.Casuel, Transform player = null)
    {
        if (type != DamageType.Projectile)
            return;

        base.TakeDamage(damage, type, player);
    }
   

}
