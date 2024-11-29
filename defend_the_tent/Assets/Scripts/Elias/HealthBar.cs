using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject hb;
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform rectTransform;
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
        transform.parent = parent.transform;
        transform.position = parent.transform.position + new Vector3(0, 5, 0);
    }
    public void ReLog(GameObject parentObject, int newMaxHP)
    {
        parent = parentObject;
        loggedOn = true;
        slider.maxValue = newMaxHP;
        UpdateHealth(newMaxHP);
        transform.parent = parent.transform;
        transform.position = parent.transform.position + new Vector3(0, 5, 0);
    }
    public void UpdateHealth(int newHealth)
    {
        slider.value = newHealth;
    }

    public void ObjectHealthbar()
    {
        transform.parent = null;
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.GetComponent<RectTransform>().position = Vector3.zero;
        canvas.GetComponent<RectTransform>().rotation = Quaternion.identity;
        rectTransform.position = new Vector3(0, 450, 0);
        rectTransform.sizeDelta = new Vector2(360, 40);
    }

    private void Update()
    {
        if (parent == null && loggedOn)
        {
            gameObject.SetActive(false);
            loggedOn = false;
        }
    }
}
