using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminableUI : MonoBehaviour
{

    //public GameObject Model;
    public float Sensitivity = 1f;
    public float ZoomSpeed = 1f;
    public Quaternion DefaultRotation = Quaternion.identity;
    public bool RotateCameraInstead = false;

    // If rotating object
    protected Transform ExaminedObject;
    protected Quaternion OriginalRotation;

    // If rotating camera
    public float MinimumZoom = -8f;
    protected GameObject cam;
    public GameObject CamPrefab;
    protected RenderTexture renderTex;
    protected Vector3 cameraFocus;
    protected float zoom;

    public void Load(Examinable ex)
    {
        RotateCameraInstead = ex.RotateCameraInstead;
        cameraFocus = ex.transform.position;
        if (renderTex != null) {
            renderTex.Release();
        }
        if (RotateCameraInstead) {
            GetComponent<RawImage>().enabled = true;
            renderTex = new RenderTexture(Screen.width, Screen.height, 16);
            cam = Instantiate(CamPrefab);
            cam.GetComponent<Camera>().targetTexture = renderTex;
            GetComponent<RawImage>().texture = renderTex;
            GetComponent<RawImage>().SetNativeSize();
            zoom = 1f;
            cam.transform.position = cameraFocus + new Vector3(0, 3, zoom);
            cam.transform.LookAt(cameraFocus);
        } else {
            GetComponent<RawImage>().enabled = false;
            ExaminedObject = ex.transform;
            OriginalRotation = ex.transform.rotation;
        }
        
    }

    public void Unload()
    {
        if (cam != null)
        {
            Destroy(cam);
        }
        if (!RotateCameraInstead && ExaminedObject != null && OriginalRotation != null) {
            ExaminedObject.transform.rotation = OriginalRotation;
        }
    }

    void Update()
    {
        /* Model.transform.Rotate(Input.GetAxis("Mouse Y") * Sensitivity,
             -Input.GetAxis("Mouse X") * Sensitivity, 
             0, Space.World);
         Model.transform.localScale += Vector3.one * Input.mouseScrollDelta.y * ZoomSpeed;*/
        if (Input.GetMouseButton(1) && RotateCameraInstead) {
            zoomCamera(Input.GetAxis("Mouse Y") * ZoomSpeed);
        } else {
            if (RotateCameraInstead) {
                rotateCamera(Input.GetAxis("Mouse Y") * Sensitivity, Input.GetAxis("Mouse X") * Sensitivity);
            } else {
                ExaminedObject.Rotate(new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"),0), Space.World);
            }
        }
    }

    // code copied from s0

    // Rotate the camera by the given values in radians, centered on cameraFocus.
    // This method uses spherical coordinates to change the inclination and azimuth of the camera directly.
    // The radius is always equal to the zoom factor.
    public void rotateCamera(float pitch, float yaw)
    {
        Vector3 relPos = cam.transform.position - cameraFocus;

        Polar3D polar = Polar3D.fromCartesian(relPos);
        polar.azimuth += yaw;
        polar.inclination += pitch;

        // Inclination is the angle subtended by the coordinate and the y-axis. Camera pitch, however, is the angle from the x-z plane.

        // Don't allow the user to rotate the camera close to 90 degrees in either direction.
        // With a camera pitch of exactly +/-90 degrees, the azimuth would be undefined, and Polar3D would default to 0, snapping the camera to a fixed yaw.
        // Ensuring the camera never looks directly down or up preserves the azimuth.
        if (polar.inclination < Math.PI / 2)
        {
            polar.inclination = Mathf.Max(polar.inclination, 0.00390625f);
        }
        else
        {
            polar.inclination = Mathf.Min(polar.inclination, Mathf.PI - 0.00390625f);
        }

        cam.transform.position = cameraFocus + polar.toCartesian();
        cam.transform.LookAt(cameraFocus);
    }

    // Move the position of the camera by the given distance away from the camera focus.
    // i.e. positive values move the camera away, negative values move it closer.
    public void zoomCamera(float diff)
    {
        float newZoom = zoom + diff;
        if (newZoom < MinimumZoom)
        {
            diff = MinimumZoom - zoom;
            zoom = MinimumZoom;
        }
        else
        {
            zoom = newZoom;
        }
        cam.transform.position += (cam.transform.position - cameraFocus).normalized * diff;
    }
}
