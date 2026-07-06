using UnityEngine;

[System.Serializable]
public class PaintBrushConfig
{
    public Color brushColor = Color.red;
    public float radius = 32f;

    [Range(0,1)] public float metallic = 0f;
    [Range(0, 1)] public float smoothness = 0f;
}
