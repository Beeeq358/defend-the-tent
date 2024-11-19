using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField]
    private Mode mode;

    void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                break;

            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                break;

            case Mode.CameraForward:
                Vector3 cameraForward = Camera.main.transform.forward;
                transform.forward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                break;

            case Mode.CameraForwardInverted:
                Vector3 cameraBackward = -Camera.main.transform.forward;
                transform.forward = new Vector3(cameraBackward.x, 0, cameraBackward.z).normalized;
                break;
        }
    }

}