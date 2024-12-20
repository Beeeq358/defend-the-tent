using UnityEngine;

public enum ObjectType
{
    Buildable,
    Throwable
}

[CreateAssetMenu(fileName = "ObjectSO", menuName = "Scriptable Objects/ new ObjectSO")]
public class ObjectSO : ScriptableObject
{
    public Transform objectPrefab;
    public string objectName;
    public ObjectType objectType;
}
