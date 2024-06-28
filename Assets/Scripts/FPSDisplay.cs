using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class FPSDisplay : MonoBehaviour
{
    private VisualElement root;
    private Label fpsDisplayText;
    private int frames;
    private float duration;

    [SerializeField, Range(0.1f, 2f)]
    private float sampleDuration = 1f;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        fpsDisplayText = root.Q<Label>("LabelFPS") as Label;
    }

    private void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames++;
        duration += frameDuration;

        if (duration > sampleDuration)
        {
            fpsDisplayText.text = $"FPS\n{Mathf.Round(frames / duration)}";
            frames = 0;
            duration = 0;
        }
    }
}
