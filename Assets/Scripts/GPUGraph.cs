using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    private const int maxResolution = 1000;

    [SerializeField, Range(10, maxResolution)]
    private int resolution = 10;
    private float step => 2f / resolution;

    [SerializeField]
    private float functionDuration = 5f;
    private float duration;

    [SerializeField]
    private float transitionDuration = 2f;
    private bool isTransitioning = false;
    private FunctionLibrary.FunctionType prevFunctionType;

    [SerializeField]
    private FunctionLibrary.FunctionType functionType = FunctionLibrary.FunctionType.Wave;

    [SerializeField]
    private ComputeShader computeShader;

    ComputeBuffer positionsBuffer;

    static readonly int positionsId = Shader.PropertyToID("_Positions");
    static readonly int resolutionId = Shader.PropertyToID("_Resolution");
    static readonly int stepId = Shader.PropertyToID("_Step");
    static readonly int timeId = Shader.PropertyToID("_Time");
    static readonly int progressId = Shader.PropertyToID("_TransitionProgress");

    [SerializeField]
    private Mesh mesh;

    [SerializeField]
    private Material material;

    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        if (isTransitioning)
        {
            computeShader.SetFloat(progressId, Mathf.SmoothStep(0, 1, duration / transitionDuration));
        }

        var kernelIndex = isTransitioning ? ((int)prevFunctionType * 2) + 1 : (int)functionType * 2;
        int groupCount = Mathf.CeilToInt(resolution / 8f);

        computeShader.SetBuffer(kernelIndex, positionsId, positionsBuffer);
        computeShader.Dispatch(kernelIndex, groupCount, groupCount, 1);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
    }

    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4); // x, y, z coords (each consumes 4 bytes)
    }

    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (duration > functionDuration)
        {
            duration = 0;
            isTransitioning = true;
            prevFunctionType = functionType;

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

        UpdateFunctionOnGPU();
    }
}
