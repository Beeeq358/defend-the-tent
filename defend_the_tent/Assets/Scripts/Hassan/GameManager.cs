using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GamePhase
    {
        PreGame,
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

    private List<GameObject> players = new();

    void Start()
    {
        gamePhase = GamePhase.PreGame;
    }

    void Update()
    {
        switch (gamePhase)
        {
            case GamePhase.PreGame:
                // Perform pre game actions
                break;
            case GamePhase.Preparation:
                // Perform preparation actions
                StartCoroutine(CountDownPreparationPhase());
                objectSpawner.SetActive(true);
                break;
            case GamePhase.Action:
                // Perform action phase actions
                StartCoroutine(CountDownActionPhase());
                break;
            case GamePhase.PostAction:
                // Perform post-action actions
                break;
        }
    }

    public void GoToPrepPhase()
    {
        gamePhase = GamePhase.PreGame;
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

    private IEnumerator ChooseBoss()
    {
        players.Clear();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
        int chosenPlayer = Random.Range(0, players.Count);
        players[chosenPlayer].GetComponent<Player>().BecomeBoss();
        yield return null;
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
