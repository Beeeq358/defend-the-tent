using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject popupObj;

    [SerializeField]
    private TextMeshProUGUI timeLeft;

    [SerializeField]
    private float preparationTime = 10f;
    [SerializeField]
    private float actionTime = 30f;

    public GamePhase gamePhase;

    private bool objectiveDestroyed;
    private bool bossWon;
    private bool playersWon;
    private bool startedSpawning;
    private bool gameEnded;

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
                timeLeft.text = "Time left to prepare: " + preparationTime.ToString("F0");

                if (!startedSpawning)
                {
                    objectSpawner.SetActive(true);
                    objectSpawner.GetComponent<ObjectSpawner>().StartSpawning(players.Count);
                    startedSpawning = true;
                }
                StartCoroutine(CountDownPreparationPhase());
                break;
            case GamePhase.Action:
                // Perform action phase actions
                timeLeft.text = "Time left to defend: " + actionTime.ToString("F0");
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

                if (playersWon && !gameEnded)
                {
                    // Perform player win logic
                    PlayerPrefs.SetInt("Winner", 0);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("End Screen");
                }
                else if (bossWon && !gameEnded)
                {
                    // Perform boss win logic
                    PlayerPrefs.SetInt("Winner", 1);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("End Screen");
                }
                break;
        }
    }

    public void GoToPrepPhase()
    {
        gamePhase = GamePhase.PreGame;
    }

    public void FirePopUp(string device, bool isLost)
    {
        if (isLost)
            message.text = device + " lost connection, please reconnect it";
        popupObj.SetActive(true);
        Time.timeScale = 0;
    }

    public void EndPopUp()
    {
        Time.timeScale = 1;
        popupObj.SetActive(false);
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
