using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public ObjectSO objectSO;

    private IObjectParent objectParent;

    private void Update()
    {

    }

    public void SetObjectParent(IObjectParent parent)
    {
        if (this.objectParent != null)
        {
            // If the object already has a parent, remove it before setting a new one
            this.objectParent.ClearObject();
        }
        // Grab the transform of the parent that the object should follow
        Transform parentTransform = parent.GetObjectFollowTransform();
        transform.parent = parentTransform;
        transform.localPosition = Vector3.zero;
        parent.SetObject(this);
    }

    public GameObject GetObjectParent()
    {
        return transform.parent.gameObject;
    }

    public static BaseObject SpawnObject(ObjectSO objectSO, IObjectParent objectParent)
    {
        Transform ObjectTransform = Instantiate(objectSO.objectPrefab);

        // Grab the script attached to the baseObject
        BaseObject baseObject = ObjectTransform.GetComponent<BaseObject>();

        baseObject.SetObjectParent(objectParent);

        return baseObject;
    }
}