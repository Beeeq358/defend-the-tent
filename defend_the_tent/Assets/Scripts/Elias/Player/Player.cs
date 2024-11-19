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
    protected Vector3 lastInteractDir;
    protected BaseObject baseObject;
    protected float recentGrabStrength;
    protected float frameCounter;
    public Transform targetTransform;
    [SerializeField]
    protected LayerMask objectLayerMask;
    [SerializeField]
    protected Transform objectHoldPoint;
    public Transform normalTransform, bossTransform;
    public Rigidbody normalRB, bossRB;

    protected PlayerMovement playerMovement;
    protected GameManager gameManager;

    protected bool isFrame = true;
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
        isBoss = false;
        if (gameManager.gamePhase != GameManager.GamePhase.PreGame)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        CalculateRecentGrabStrength();
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

   

    protected void SetSelectedObject(BaseObject selectedObject)
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
