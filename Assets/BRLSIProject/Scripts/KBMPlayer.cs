using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Written by Lyra

public class KBMPlayer : MonoBehaviour
{

    public Camera Camera;
    public AudioListener AudioListener;
    [HideInInspector] public GameObject LookTarget;

    public SoundArea ActiveVoiceSource;

    public SoundArea AudioDemoPlayer;

    public float Speed = 0.1f;
    public float JumpPower = 60f;
    public float Sensitivity = 3f;
    public float Gravity = -1f;
    public bool Noclip = false;
    public float ScrollSensitivity = 1f;

    float pmx = 0f;
    float pmy = 0f;
    float mx = 0f;
    float my = 0f;

    Vector3 startPos;
    Rigidbody rb;
    Collider col;
    LookText lastLookText;
    bool raycastedThisFrame = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        startPos = transform.position;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        ActiveVoiceSource = AudioDemoPlayer;
        AudioSource src = ActiveVoiceSource.GetComponent<AudioSource>();
        src.time = 0f;
        src.Play();
        UIController.Instance.AudioStopHint.SetActive(true);
    }

    private void Update()
    {
        raycastedThisFrame = false;
        Vector3 dv = new Vector3(0, Noclip ? 0 : Gravity+rb.velocity.y, 0);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (ActiveVoiceSource != null && Input.GetKeyDown("m")) {
            ActiveVoiceSource.GetComponent<AudioSource>().Stop();
            UIController.Instance.AudioStopHint.SetActive(false);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            Camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
        }
        if (Input.GetMouseButtonDown(2)) {
            Camera.fieldOfView = 60f;
        }

        if (!UIController.Instance.ControlsLocked)
        {
            if (Input.GetKeyDown("v")) {
                Noclip = !Noclip;
                col.enabled = !Noclip;
                Debug.Log("Noclip = " + Noclip);
            }

            float speedmul;
            if (Input.GetKey("left shift")) { speedmul = 3f; }
            else if (Input.GetKey("left ctrl")) { speedmul = 0.2f; }
            else { speedmul = 1f; }

            if (Noclip) {
                dv = Camera.transform.rotation * new Vector3(
                    Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0,
                    Input.GetKey("e") ? 1 : Input.GetKey("q") ? -1 : 0,
                    Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0
                ) * speedmul * Speed;
            }
            else {
                dv += Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0) * new Vector3(
                    (Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0) * speedmul * Speed,
                    Input.GetKeyDown("space") ? JumpPower : 0,
                    (Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0) * speedmul * Speed
                );
            }

            // Only rotate camera and do raycast if the mouse has been moved

            mx += Input.GetAxis("Mouse X") * Sensitivity;
            my = Mathf.Clamp(my + Input.GetAxis("Mouse Y") * Sensitivity, -90, 90);

            if (mx != pmx || my != pmy) { // When mouse is moved:
                pmx = mx; pmy = my;
                Camera.transform.rotation = Quaternion.Euler(-my, mx, 0);
                if (!raycastedThisFrame) {
                    DoRaycast();
                }
            }

            if (LookTarget != null)
            {
                // Examinable behavior
                if (Input.GetMouseButtonDown(0))
                { // left
                    Examinable ex = LookTarget.GetComponent<Examinable>();
                    if (ex != null)
                    {
                        UIController.Instance.Examine(ex);
                    }
                    TurntableRotate tbr = LookTarget.GetComponent<TurntableRotate>();
                    if (tbr != null) {
                        tbr.Begin();
                        UIController.Instance.ExitHint.SetActive(true);
                    }
                    AxisRotate ar = LookTarget.GetComponent<AxisRotate>();
                    if (ar != null) {
                        ar.Begin();
                        UIController.Instance.ExitHint.SetActive(true);
                    }
                    SoundArea sa = LookTarget.GetComponent<SoundArea>();
                    if (sa != null) {
                        if (ActiveVoiceSource != null) {
                            ActiveVoiceSource.GetComponent<AudioSource>().Stop();
                        }
                        ActiveVoiceSource = sa;
                        AudioSource src = ActiveVoiceSource.GetComponent<AudioSource>();
                        src.time = 0f;
                        src.Play();
                        UIController.Instance.AudioStopHint.SetActive(true);
                    }
                    Transporter ts = LookTarget.GetComponent<Transporter>();
                    if (ts != null) {
                        transform.parent = ts.Destination;
                        transform.localPosition = Vector3.zero;
                        if (ts.EnableObject != null) {
                            ts.EnableObject.SetActive(true);
                        }
                        if (ts.DisableObject != null) {
                            ts.DisableObject.SetActive(false);
                        }
                    }
                }
            }
        }

        rb.velocity = dv;

        // Out of bounds check
        if (transform.position.y < -20) {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }

    }

    protected void DoRaycast()
    {
        raycastedThisFrame = true;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        LookTarget = null;
        if (Physics.Raycast(ray, out rayHit))
        {
            LookTarget = rayHit.transform.gameObject;

            // LookText behavior
            LookText lt = LookTarget.GetComponent<LookText>();
            if (lt == null)
            { // Hide text if nothing was found
                UIController.Instance.HideText();
                lastLookText = null;
            }
            else
            {
                if (lt != lastLookText && (LookTarget.transform.position - transform.position).magnitude < lt.MaxLookDistance)
                {
                    lastLookText = lt;
                    UIController.Instance.DisplayText(lt);
                }
            }
        }
    }

}
