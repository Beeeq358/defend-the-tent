using UnityEngine;

public interface IChildObject
{
    public void SetObjectParent(IObjectParent parent);
    public void ClearObjectParent(Player parent);
    public GameObject GetObjectParent();
    public GameObject GetGameObject();
}
