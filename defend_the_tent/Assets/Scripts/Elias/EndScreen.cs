using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    private void Awake()
    {
        int winner = PlayerPrefs.GetInt("Winner");
        if (winner == 0)
        {
            winnerText.text = "Defenders Win!";
        }
        else if (winner == 1)
        {
            winnerText.text = "Attacker Wins!";
        }
        else
        {
            Debug.LogError("PlayerPrefs winner returns invalid int: " + winner);
        }
    }
}
