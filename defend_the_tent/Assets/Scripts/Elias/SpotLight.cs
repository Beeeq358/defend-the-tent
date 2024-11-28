using System.Collections;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private float waitTime;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
        waitTime = Random.Range(0f, 2f);
        StartCoroutine(WaitUntilStart());
    }


    private IEnumerator WaitUntilStart()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("Start");
    }
}
