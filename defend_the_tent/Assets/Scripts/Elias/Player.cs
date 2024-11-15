using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IObjectParent
{
    [Header("Config")]
    public float spawnRange;
    public float throwStrength;
    public float throwHeight;

    public Vector2 inputMovement;
    public bool inputJumped;
    public bool inputBuilded;
    public float inputGrabStrength;
    private Vector3 lastInteractDir;
    public BaseObject selectedBaseObject;
    private BaseObject baseObject;
    private float recentGrabStrength;
    private float frameCounter;
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
        CalculateRecentGrabStrength();
        if (inputBuilded )
        {
            if (inputBuilded)
            {
                if (selectedBaseObject != null && selectedBaseObject.objectSO.objectType == ObjectType.Buildable)
                {
                    if (selectedBaseObject is BuildableObject buildableObject)
                    {
                        InteractBuild(this, buildableObject);
                    }
                    else
                    {
                        Debug.LogWarning("Selected object is not a BuildableObject!");
                    }
                }
            }
        }
        if (inputGrabStrength > 0)
        {
            Debug.Log("Interacted!");
            if (baseObject == null)
            {
                if (selectedBaseObject != null)
                {
                    InteractGrab(this, selectedBaseObject);
                }
                else
                {
                    Debug.LogWarning("No Object selected!");
                }
            }
        }
        if (inputGrabStrength == 0 && baseObject != null)
        {
            Rigidbody tempRB = baseObject.rb;
            Debug.Log("Dropping Object");
            baseObject.ClearObjectParent(this);
            tempRB.isKinematic = false;
            tempRB.AddForce((transform.forward + new Vector3(0, throwHeight, 0)) * recentGrabStrength * throwStrength, ForceMode.Impulse);
        }
    }

    private void CalculateRecentGrabStrength()
    {
        if (inputGrabStrength > 0 && inputGrabStrength >= recentGrabStrength)
        {
            recentGrabStrength = inputGrabStrength;
            frameCounter = 0;
        }
        frameCounter++;
        if (frameCounter > 30)
        {
            recentGrabStrength = 0;
        }
        Debug.Log(recentGrabStrength);
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

    public void InteractGrab(Player player, BaseObject baseObject)
    {
        baseObject.SetObjectParent(player);
    }

    public void InteractBuild(Player player, BuildableObject buildableObject)
    {
        Debug.Log("Building");
        buildableObject.SetKinematic(true);
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
        if (this.selectedBaseObject != null)
        {
            this.selectedBaseObject.SetSelectedVisual(false);
        }

        this.selectedBaseObject = selectedObject;

        if (this.selectedBaseObject != null)
        {
            this.selectedBaseObject.SetSelectedVisual(true);
        }
    }


    public Transform GetObjectFollowTransform()
    {
        return objectHoldPoint;
    }

    public void SetObject(BaseObject baseObject)
    {
        this.baseObject = baseObject;
    }
    public BaseObject GetObject()
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
