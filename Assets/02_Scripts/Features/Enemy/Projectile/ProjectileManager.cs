using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] List<EnemyProjectile> enemyProjectiles = new();
    public static void ProjectileRegister(EnemyProjectile projectile)
    {
        Instance.enemyProjectiles.Add(projectile);
    }
    public static void ProjectileUnregister(EnemyProjectile projectile)
    {
        Instance.enemyProjectiles.Remove(projectile);
    }
    public static List<EnemyProjectile> GetProjectiles() => Instance.enemyProjectiles;
}
