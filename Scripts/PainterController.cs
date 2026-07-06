using UnityEngine;
using UnityEngine.InputSystem;

public class PainterController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] PaintBrushConfig brush;

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if(Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.value), out RaycastHit hitInfo))
            {
                if(ColorPicker.TryPickColor(hitInfo, out Color pickedColor))
                {
                    brush.brushColor = pickedColor;
                }
            }
        }

        if (!Mouse.current.leftButton.isPressed)
            return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.value);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            PaintableObject paintableObject = hit.collider.GetComponent<PaintableObject>();

            if (paintableObject == null)
                return;

            Vector2 uv = hit.textureCoord;

            int width = paintableObject.runtimeAlbedo.width;
            int height = paintableObject.runtimeAlbedo.height;

            int centerX = Mathf.RoundToInt(uv.x * width);
            int centerY = Mathf.RoundToInt(uv.y * height);

            int radius = Mathf.RoundToInt(brush.radius);

            for(int y = -radius; y <= radius; y++)
            {
                for(int x = -radius; x <=radius; x++)
                {
                    int px = centerX + x;
                    int py = centerY + y;

                    if (px < 0 || py < 0 || px >= width || py >= height)
                        continue;

                    float distance = Mathf.Sqrt(x * x + y * y);

                    if (distance > radius)
                        continue;

                    // Paint Albedo
                    paintableObject.runtimeAlbedo.SetPixel(px, py, brush.brushColor);

                    // Paint Metallic and Smoothness
                    Color metallicAndSmoothness = new Color(brush.metallic, 0f, 0f, brush.smoothness);

                    paintableObject.runtimeMetallic.SetPixel(px,py, metallicAndSmoothness);
                }
            }

            paintableObject.runtimeAlbedo.Apply();
            paintableObject.runtimeMetallic.Apply();
        }
    }
}
