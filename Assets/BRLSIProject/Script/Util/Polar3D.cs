using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Translated from s0
// https://github.com/edude545/system0/blob/master/src/main/java/net/ethobat/system0/api/math/Polar3D.java

public class Polar3D
{

    public float radius;
    public float inclination;
    public float azimuth;

    public Polar3D(float r, float i, float a)
    {
        radius = r;
        inclination = i;
        azimuth = a;
    }

    public void wrapAngles()
    {
        if (inclination < 0)
        {
            inclination = -inclination;
            azimuth += Mathf.PI;
        }
        inclination = inclination - (inclination % Mathf.PI) * Mathf.PI;
        azimuth = azimuth - (azimuth % Mathf.PI) * Mathf.PI;
    }

    public static Polar3D fromDegrees(float radius, float inclination, float azimuth)
    {
        return new Polar3D(radius,
                inclination * Mathf.Deg2Rad,
                azimuth * Mathf.Deg2Rad);
    }

    public static Polar3D fromCartesian(float x, float y, float z)
    {
        float radius;
        float inclination;
        float azimuth;

        // Formulae from en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
        // This page uses X and Y as the horizontal axes - Y and Z are swapped here, because Y should be vertical.
        radius = Mathf.Sqrt(x * x + y * y + z * z);
        inclination = Mathf.Acos(y / radius);
        if (x > 0)
        {
            azimuth = Mathf.Atan(z / x);
        }
        else if (x < 0 && z >= 0)
        {
            azimuth = Mathf.Atan(z / x) + Mathf.PI;
        }
        else if (x < 0 && z < 0)
        {
            azimuth = Mathf.Atan(z / x) - Mathf.PI;
        }
        else if (x == 0 && z > 0)
        {
            azimuth = Mathf.PI / 2;
        }
        else if (x == 0 && z < 0)
        {
            azimuth = -Mathf.PI / 2;
        }
        else
        {
            azimuth = 0; // Undefined; value doesn't matter.
        }

        return new Polar3D(radius, inclination, azimuth);
    }

    public static Polar3D fromCartesian(Vector3 vec)
    {
        return fromCartesian(vec.x, vec.y, vec.z);
    }

    public Vector3 toCartesian()
    {
        return new Vector3(
                radius * Mathf.Cos(azimuth) * Mathf.Sin(inclination),
                radius * Mathf.Cos(inclination),
                radius * Mathf.Sin(azimuth) * Mathf.Sin(inclination)
        );
    }

    public static Polar3D cameraLookDirection(Transform camera)
    {
        Vector3 euler = camera.eulerAngles;
        return fromDegrees(1, euler.x + 90, euler.y + 90); // not sure about this
    }

}