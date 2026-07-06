using UnityEngine;

public static class ColorPicker
{
    public static bool TryPickColor(RaycastHit hit, out Color color)
    {
        color = Color.white;

        Renderer renderer = hit.collider.GetComponent<Renderer>();

        if (renderer == null) return false;

        Material material = renderer.material;

        // Get the material tint color
        Color tint = Color.white;

        if (material.HasProperty("_BaseColor"))
            tint = material.GetColor("_BaseColor");
        else if (material.HasProperty("_Color"))
            tint = material.GetColor("_Color");

        // Try to sample the albedo texture
        if (material.HasProperty("_BaseMap"))
        {
            Texture2D texture = material.GetTexture("_BaseMap") as Texture2D;

            if(texture != null && texture.isReadable)
            {
                Color textureColor = texture.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y);

                color = textureColor * tint;
                return true;
            }
        }

        // No texture, just use material color
        color = tint;
        return true;
    }
}
