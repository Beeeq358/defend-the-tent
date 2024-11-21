using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject parent;
    private Slider slider;
    private int health;
    private int maxHealth;

    public void LogOn(GameObject parentObject, int maxHP)
    {
        parent = parentObject;
        maxHealth = maxHP;
    }
    public void UpdateHealth(int newHealth)
    {
        health = newHealth;
    }

    private void Update()
    {
        transform.position = parent.transform.position + new Vector3(0, 4, 0);
    }
}
