using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    GameManager gameManager;
    GameObject everyonesInButton;
    public void OnStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void EveryonesIn()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.GetPlayers().Count > 1)
        {
            gameManager.gamePhase = GameManager.GamePhase.Preparation;
            everyonesInButton = GameObject.Find("Everyone's In");
            everyonesInButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Not enough players, 2 players are needed to play. 3 or 4 is recommended");
        }
    }
    public void EndPopup()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.EndPopUp();
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("Starting Screen");
    }
}
