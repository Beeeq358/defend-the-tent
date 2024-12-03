using UnityEngine;

public class FollowingLight : MonoBehaviour
{
    private GameObject parent;
    private Light followingLight;

    private void Awake()
    {
        followingLight = GetComponent<Light>();
    }
    public void LogOn(GameObject parentObject)
    {
        parent = parentObject;
    }
    public void BossLogOn(GameObject bossObject)
    {
        followingLight.intensity *= 1.2f;
        followingLight.range += 10;
        followingLight.enableSpotReflector = true;
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
