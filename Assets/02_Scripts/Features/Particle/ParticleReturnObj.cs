using UnityEngine;

public class ParticleReturnObj : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);

        PoolManager.ReturnObj(this);
    }
}
