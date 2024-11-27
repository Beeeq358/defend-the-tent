using System;
using UnityEngine;

public interface IChildObject
{
    event Action<IChildObject> OnDestroyed;
    public void SetObjectParent(IObjectParent parent);
    public void ClearObjectParent(Player parent);
    public GameObject GetObjectParent();
    public GameObject GetGameObject();
}
