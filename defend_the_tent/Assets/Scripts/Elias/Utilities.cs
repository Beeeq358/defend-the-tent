using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    GameManager gameManager;
    public void OnStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void EveryonesIn()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.gamePhase = GameManager.GamePhase.Preparation;
    }
}
