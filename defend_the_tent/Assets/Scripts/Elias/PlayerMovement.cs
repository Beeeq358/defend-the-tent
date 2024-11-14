using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Player player;
    private Vector3 moveVector;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        moveVector = player.moveVector;
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveVector * moveSpeed);
    }
}
