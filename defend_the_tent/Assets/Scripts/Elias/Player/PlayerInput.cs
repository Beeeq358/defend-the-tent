using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Animator animator;
    public Vector2 inputMovement;
    public bool inputJumped;
    public bool inputBuilded;
    public float inputGrabStrength;
    public Vector3 moveVector = Vector3.zero;
    private void Update()
    {
        if (moveVector.normalized != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else if (moveVector.normalized == Vector3.zero)
        {
            animator.SetBool("isWalking", false);
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
}
