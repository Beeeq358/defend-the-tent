using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float spawnRange;

    private Vector2 inputMovement;
    private bool inputJumped;
    private bool inputBuilded;
    private float inputGrabStrength;
    private bool isFrame = true;


    private Vector3 lastInteractDir;
    private BaseObject selectedBaseObject;
    private BaseObject baseObject;
    [SerializeField]
    private Transform objectHoldPoint;

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

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        inputJumped = context.action.triggered;
    }
    public void OnBuild(InputAction.CallbackContext context)
    {
        inputBuilded = context.action.triggered;
    }
    public void OnGrab(InputAction.CallbackContext context)
    {
        inputGrabStrength = context.ReadValue<float>();
    }

    //private Vector2 GetMovementVectorNormalized()
    //{
        //Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
    //}

    private void SetSelectedObject(BaseObject selectedObject)
    {
        this.selectedBaseObject = selectedObject;
    }
}
