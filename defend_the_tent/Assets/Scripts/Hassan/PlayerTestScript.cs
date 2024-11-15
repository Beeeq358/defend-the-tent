using UnityEngine;

public class PlayerTestScript : MonoBehaviour, IObjectParent
{
    [SerializeField]
    private Transform objectHoldPoint;

    [SerializeField]
    private BaseObject baseObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Interact(this);
        }

        if (Input.GetMouseButtonDown(1))
        {
            InteractAlternate(this);
        }
    }

    public void Interact(PlayerTestScript player)
    {
        BaseObject.SpawnObject(baseObject.objectSO, player);
    }

    public void InteractAlternate(PlayerTestScript player)
    {
        Destroy(baseObject);
        ClearObject();
    }


    public Transform GetObjectFollowTransform()
    {
        return objectHoldPoint;
    }

    public void SetObject(BaseObject baseObject)
    {
        this.baseObject = baseObject;
    }
    public BaseObject GetObject()
    {
        return baseObject;
    }
    public void ClearObject()
    {
        Destroy(baseObject.gameObject);
        baseObject = null;
    }
    public bool HasObject()
    {
        return baseObject != null;
    }
}
