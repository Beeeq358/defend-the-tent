using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Animator animator;
    public Vector2 inputMovement;
    public bool inputJumped;
    public bool inputBuilded;
    public bool inputAttack1, inputAttack2, inputAttack3;
    public float inputGrabStrength;
    public Vector3 moveVector = Vector3.zero;
    private Player player;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        // Normalize moveVector magnitude and use it to set animation parameters
        float moveSpeed = moveVector.magnitude;

        // Update animation states
        if (moveSpeed > 0)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("moveSpeed", moveSpeed); // Set the speed parameter
            if (player.isBoss)
            {
                player.bossAnimator.SetBool("isWalking", true);
                player.bossAnimator.SetFloat("moveSpeed", moveSpeed);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            if (player.isBoss)
            {
                player.bossAnimator.SetBool("isWalking", false);
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        Vector3 localMove = new Vector3(inputMovement.x, 0, inputMovement.y);

        //get the current rotation from the camera and rotate the movement inputs so forward is forward relative to the camera
        float rotationalOffset = Camera.main.transform.rotation.eulerAngles.y;
        moveVector = Quaternion.AngleAxis(rotationalOffset, Vector3.up) * localMove;
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
    public void OnAttack1(InputAction.CallbackContext context)
    {
        inputAttack1 = context.action.triggered;
    }
    public void OnAttack2(InputAction.CallbackContext context)
    {
        inputAttack2 = context.action.triggered;
    }
    public void OnAttack3(InputAction.CallbackContext context)
    {
        inputAttack3 = context.action.triggered;
    }

    public void OnDeviceLost(InputDevice device)
    {
        gameManager.FirePopUp(device.displayName.ToString(), true);
        if (device is not Gamepad or Keyboard)
        {
            Debug.LogWarning("Could not recover lost device info!");
        }
    }


    public void OnDeviceRegained(InputDevice device)
    {
        gameManager.EndPopUp();
        if (device is not Gamepad or Keyboard)
        {
            Debug.LogWarning("Could not recover regained device info!");
        }
    }
    public void OnDeviceChanged(InputDevice device)
    {
        Debug.Log("Device Changed");
        if (device is not Gamepad or Keyboard)
        {
            Debug.LogWarning("Device changed, new type is NOT compatible!");
        }
    }
}
