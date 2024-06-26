using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField, Range(10, 100)]
    private int resolution = 10;
    private float step => 2f / resolution;

    [SerializeField]
    private Transform pointPrefab;

    Transform[] points;

    void Awake()
    {
        points = new Transform[resolution];
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.SetParent(transform);
            points[i] = point;
            point.localPosition = Vector3.zero + Vector3.right * ((i + 0.5f) * step - 1f);
            point.localScale = Vector3.one * step;
        }
    }

    void Update()
    {
        float currentTime = Time.time;
        // Move graph
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 pos = point.localPosition;
            pos.y = Mathf.Sin(Mathf.PI * (pos.x + currentTime));
            point.localPosition = pos;
        }
    }
}
