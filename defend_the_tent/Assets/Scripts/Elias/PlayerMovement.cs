using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player playerScript;
    private Vector3 moveVector = Vector3.zero;
    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    private void Update()
    {
        Vector3 localMove = new Vector3(playerScript.inputMovement.x, 0, playerScript.inputMovement.y);

        //get the current rotation from the camera and rotate the movement inputs so forward is forward relative to the camera
        float rotationalOffset = Camera.main.transform.rotation.eulerAngles.y;
        moveVector = Quaternion.AngleAxis(rotationalOffset, Vector3.up) * localMove;
    }
}
