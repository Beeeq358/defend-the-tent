using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IObjectParent
{
    public float spawnRange;

    public Vector2 inputMovement;
    public bool inputJumped;
    public bool inputBuilded;
    public float inputGrabStrength;


    private Vector3 lastInteractDir;
    public BaseObject selectedBaseObject;
    private BaseObject baseObject;
    [SerializeField]
    private LayerMask objectLayerMask;
    [SerializeField]
    private Transform objectHoldPoint;

    private bool isFrame = true;
    public Vector3 moveVector = Vector3.zero;
    private void Awake()
    {
        transform.position = GetSpawnPosition();
        StartCoroutine(FrameCheck());
    }

    private void Update()
    {
        HandleInteractions();
        if (inputGrabStrength > 0)
        {
            Debug.Log("Interacted!");
            if (baseObject == null)
            {
                if (selectedBaseObject != null)
                {
                    Interact(this, selectedBaseObject);
                }
                else
                {
                    Debug.LogWarning("No Object selected!");
                }
            }
        }
        if (inputGrabStrength == 0 && baseObject != null)
        {
            Debug.Log("Dropping Object");
            baseObject.ClearObjectParent(this);
        }
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

    public Vector2 GetMovementVectorNormalized()
    {
        return inputMovement.normalized;
    }

    public void Interact(Player player, BaseObject baseObject)
    {
        baseObject.SetObjectParent(player);
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, objectLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseObject baseObject))
            {
                if (baseObject != selectedBaseObject)
                {
                    SetSelectedObject(baseObject);
                }
            }
            else
            {
                SetSelectedObject(null); 
            }
        }
        else
        {
            SetSelectedObject(null);
        }
    }

    private void SetSelectedObject(BaseObject selectedObject)
    {
        this.selectedBaseObject = selectedObject;
    }

    public Transform GetObjectFollowTransform()
    {
        return objectHoldPoint;
    }

    public void SetObject(BaseObject baseObject)
    {
        this.baseObject = baseObject;
    }
    public Object GetObject()
    {
        return baseObject;
    }
    public void ClearObject()
    {
        Debug.Log("Object cleared!");
        baseObject = null;
    }
    public bool HasObject()
    {
        return baseObject != null;
    }
}
