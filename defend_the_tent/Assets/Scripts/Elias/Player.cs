using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float spawnRange;

    public Vector2 inputMovement;
    public bool inputJumped;
    public bool inputBuilded;
    public float inputGrabStrength;

    private bool isFrame = true;
    public Vector3 moveVector = Vector3.zero;
    private void Awake()
    {
        transform.position = GetSpawnPosition();
        StartCoroutine(FrameCheck());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check if a player is already colliding with me and if this is a frame i need to check
        if (collision.gameObject.CompareTag("Player") && isFrame)
        {
            transform.position = GetSpawnPosition();
            StartCoroutine(FrameCheck());
        }
    }
    //returns a random spawn position in the middle of the arena
    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-spawnRange, spawnRange), 1, Random.Range(-spawnRange, spawnRange));
    }

    //wait a frame after spawn
    private IEnumerator FrameCheck()
    {
        isFrame = true;
        yield return new WaitForEndOfFrame();
        isFrame = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        Vector3 localMove = new Vector3(inputMovement.x, 0, inputMovement.y);

        //get the current rotation from the camera and rotate the movement inputs so forward is forward relative to the camera
        float rotationalOffset = Camera.main.transform.rotation.eulerAngles.y;
        moveVector = Quaternion.AngleAxis(rotationalOffset, Vector3.up) * localMove;
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        inputJumped = context.action.triggered;
    }
    private void OnBuild(InputAction.CallbackContext context)
    {
        inputBuilded = context.action.triggered;
    }
    private void OnGrab(InputAction.CallbackContext context)
    {
        inputGrabStrength = context.ReadValue<float>();
    }
}
