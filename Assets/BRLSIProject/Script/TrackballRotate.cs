using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// Trackball rotation code from https://www.xarg.org/2021/07/trackball-rotation-using-quaternions/
public class TrackballRotate : MonoBehaviour
{

    public float Radius = 1f;

    Quaternion initialRotation;

    bool active = false;
    Vector3 mousePosStart;
    bool rotating = false;

    Vector3 p;
    Vector3 q;

    Quaternion rotLast;
    Quaternion rotCur;

    private void Awake() {
        initialRotation = transform.localRotation;
        rotLast = initialRotation;
    }

    public void Begin() {
        active = true;
        UIController.Instance.TrackballRotate = this;
        UIController.Instance.ControlsLocked = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void End() {
        active = false;
        rotating = false;
        transform.rotation = initialRotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (active) {
            if (Input.GetMouseButtonDown(0)) {
                mousePosStart = Input.mousePosition;
                p = Project(mousePosStart);
                rotating = true;
            } else if (Input.GetMouseButtonUp(0)) {
                rotating = false;
                rotLast = rotCur * rotLast;
                rotCur = Quaternion.identity;
            }
            if (rotating) {
                if (Input.mousePosition != mousePosStart) {
                    q = Project(Input.mousePosition);
                    //Debug.Log("rotating with new q " + q);
                    rotCur = Quaternion.FromToRotation(p, q);
                    Debug.Log(rotCur);
                    transform.localRotation = rotCur;// * rotLast;
                }
            }
        }
    }

    protected Vector3 Project(Vector3 mousePos) {
        float r2 = Radius * Radius;
        float res = Mathf.Min(Screen.width, Screen.height) - 1;

        // map to -1 to 1
        mousePos.x = (2 * mousePos.x - Screen.width - 1) / res;
        mousePos.y = (2 * mousePos.y - Screen.height - 1) / res;

        float x2 = mousePos.x * mousePos.x;
        float y2 = mousePos.y * mousePos.y;

        float z = (x2 + y2 <= r2 / 2) ? 
            Mathf.Sqrt(r2 - x2 - y2):
            (r2 / 2 / Mathf.Sqrt(x2 + y2));

        Debug.Log(x2 + y2 + " <= " + r2 / 2);
        Debug.Log("Taking sqrt of " + (r2 - x2 - y2));
        Debug.Log(z + " = " + Mathf.Sqrt(r2 - x2 - y2) + " if " + (x2 + y2 <= r2 / 2) + " else " + (r2 / 2 / Mathf.Sqrt(x2 + y2)));

        return new Vector3(mousePos.x, -mousePos.y, z);
}

}
