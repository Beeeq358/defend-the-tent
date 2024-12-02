using System.Collections;
using UnityEngine;

public class ExplosionFlash : MonoBehaviour
{
    [SerializeField] private float flashTime;

    private void Awake()
    {
        StartCoroutine(Flash());
    }
    private IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashTime);
        Destroy(gameObject);
    }
}
