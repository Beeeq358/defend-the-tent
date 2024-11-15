using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GamePhase
    {
        Preparation,
        Action,
        PostAction
    }

    [SerializeField]
    private GameObject objectSpawner;

    [SerializeField]
    private float preparationTime = 10f;
    [SerializeField]
    private float actionTime = 30f;

    public GamePhase gamePhase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamePhase = GamePhase.Preparation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gamePhase)
        {
            case GamePhase.Preparation:
                // Perform preparation action
                StartCoroutine(CountDownPreparationPhase());
                objectSpawner.SetActive(true);
                break;
            case GamePhase.Action:
                // Perform action phase actions
                objectSpawner.SetActive(false);
                StartCoroutine(CountDownActionPhase());
                break;
            case GamePhase.PostAction:
                // Perform post-action actions
                break;
        }
    }

    private IEnumerator CountDownPreparationPhase()
    {
        preparationTime -= Time.deltaTime;
        yield return new WaitForSeconds(preparationTime);
        if (preparationTime <= 0)
        {
            gamePhase = GamePhase.Action;
            StopCoroutine(CountDownPreparationPhase());
        }
    }

    private IEnumerator CountDownActionPhase()
    {
        actionTime -= Time.deltaTime;
        yield return new WaitForSeconds(actionTime);
        if (actionTime <= 0)
        {
            gamePhase = GamePhase.PostAction;
            StopCoroutine(CountDownActionPhase());
        }
    }
}
