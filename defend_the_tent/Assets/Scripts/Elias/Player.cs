using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float spawnRange;

    private Vector2 inputMovement;
    private bool inputJumped;
    private bool inputBuilded;
    private float inputGrabStrength;

    private void Awake()
    {
        transform.position = new Vector3(Random.Range(-spawnRange, spawnRange), 1, Random.Range(-spawnRange, spawnRange));
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
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
