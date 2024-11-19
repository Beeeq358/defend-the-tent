using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IObjectParent
{
    [Header("Config")]
    public float spawnRange;
    public float bossSpawnRange;
    public float throwStrength;
    public float throwHeight;

    public Vector2 inputMovement;
    public bool inputJumped;
    public bool inputBuilded;
    public float inputGrabStrength;
    public BaseObject selectedBaseObject;
    public bool isBoss;
    private Vector3 lastInteractDir;
    private BaseObject baseObject;
    private float recentGrabStrength;
    private float frameCounter;
    public Transform targetTransform;
    [SerializeField]
    private LayerMask objectLayerMask;
    [SerializeField]
    private Transform objectHoldPoint;
    public Transform normalTransform, bossTransform;
    public Rigidbody normalRB, bossRB;

    private PlayerMovement playerMovement;
    private GameManager gameManager;

    private bool isFrame = true;
    public Vector3 moveVector = Vector3.zero;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerMovement = GetComponent<PlayerMovement>();
        targetTransform = normalTransform;
        targetTransform.gameObject.SetActive(true);
        targetTransform.position = GetSpawnPosition(false);
    }
    private void Start()
    {
        StartCoroutine(FrameCheck());
        isBoss = false;
        if (gameManager.gamePhase != GameManager.GamePhase.PreGame)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleInteractions();
        CalculateRecentGrabStrength();
        if (inputBuilded)
        {
            if (inputBuilded)
            {
                if (selectedBaseObject != null && selectedBaseObject.objectSO.objectType == ObjectType.Buildable && !isBoss)
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
            if (baseObject == null && !isBoss)
            {
                if (selectedBaseObject != null)
                {
                    if (selectedBaseObject is BuildableObject buildableObject)
                    {
                        if (!buildableObject._isInteractive)
                        {
                            Debug.LogWarning("This object is not interactive!");
                            return;
                        }
                    }
                    InteractGrab(this, selectedBaseObject);
                }
                else
                {
                    Debug.LogWarning("No Object selected!");
                }
            }

        }
        if (inputGrabStrength == 0 && baseObject != null && !isBoss)
        {
            Rigidbody tempRB = baseObject.rb;
            Debug.Log("Dropping Object");
            baseObject.ClearObjectParent(this);
            tempRB.isKinematic = false;
            tempRB.AddForce(recentGrabStrength * throwStrength * (targetTransform.forward + new Vector3(0, throwHeight, 0)), ForceMode.Impulse);
        }
    }

    protected virtual void CalculateRecentGrabStrength()
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
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //check if a player is already colliding with me and if this is a frame i need to check
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.name == "P_Circus tent") && isFrame)
        {
            targetTransform.position = GetSpawnPosition(false);
            StartCoroutine(FrameCheck());
        }
    }
    //returns a random spawn position in the middle of the arena
    protected virtual Vector3 GetSpawnPosition(bool isBoss)
    {
        if (isBoss)
        {
            Vector3 spawnPos;
            spawnPos = new Vector3(Random.Range(-bossSpawnRange, bossSpawnRange), 2, Random.Range(-bossSpawnRange, bossSpawnRange));
            while (Vector3.Distance(spawnPos, Vector3.zero) < 15)
            {
                spawnPos = new Vector3(Random.Range(-bossSpawnRange, bossSpawnRange), 2, Random.Range(-bossSpawnRange, bossSpawnRange));
            }
            return spawnPos;
        }
        else
            return new Vector3(Random.Range(-spawnRange, spawnRange), 1, Random.Range(-spawnRange, spawnRange));
    }

    //wait a frame after spawn
    protected virtual IEnumerator FrameCheck()
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
        StartCoroutine(playerMovement.StunTime(1f));
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
        if (Physics.Raycast(targetTransform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, objectLayerMask))
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
