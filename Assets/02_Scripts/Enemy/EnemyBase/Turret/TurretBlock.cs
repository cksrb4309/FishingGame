using System.Collections;
using UnityEngine;

public class TurretBlock : Block
{
    [SerializeField] TurretAttack turretAttack;
    public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage);

        if (isAlive) // 데미지를 받고 살아있을 때
        {
            turretAttack.SetTarget();
        }
    }
    public void OnPlayerDetected()
    {
        turretAttack.SetTarget();
    }
}
