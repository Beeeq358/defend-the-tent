using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}
