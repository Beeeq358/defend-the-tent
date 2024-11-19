using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IObjectParent
{
    [Header("Config")]
    protected float spawnRange = 3;
    protected float bossSpawnRange= 40;
    protected float throwStrength = 7;
    protected float throwHeight = 2;

    protected Vector2 inputMovement;
    protected bool inputJumped;
    protected bool inputBuilded;
    protected float inputGrabStrength;
    protected BaseObject selectedBaseObject;
    protected bool isBoss;
    protected Vector3 lastInteractDir;
    protected BaseObject baseObject;
    protected float recentGrabStrength;
    protected float frameCounter;
    protected Transform targetTransform;
    [SerializeField]
    protected LayerMask objectLayerMask;

    protected Transform playerObjectHoldPoint;
    protected Transform bossObjectHoldPoint;
    protected Transform normalTransform, bossTransform;
    [SerializeField]
    protected Rigidbody normalRB, bossRB;

    [SerializeField]
    protected GameObject normalPlayer, bossPlayer, playerHoldPoint, bossHoldPoint;

    protected PlayerMovement playerMovement;
    [SerializeField]
    protected GameManager gameManager;

    protected bool isFrame = true;
    protected Vector3 moveVector = Vector3.zero;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        PlayerStart();
        isBoss = false;
        if (gameManager != null)
        {
            if (gameManager.gamePhase != GameManager.GamePhase.PreGame)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        PlayerUpdate();
    }

    protected virtual void PlayerStart()
    {
        playerObjectHoldPoint = playerHoldPoint.transform;
        bossObjectHoldPoint = bossHoldPoint.transform;
        normalTransform = normalPlayer.transform;
        bossTransform = bossPlayer.transform;
        normalRB = normalPlayer.GetComponent<Rigidbody>();
        bossRB = bossPlayer.GetComponent<Rigidbody>();
        targetTransform = normalTransform;
        targetTransform.gameObject.SetActive(true);
        targetTransform.position = GetSpawnPosition(false);
    }

    protected virtual void PlayerUpdate()
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
        return playerObjectHoldPoint;
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
