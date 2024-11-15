using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Player player;
    private Vector3 moveVector;
    [SerializeField] private Rigidbody normalRB;
    [SerializeField] private bool isStunned;
    [SerializeField] private Transform normalTransform;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            Vector2 inputVector = player.GetMovementVectorNormalized();

            Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

            moveVector = player.moveVector;
            normalRB.linearVelocity += moveVector * moveSpeed;
            float rotateSpeed = 10f;
            normalTransform.forward = Vector3.Slerp(normalTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
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
