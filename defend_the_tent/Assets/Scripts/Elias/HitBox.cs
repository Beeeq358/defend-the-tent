using UnityEngine;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
    private List<Collider> currentPlayerColliders = new();
    private List<Collider> currentObjectColliders = new();
    private Collider currentObjectiveCollider;
    public List<Collider> GetPlayerColliders()
    {
        return currentPlayerColliders;
    }
    public List<Collider> GetObjectColliders()
    {
        return currentObjectColliders;
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
}
