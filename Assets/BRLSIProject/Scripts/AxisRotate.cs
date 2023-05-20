using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;

public class AxisRotate : MonoBehaviour {

    [Range(0, 1)] public float AngularDrive = 0.995f;
    public float Sensitivity = 0.25f;
    public string MouseAxis = "X";

    bool active;
    float angularVelocity = 0f;

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
            angularVelocity += Input.GetAxis("Mouse "+MouseAxis) * Sensitivity;
        }
        if (angularVelocity != 0f) {
            transform.Rotate(Vector3.back, angularVelocity);
            angularVelocity *= AngularDrive;
            if (Mathf.Abs(angularVelocity) < 0.001f) {
                angularVelocity = 0f;
            }
        }
    }
}
