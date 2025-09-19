using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<int, ParticleSystem> parryParticleDict;
    public void Parry(int index)
    {
        parryParticleDict[index].Play();
    }
}
