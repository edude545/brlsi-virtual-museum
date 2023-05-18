using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TurntableRotate : MonoBehaviour
{

    bool active;

    Quaternion initialRotation;
    Quaternion fromRotation;
    bool returning;
    float returnProgress;

    private void Awake() {
        initialRotation = transform.localRotation;
    }

    public void Begin() {
        active = true;
        if (!returning) {
            UIController.Instance.TurntableRotate = this;
            UIController.Instance.ControlsLocked = true;
            UIController.Instance.Reticle.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void End() {
        active = false;
        fromRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        returning = true;
        returnProgress = 0f;
    }

    private void Update() {
        if (active && Input.GetMouseButton(0)) {
            transform.Rotate(Camera.main.transform.rotation * Vector3.right, Input.GetAxis("Mouse Y"), Space.World);
            transform.Rotate(Camera.main.transform.rotation * Vector3.down, Input.GetAxis("Mouse X"), Space.World);
        } else if (returning) {
            returnProgress += Time.deltaTime;
            if (returnProgress > 1) {
                returning = false;
            } else {
                float x = returnProgress - 1;
                transform.localRotation = Quaternion.Slerp(fromRotation, initialRotation, x*x*x+1);
            }
        }
    }
}
