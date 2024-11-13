using UnityEngine;

public interface IObjectParent
{
    public Transform GetObjectFollowTransform();
    
    public void SetObject(BaseObject baseObject);
    public Object GetObject();
    public void ClearObject();
    public bool HasObject();
}
