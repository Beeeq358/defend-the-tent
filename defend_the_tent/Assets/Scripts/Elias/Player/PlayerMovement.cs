using System.Collections;
using UnityEngine;

public class PlayerMovement : Player
{
    public float moveSpeed;

    private Player player;
    public Rigidbody targetRB;
    public GameObject stunParticle;

    public bool isStunned;
    private void Start()
    {
        base.PlayerStart();
        StartCoroutine(FrameCheck());
        player = GetComponent<Player>();
        targetRB = normalRB;
        StopStunParticles();
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            Vector2 inputVector = player.GetMovementVectorNormalized();

            Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

            targetRB.linearVelocity += input.moveVector * moveSpeed;
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
        stunParticle.transform.position = targetTransform.position;
        if (isBoss)
        {
            stunParticle.transform.position += new Vector3(0, 5, 0);
        }
        stunParticle.SetActive(true);
        //lock player movement;
        StartCoroutine(StunTime(strength));
    }

    public override void BecomeBoss()
    {
        targetTransform = bossTransform;
        targetTransform.gameObject.SetActive(true);
        normalTransform.gameObject.SetActive(false);
        targetRB = bossRB;
        targetTransform.position = GetSpawnPosition(true);
        moveSpeed = 0.3f;
        base.BecomeBoss();
    }
    protected override void BaseClassExclusive()
    {

    }

    public IEnumerator StunTime(float stunTime)
    {
        float originalMass = 4;
        isStunned = true;
        if (isBoss)
        {
            stunTime *= 3;
            bossRB.mass = 0.1f;
        }
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        if (isBoss)
            bossRB.mass = originalMass;

        StopStunParticles();
    }

    private void StopStunParticles()
    {
        stunParticle.SetActive(false);
    }
}
