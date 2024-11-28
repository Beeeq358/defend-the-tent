using UnityEngine;

public class FollowingLight : MonoBehaviour
{
    private GameObject parent;
    public void LogOn(GameObject parentObject)
    {
        parent = parentObject;
    }


    private void Update()
    {
        if (parent != null)
        {
            transform.LookAt(parent.transform.position);
        }
    }
}
