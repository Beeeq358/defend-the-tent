using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour
{
    public ObjectSO objectSO;

    private IObjectParent objectParent;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (objectParent != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    public void SetObjectParent(IObjectParent parent)
    {
        // Clear existing parent if present
        this.objectParent?.ClearObject();

        // Set new parent
        Transform parentTransform = parent.GetObjectFollowTransform();
        transform.parent = parentTransform;
        StartCoroutine(LerpToPosition(Vector3.zero, 0.25f));
        parent.SetObject(this);

        // Debug the assignment
        this.objectParent = parent;
        Debug.Log($"Object parent set to: {this.objectParent}");
    }

    private IEnumerator LerpToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = transform.localPosition;
        float distanceToTarget;
        float time = 0;
        distanceToTarget = Vector3.Distance(targetPosition, initialPosition.normalized);
        while (time < duration && !objectParent.HasObject() && distanceToTarget > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
    }

    public void ClearObjectParent(Player parent)
    {
        if (this.objectParent != null)
        {
            Debug.Log("Calling ClearObject on objectParent.");
            this.objectParent.ClearObject();
            transform.parent = null;
            this.objectParent = null;
        }
        else
        {
            Debug.LogWarning("objectParent is null!");
        }
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