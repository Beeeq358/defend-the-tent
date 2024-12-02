using UnityEngine;

public class LandmineFlash : MonoBehaviour
{
    [SerializeField] private GameObject lightObj;
    [SerializeField] private float flashTime;

    private float time;
    private void Awake()
    {
        time = flashTime;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
            Flash(lightObj.activeInHierarchy);
    }


    private void Flash(bool active)
    {
        time = flashTime;
        if (active)
            lightObj.SetActive(false);
        else
            lightObj.SetActive(true);

    }
}
