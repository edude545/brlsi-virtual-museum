using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Written by Lyra

public class Examinable : MonoBehaviour
{

    public float ScaleFactorOverride = 512f;

    // Load this Examinable's mesh and material to the passed GameObject.
    /*public void LoadModel(GameObject obj)
    {
        Mesh mesh = getMesh();
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.GetComponent<MeshRenderer>().sharedMaterial = getMaterial();

        float maxMagnitude = float.MinValue;
        Vector3 centre = Vector3.zero; int n = 0;
        foreach (Vector3 v in mesh.vertices)
        {
            centre += v; n++;
        }
        centre /= n;
        foreach (Vector3 v in mesh.vertices)
        {
            maxMagnitude = Mathf.Max(maxMagnitude, v.magnitude);
        }
        float d = 1 / Mathf.Sqrt(maxMagnitude * maxMagnitude) / 5 * ScaleFactorOverride;
        Debug.Log(d);
        obj.transform.localScale = new Vector3(d, d, d);

    }*/

    // todo: we probably want to separate the meshes displayed in the museum from the examined meshes
    // could help with web optimisation, maybe only load the high-fidelity meshes from the server on request?
    private Mesh getMesh() {
        return GetComponent<MeshFilter>().sharedMesh;
    }
    private Material getMaterial() {
        return GetComponent<MeshRenderer>().sharedMaterial;
    }
}
