using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager instances found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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
                UpdatePreparationPhase();
                break;
            case GamePhase.Action:
                // Perform action phase actions
                UpdateActionPhase();
                break;
            case GamePhase.PostAction:
              
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

        if (players.Count == 0)
        {
            Debug.LogWarning("No players available to choose a boss from!");
            return;
        }

        int chosenPlayer = Random.Range(0, players.Count);

        // Fetch all derived Player components and call BecomeBoss
        Player[] playerScripts = players[chosenPlayer].GetComponents<Player>();
        foreach (Player script in playerScripts)
        {
            script.BecomeBoss();
        }
    }

    public void RegisterPlayer(GameObject player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

    public void DeregisterPlayer(GameObject player)
    {
        if (players.Contains(player))
            players.Remove(player);
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

    private void UpdatePreparationPhase()
    {
        if (preparationTime > 0)
        {
            preparationTime -= Time.deltaTime;
            timeLeft.text = "Time left to prepare: " + Mathf.Ceil(preparationTime).ToString();

            if (preparationTime <= 0)
            {
                TransitionToPhase(GamePhase.Action);
            }
        }
    }

    private void UpdateActionPhase()
    {
        if (actionTime > 0)
        {
            actionTime -= Time.deltaTime;
            timeLeft.text = "Time left to defend: " + Mathf.Ceil(actionTime).ToString();

            if (actionTime <= 0)
            {
                TransitionToPhase(GamePhase.PostAction);
            }
        }

        if (!bossChosen)
        {
            ChooseBoss();
            bossChosen = true;
        }
    }

    public void TransitionToPhase(GamePhase newPhase)
    {
        gamePhase = newPhase;

        if (newPhase == GamePhase.Preparation)
        {
            objectSpawner.SetActive(true);
            objectSpawner.GetComponent<ObjectSpawner>().StartSpawning(players.Count);
        }
        else if (newPhase == GamePhase.Action)
        {

        }
        else if (newPhase == GamePhase.PostAction)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
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
    }
}
