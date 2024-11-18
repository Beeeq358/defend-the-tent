using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Player player;
    private Vector3 moveVector;
    public Transform targetTransform;
    public Rigidbody targetRB;

    [SerializeField] private bool isStunned;
    private void Start()
    {
        player = GetComponent<Player>();
        targetTransform = player.normalTransform;
        targetRB = player.normalRB;
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            Vector2 inputVector = player.GetMovementVectorNormalized();

            Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

            moveVector = player.moveVector;
            targetRB.linearVelocity += moveVector * moveSpeed;
            float rotateSpeed = 10f;
            targetTransform.forward = Vector3.Slerp(targetTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    public void IsStunned(float strength)
    {
        //play stunned animation
        //play stunned effects
        //lock player movement;
        StartCoroutine(StunTime(strength));
    }

    public IEnumerator StunTime(float stunTime)
    {
        isStunned = true;
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }
}
