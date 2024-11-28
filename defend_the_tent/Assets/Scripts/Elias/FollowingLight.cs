using UnityEngine;

public class FollowingLight : MonoBehaviour
{
    private GameObject parent;
    private Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
    }
    public void LogOn(GameObject parentObject)
    {
        parent = parentObject;
    }
    public void BossLogOn(GameObject bossObject)
    {
        light.intensity *= 1.2f;
        light.range += 10;
        light.enableSpotReflector = true;
        parent = bossObject;
    }


    private void Update()
    {
        if (parent != null)
        {
            transform.LookAt(parent.transform.position);
        }
    }
}
