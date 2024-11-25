using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DynamicArmIK : MonoBehaviour
{
    public PlayerInteract playerInteract;

    public TwoBoneIKConstraint leftArmIK; 
    public TwoBoneIKConstraint rightArmIK; 
    public Transform leftHandTarget;      
    public Transform rightHandTarget;     
    public Transform objectToHold;        
    public float lerpSpeed = 5f;          

    private bool isGrabbing = false;    

    private void Start()
    {
        // Add a method from this class as a listener
        playerInteract.OnPlayerGrab.AddListener(HandlePlayerGrab);
        playerInteract.OnPlayerStopGrab.AddListener(StopGrab);
    }

    // Method to handle grab events
    private void HandlePlayerGrab(Transform grabbedObject)
    {
        isGrabbing = true;
        objectToHold = grabbedObject;
        Debug.Log("Player started grabbing.");
    }

    private void StopGrab()
    {
        isGrabbing = false;
        objectToHold = null;
    }

    void Update()
    {
        if (isGrabbing && objectToHold != null)
        {
            // Dynamically move the IK targets towards the object's position
            leftHandTarget.position = Vector3.Lerp(leftHandTarget.position, GetClosestPoint(objectToHold, leftHandTarget), Time.deltaTime * lerpSpeed);
            rightHandTarget.position = Vector3.Lerp(rightHandTarget.position, GetClosestPoint(objectToHold, rightHandTarget), Time.deltaTime * lerpSpeed);

            // Adjust IK weights to smoothly blend in
            leftArmIK.weight = Mathf.Lerp(leftArmIK.weight, 1f, Time.deltaTime * lerpSpeed);
            rightArmIK.weight = Mathf.Lerp(rightArmIK.weight, 1f, Time.deltaTime * lerpSpeed);
        }
        else
        {
            // Smoothly reset IK weights to release the arms
            leftArmIK.weight = Mathf.Lerp(leftArmIK.weight, 0f, Time.deltaTime * lerpSpeed);
            rightArmIK.weight = Mathf.Lerp(rightArmIK.weight, 0f, Time.deltaTime * lerpSpeed);
        }
        if (!isGrabbing)
        {
            // Keep targets near the hands when not grabbing
            leftHandTarget.position = leftArmIK.data.tip.position;
            rightHandTarget.position = rightArmIK.data.tip.position;
        }
    }

    private Vector3 GetClosestPoint(Transform targetObject, Transform hand)
    {
        // Use the closest point on the object's collider for dynamic adjustment
        Collider targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(hand.position);
        }
        else
        {
            return targetObject.position;
        }
    }
}