using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<int, ParticleSystem> parryParticleDict;
    [SerializeField] ParticleSystem projectileHitParticle;
    [SerializeField] ParticleSystem playerHitParticle;

    public void Parry(int index)
    {
        parryParticleDict[index].Play();
    }
    public void ProjectileHit(Vector3 position)
    {
        ParticleSystem particle = PoolManager.GetObj(projectileHitParticle);

        if (!particle.gameObject.activeSelf) particle.gameObject.SetActive(true);

        particle.transform.position = position;

        particle.Play();
    }
    public void PlayHitParticle()
    {
        PoolManager.ParticlePlay(playerHitParticle, transform.position);
    }
}
