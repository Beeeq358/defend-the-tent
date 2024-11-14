using UnityEngine;

public interface IBuildable
{
    public Transform GetConnectPosition();
    public void Connect();
    public BaseObject GetBaseObject();
    public void SetBaseObject(BaseObject obj);
    //public
}
