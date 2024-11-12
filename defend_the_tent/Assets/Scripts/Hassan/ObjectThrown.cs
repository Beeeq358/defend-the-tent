using UnityEngine;

public class ObjectThrown : MonoBehaviour
{
    private float forceMagnitude;
    [SerializeField]
    private float upwardForceMagnitude = 5f;
    [SerializeField]
    private Rigidbody objectRb;

    void Start()
    {
        // Choose a random target distance from the origin between 5 and 10
        float targetDistance = Random.Range(5f, 10f);

        // Choose a random angle for a landing point within the target distance circle
        float angle = Random.Range(0, Mathf.PI * 2);  // Random angle in radians
        Vector3 targetPoint = new Vector3(
            Mathf.Cos(angle) * targetDistance,
            0,  // Keep the target point on the same Y level
            Mathf.Sin(angle) * targetDistance
        );

        // Calculate the direction from the object's position to the target point
        Vector3 directionToTarget = (targetPoint - transform.position).normalized;

        // Set a random force magnitude to vary launch power
        forceMagnitude = Random.Range(10f, 30f);

        // Calculate the launch force, including upward force for an arc
        Vector3 force = (directionToTarget * forceMagnitude) + (Vector3.up * upwardForceMagnitude);

        // Apply the force at the object's current position
        objectRb.AddForceAtPosition(force, transform.position, ForceMode.Impulse);
    }
}