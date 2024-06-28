using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField, Range(10, 200)]
    private int resolution = 10;
    private float step => 2f / resolution;

    [SerializeField]
    private Transform pointPrefab;

    [SerializeField]
    private float functionDuration = 5f;
    private float duration;

    [SerializeField]
    private float transitionDuration = 2f;
    private bool isTransitioning = false;
    private FunctionLibrary.Function prevFunction;

    [SerializeField]
    private FunctionLibrary.FunctionType functionType = FunctionLibrary.FunctionType.Wave;

    Transform[] points;

    void Awake()
    {
        points = new Transform[resolution * resolution];
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Transform point = Instantiate(pointPrefab);
                point.SetParent(transform);
                points[z * resolution + x] = point;
                point.localScale = Vector3.one * step;
            }
        }
    }

    void Update()
    {
        duration += Time.deltaTime;
        if (duration > functionDuration)
        {
            duration = 0;
            isTransitioning = true;
            prevFunction = FunctionLibrary.GetFunction(functionType);

            if (FunctionLibrary.IsLastFunction(functionType))
            {
                functionType = 0;
            }
            else functionType++;
        }

        if (duration > transitionDuration)
        {
            isTransitioning = false;
        }

        float currentTime = Time.time;
        FunctionLibrary.Function func = FunctionLibrary.GetFunction(functionType);
        // Move graph

        for (int z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++)
            {
                Transform point = points[z * resolution + x];
                float u = (x + 0.5f) * step - 1f;

                if (isTransitioning)
                {
                    point.localPosition = FunctionLibrary.FuncInterp(u, v, currentTime, prevFunction, func, duration / transitionDuration);
                }
                else point.localPosition = func(u, v, currentTime);

                point.localScale = Vector3.one * step;
            }
        }
    }
}
