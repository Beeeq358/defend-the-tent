using UnityEngine;

public interface IObjectParent
{
    public Transform GetObjectFollowTransform();
    
    public void SetObject(IChildObject childObject);
    public IChildObject GetObject();
    public GameObject GetGameObject();
    public void ClearObject();
    public bool HasObject();
}
