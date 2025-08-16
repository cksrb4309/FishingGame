using UnityEngine;

public class ThrushSlash : MonoBehaviour
{
    public void Setting()
    {

    }
    public void Enable()
    {

    }
    public void Disable()
    {

    }
    public void Return()
    {
        gameObject.SetActive(false);

        PoolManager.ReturnObj(ObjectPoolID.ThrushSlash, this);
    }
}
