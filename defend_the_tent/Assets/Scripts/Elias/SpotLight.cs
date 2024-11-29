using System.Collections;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public enum LightType
    {
        GroundSpotLight,
        CrowdSpotLight
    }

    private float waitTime;
    private GameManager gameManager;
    [SerializeField] private Animator animator;
    private bool isPrep, isRotate;
    public LightType lightType;
    private float randomRotationSpeed;

    private void Awake()
    {
        randomRotationSpeed = Random.Range(-0.6f, 0.6f);
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
            isRotate = true;
        }
        if (gameManager.gamePhase == GameManager.GamePhase.Preparation)
            isPrep = true;


        if (isRotate && lightType == LightType.CrowdSpotLight)
        {
            transform.Rotate(0, randomRotationSpeed, 0);
        }
    }


    private IEnumerator WaitUntilStart()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("Start");
    }
}
