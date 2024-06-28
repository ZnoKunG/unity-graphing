using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionLibrary
{
    public enum FunctionType
    {
        Wave,
        MultiWave,
        Ripple,
        Wave3D,
        MultiWave3D,
        Ripple3D,
        Sphere,
        Turbo,
        Torus,
    }

    public delegate Vector3 Function(float u, float v, float t);

    public static Function GetFunction(FunctionType functionType)
    {
        if (functionType == FunctionType.Wave)
        {
            return Wave;
        }
        if (functionType == FunctionType.MultiWave)
        {
            return MultiWave;
        }
        if (functionType == FunctionType.Ripple)
        {
            return Ripple;
        }
        if (functionType == FunctionType.Wave3D)
        {
            return Wave3D;
        }
        if (functionType == FunctionType.MultiWave3D)
        {
            return MultiWave3D;
        }
        if (functionType == FunctionType.Ripple3D)
        {
            return Ripple3D;
        }
        if (functionType == FunctionType.Sphere)
        {
            return Sphere;
        }
        if (functionType == FunctionType.Turbo)
        {
            return Turbo;
        }
        if (functionType == FunctionType.Torus)
        {
            return Torus;
        }
        else return (x, z, t) =>
        {
            return Vector3.zero;
        };
    }

    public static bool IsLastFunction(FunctionType functionType)
    {
        return functionType == FunctionType.Torus;
    }

    public static int Count()
    {
        return (int)FunctionType.Torus;
    }

    public static Vector3 FuncInterp(float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.Lerp(from(u, v, t), to(u, v, t), Mathf.SmoothStep(0, 1, progress));
    }

    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        p.y = Mathf.Sin(Mathf.PI * (u + t));
        return p;
    }

    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        p.y = Mathf.Sin(Mathf.PI * (u + 0.5f * t));
        p.y += 0.5f * Mathf.Sin(2 * Mathf.PI * (u + t));
        return p;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        float d = Mathf.Abs(u);
        p.y = Mathf.Sin(Mathf.PI * (4f * d - t));
        p.y = p.y / (1f + 10f * d);
        return p;
    }

    public static Vector3 Wave3D(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        p.y = Mathf.Sin(Mathf.PI * (u + v + t));
        return p;
    }

    public static Vector3 MultiWave3D(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        p.y = Mathf.Sin(Mathf.PI * (u + 0.5f * t));
        p.y += 0.5f * Mathf.Sin(2 * Mathf.PI * (v + t));
        p.y += Mathf.Sin(Mathf.PI * (u + v + 0.25f * t));
        p.y = p.y * (2f / 3f);
        return p;
    }

    public static Vector3 Ripple3D(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        float d = Mathf.Sqrt(u * u + v * v);
        p.y = Mathf.Sin(Mathf.PI * (4f * d - t));
        p.y = p.y / (1f + 10f * d);
        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        float r = 1f;
        float s = r * Mathf.Cos(0.5f * Mathf.PI * v);
        p.x = s * Mathf.Sin(Mathf.PI * (u + 0.1f * t));
        p.z = s * Mathf.Cos(Mathf.PI * (u + 0.1f * t));
        p.y = r * Mathf.Sin(0.5f * Mathf.PI * v);
        return p;
    }

    public static Vector3 Turbo(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + 0.2f * Mathf.Sin(Mathf.PI * (6f * u + 4f * v + t));
        float s = r * Mathf.Cos(0.5f * Mathf.PI * v);
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.z = s * Mathf.Cos(Mathf.PI * u);
        p.y = r * Mathf.Sin(0.5f * Mathf.PI * v);
        return p;
    }

    public static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.65f + 0.1f * Mathf.Sin(Mathf.PI * (5f * u + t / 3f));
        float r2 = 0.20f + 0.05f * Mathf.Sin(Mathf.PI * (2f * u + 8f * v + 3f * t));
        float s = r1 + r2 * Mathf.Cos(Mathf.PI * v);
        Vector3 p;
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.y = r2 * Mathf.Sin(Mathf.PI * v);
        p.z = s * Mathf.Cos(Mathf.PI * u);
        return p;
    }
}
