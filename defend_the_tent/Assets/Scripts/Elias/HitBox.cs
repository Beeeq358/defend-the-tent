using UnityEngine;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
    private List<Collider> currentPlayerColliders = new();
    private List<IChildObject> currentObjectInstances = new();
    private Collider currentObjectiveCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Normal Player")
        {
            currentPlayerColliders.Add(other);
        }

        if (other.TryGetComponent<IChildObject>(out var childObject))
        {
            // Add the object to the tracked list and subscribe to its event
            if (!currentObjectInstances.Contains(childObject))
            {
                currentObjectInstances.Add(childObject);
                childObject.OnDestroyed += HandleBaseObjectDestroyed;
            }
        }

        if (other.gameObject.CompareTag("Objective"))
        {
            currentObjectiveCollider = other;
        }
        else if (!other.gameObject.CompareTag("Scenery"))
        {
            Debug.LogWarning("Unknown object entered hitbox trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IChildObject>(out var childObject))
        {
            // Remove the object from the tracked list and unsubscribe from its event
            if (currentObjectInstances.Contains(childObject))
            {
                childObject.OnDestroyed -= HandleBaseObjectDestroyed;
                currentObjectInstances.Remove(childObject);
            }
        }
        else if (other.gameObject.name == "Normal Player")
        {
            currentPlayerColliders.Remove(other);
        }
    }

    private void HandleBaseObjectDestroyed(IChildObject destroyedObject)
    {
        // Remove the destroyed object from the tracked list
        currentObjectInstances.Remove(destroyedObject);

        // Find the collider associated with the destroyed object and remove it
        var colliderToRemove = currentPlayerColliders.Find(collider => collider.gameObject == destroyedObject.GetGameObject());
        if (colliderToRemove != null)
        {
            currentPlayerColliders.Remove(colliderToRemove);
        }

        Debug.Log($"Object destroyed: {destroyedObject.GetGameObject().name}");
    }

    private void OnDisable()
    {
        // Ensure cleanup of subscriptions when the HitBox is disabled
        foreach (var childObject in currentObjectInstances)
        {
            childObject.OnDestroyed -= HandleBaseObjectDestroyed;
        }

        currentObjectInstances.Clear();
    }

    public List<Collider> GetPlayerColliders()
    {
        return currentPlayerColliders;
    }

    public List<IChildObject> GetObjectInstances()
    {
        return currentObjectInstances;
    }

    public List<Collider> GetObjectColliders()
    {
        List<Collider> objectColliders = new List<Collider>();

        foreach (IChildObject childObject in currentObjectInstances)
        {
            Collider collider = childObject.GetGameObject().GetComponent<Collider>();
            if (collider != null)
            {
                objectColliders.Add(collider);
            }
        }

        return objectColliders;
    }

    public Collider GetObjectiveCollider()
    {
        return currentObjectiveCollider;
    }
}
