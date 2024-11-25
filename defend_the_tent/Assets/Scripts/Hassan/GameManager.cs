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
    private ObjectiveScript objective;

    [SerializeField]
    private GameObject objectSpawner;

    [SerializeField]
    private float preparationTime = 10f;
    [SerializeField]
    private float actionTime = 30f;

    public GamePhase gamePhase;

    private bool objectiveDestroyed;
    private bool bossWon;
    private bool playersWon;

    public List<GameObject> players = new();
    private bool bossChosen;

    void Start()
    {
        objective.OnObjectiveDestroyed.AddListener(SetObjectiveDestroyed);
        gamePhase = GamePhase.PreGame;
    }

    private void SetObjectiveDestroyed()
    {
        objectiveDestroyed = true;
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
                objectSpawner.SetActive(true);
                StartCoroutine(CountDownPreparationPhase());
                break;
            case GamePhase.Action:
                // Perform action phase actions
                StartCoroutine(CountDownActionPhase());
                if (!bossChosen)
                    ChooseBoss();
                if (objectiveDestroyed)
                {
                    StopCoroutine(CountDownActionPhase());
                    gamePhase = GamePhase.PostAction;
                }
                break;
            case GamePhase.PostAction:
                // Perform post-action actions
                if (objectiveDestroyed)
                {
                    bossWon = true;
                }
                else
                {
                    playersWon = true;
                }

                if (playersWon)
                {
                    // Perform player win logic
                    PlayerPrefs.SetInt("Winner", 0);
                    PlayerPrefs.Save();
                }
                else if (bossWon)
                {
                    // Perform boss win logic
                    PlayerPrefs.SetInt("Winner", 1);
                    PlayerPrefs.Save();
                }
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

    private void ChooseBoss()
    {
        bossChosen = true;
        players = GetPlayers();
        int chosenPlayer = Random.Range(0, players.Count);
        players[chosenPlayer].GetComponent<Player>().BecomeBoss();
        players[chosenPlayer].GetComponent<PlayerInteract>().isBoss = true;
        players[chosenPlayer].GetComponent<PlayerMovement>().BecomeBoss();
    }

    public List<GameObject> GetPlayers()
    {
        List<GameObject> activePlayers = new();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<Player>() != null)
                activePlayers.Add(player);
        }
        return activePlayers;
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
