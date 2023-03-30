using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExaminableUI : MonoBehaviour
{

    public GameObject Model;
    public float Sensitivity = 1f;
    public float ZoomSpeed = 1f;
    public Quaternion DefaultRotation = Quaternion.identity;

    float rx = 0f;
    float ry = 0f;

    public void Load(Examinable ex)
    {
        ex.LoadModel(Model);
        Model.transform.rotation = DefaultRotation;
        rx = 0f; ry = 0f;
    }

    void Update()
    {
        rx = Mathf.Clamp(rx + (Input.GetKey("a") ? Sensitivity : Input.GetKey("d") ? -Sensitivity : 0), -90, 90);
        ry += Input.GetKey("w") ? Sensitivity : Input.GetKey("s") ? -Sensitivity : 0;
        Model.transform.localRotation = Quaternion.Euler(ry,rx,0);
        Model.transform.localScale += Vector3.one * Input.mouseScrollDelta.y * ZoomSpeed;
    }
}
