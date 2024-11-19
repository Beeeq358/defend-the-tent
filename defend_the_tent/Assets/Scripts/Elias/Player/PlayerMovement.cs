using System.Collections;
using UnityEngine;

public class PlayerMovement : Player
{
    public float moveSpeed;

    private Player player;
    public Rigidbody targetRB;

    [SerializeField] private bool isStunned;
    private void Start()
    {
        StartCoroutine(FrameCheck());
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
    protected void OnCollisionEnter(Collision collision)
    {
        //check if a player is already colliding with me and if this is a frame i need to check
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.name == "P_Circus tent") && isFrame)
        {
            targetTransform.position = GetSpawnPosition(false);
            StartCoroutine(FrameCheck());
        }
    }

    //wait a frame after spawn
    protected virtual IEnumerator FrameCheck()
    {
        isFrame = true;
        yield return new WaitForEndOfFrame();
        isFrame = false;
    }

    public void IsStunned(float strength)
    {
        //play stunned animation
        //play stunned effects
        //lock player movement;
        StartCoroutine(StunTime(strength));
    }

    public void BecomeBoss()
    {
        targetTransform = bossTransform;
        targetTransform.gameObject.SetActive(true);
        normalTransform.gameObject.SetActive(false);
        targetRB = bossRB;
        targetTransform.position = GetSpawnPosition(true);
        moveSpeed = 0.3f;
        isBoss = true;
    }

    public IEnumerator StunTime(float stunTime)
    {
        isStunned = true;
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }
}
