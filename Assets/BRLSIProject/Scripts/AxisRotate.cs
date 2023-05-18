using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;

public class AxisRotate : MonoBehaviour {

    bool active;

    [ReadOnly] public float AngularVelocity = 0f;
    [Range(0, 1)] public float AngularDrive = 0.995f;

    public float Sensitivity = 0.25f;

    public void Begin() {
        active = true;
        UIController.Instance.AxisRotate = this;
        UIController.Instance.ControlsLocked = true;
        UIController.Instance.Reticle.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void End() {
        active = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (active && Input.GetMouseButton(0)) {
            AngularVelocity += Input.GetAxis("Mouse X") * Sensitivity;
        }
        if (AngularVelocity != 0f) {
            transform.Rotate(Vector3.back, AngularVelocity);
            AngularVelocity *= AngularDrive;
            if (Mathf.Abs(AngularVelocity) < 0.001f) {
                AngularVelocity = 0f;
            }
        }
    }
}
