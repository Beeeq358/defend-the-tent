using System.Collections;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private float waitTime;
    private GameManager gameManager;
    [SerializeField] private Animator animator;
    private bool isPrep;

    private void Awake()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
        waitTime = Random.Range(0f, 2f);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (isPrep)
        {
            StartCoroutine(WaitUntilStart());
            isPrep = false;
        }
        if (gameManager.gamePhase == GameManager.GamePhase.Preparation)
            isPrep = true;
    }


    private IEnumerator WaitUntilStart()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("Start");
    }
}
