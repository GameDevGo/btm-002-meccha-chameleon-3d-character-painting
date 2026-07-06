using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public Texture2D runtimeAlbedo {  get; private set; }
    public Texture2D runtimeMetallic { get; private set; }

    Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        // Albedo
        runtimeAlbedo = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

        Color[] albedoPixels = new Color[runtimeAlbedo.width * runtimeAlbedo.height];

        for(int i = 0;i< albedoPixels.Length; i++)
        {
            albedoPixels[i] = Color.white;
        }

        runtimeAlbedo.SetPixels(albedoPixels);
        runtimeAlbedo.Apply();

        // Metallic
        runtimeMetallic = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

        Color[] metallicPixels = new Color[runtimeMetallic.width * runtimeMetallic.height];

        Color defaultMetallicColor = new Color(0f, 0f, 0f, 0.5f); // R = Metallic, G = Unused, B = Unused, A = Smoothness

        for(int i = 0; i < metallicPixels.Length; i++)
        {
            metallicPixels[i] = defaultMetallicColor;
        }

        runtimeMetallic.SetPixels(metallicPixels);
        runtimeMetallic.Apply();

        Material mat = rend.material;
        mat.SetTexture("_BaseMap", runtimeAlbedo);
        mat.SetTexture("_MetallicGlossMap", runtimeMetallic);

        mat.SetFloat("_Metallic", 1f);
        mat.SetFloat("_Smoothness", 1f);

        // Enable metallic map
        mat.EnableKeyword("_METALLICSPECGLOSSMAP");

        mat.color = Color.white;
    }
}
