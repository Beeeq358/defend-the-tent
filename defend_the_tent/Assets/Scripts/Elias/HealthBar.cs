using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject hb;
    private GameObject parent;
    private Slider slider;
    private bool loggedOn = false;
    public void LogOn(GameObject parentObject, int maxHP)
    {
        parent = parentObject;
        loggedOn = true;
        slider = hb.GetComponent<Slider>();
        slider.maxValue = maxHP;
        UpdateHealth(maxHP);
    }
    public void UpdateHealth(int newHealth)
    {
        slider.value = newHealth;
    }

    private void Update()
    {
        if (parent != null)
            transform.position = parent.transform.position + new Vector3(0, 4, 0);
        else if (parent == null && loggedOn)
            Destroy(gameObject);
    }
}
