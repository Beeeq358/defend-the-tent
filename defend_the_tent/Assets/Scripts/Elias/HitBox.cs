using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class HitBox : MonoBehaviour
{
    private List<Collider> currentPlayerColliders = new();
    private List<Collider> currentObjectColliders = new();
    private Collider currentObjectiveCollider;

    private void OnEnable()
    {
        BaseObject.OnDestroyed += HandleBaseObjectDestroyed;
    }

    private void OnDisable()
    {
        BaseObject.OnDestroyed -= HandleBaseObjectDestroyed;
    }

    public List<Collider> GetPlayerColliders()
    {
        return currentPlayerColliders;
    }
    public List<Collider> GetObjectColliders()
    {
        return currentObjectColliders;
    }

    public Collider GetObjectiveCollider()
    {
        return currentObjectiveCollider;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Normal Player")
            currentPlayerColliders.Add(other);
        if (other.GetComponent<BaseObject>() != null)
            currentObjectColliders.Add(other);
        if (other.gameObject.CompareTag("Objective"))
        {
            currentObjectiveCollider = other;
        }
        else if (!other.gameObject.CompareTag("Scenery"))
            Debug.LogWarning("Unknown object entered hitbox trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseObject>() != null)
            currentObjectColliders.Remove(other);
        else if (other.gameObject.name == "Normal Player")
            currentPlayerColliders.Remove(other);
    }

    private void HandleBaseObjectDestroyed(BaseObject baseObject)
    {
        // Find the collider associated with the destroyed object and remove it
        currentObjectColliders.RemoveAll(collider => collider.gameObject == baseObject.gameObject);
    }
}
