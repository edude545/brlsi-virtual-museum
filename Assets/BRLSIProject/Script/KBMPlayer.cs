using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by Lyra

public class KBMPlayer : MonoBehaviour
{

    public Camera Camera;

    public float Speed = 0.1f;
    public float JumpPower = 60f;
    public float Sensitivity = 3f;
    public float Gravity = -1f;
    public bool Noclip = false;

    float mx = 0f;
    float my = 0f;
    float speedmul = 1f;
    Vector3 startPos;

    Rigidbody rb;
    Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        startPos = transform.position;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            Noclip = !Noclip;
            col.enabled = !Noclip;
            Debug.Log("Noclip: " + Noclip);
        }

        if (Input.GetKey("left shift"))
        {
            speedmul = 3f;
        }
        else if (Input.GetKey("left ctrl"))
        {
            speedmul = 0.2f;
        }
        else
        {
            speedmul = 1f;
        }

        Vector3 v;

        if (Noclip)
        {
            v = Camera.transform.rotation * new Vector3(
                Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0,
                Input.GetKey("e") ? 1 : Input.GetKey("q") ? -1 : 0,
                Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0
            ) * speedmul * Speed;
        }
        else
        {
            v = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0) * new Vector3(
                (Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0) * speedmul * Speed,
                (Input.GetKeyDown("space") ? JumpPower : 0) + Gravity + rb.velocity.y,
                (Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0) * speedmul * Speed
            );
        }

        mx += Input.GetAxis("Mouse X") * Sensitivity;
        my = Mathf.Clamp(my+Input.GetAxis("Mouse Y") * Sensitivity, -90, 90);

        Camera.transform.rotation = Quaternion.Euler(-my, mx, 0);

        rb.velocity = v;

        // Out of bounds check
        if (transform.position.y < -20)
        {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }

    }

}
