using UnityEngine;

public class OneBit : MonoBehaviour {
    public Shader shader;

    [Range(0, 1)]
    public float threshold = 0.5f;
    public Color Color1 = Color.white;
    public Color Color2 = Color.black;
    [Range(0, 5)]
    public byte downSamples = 2;
    public bool invert = false;

    private Material bitMat;
    
    void OnEnable() {
        bitMat = new Material(shader);
        bitMat.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnDisable() {
        bitMat = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        bitMat.SetFloat("_Threshold", threshold);
        bitMat.SetInt("_Invert", invert ? 1 : 0);
        bitMat.SetColor("_Color1", Color1);
        bitMat.SetColor("_Color2", Color2);

        int width = source.width;
        int height = source.height;

        RenderTexture[] textures = new RenderTexture[8];

        RenderTexture currentSource = source;

        for (int i = 0; i < downSamples; ++i) {
            width /= 2;
            height /= 2;

            if (height < 2)
                break;

            RenderTexture currentDest = textures[i] = RenderTexture.GetTemporary(width, height, 0, source.format);

            currentDest.filterMode = FilterMode.Point;
            currentSource.filterMode = FilterMode.Point;

            Graphics.Blit(currentSource, currentDest, bitMat, 1);
            currentSource = currentDest;
        }

        RenderTexture screen = RenderTexture.GetTemporary(width, height, 0, source.format);

        screen.filterMode = FilterMode.Point;
        currentSource.filterMode = FilterMode.Point;

        Graphics.Blit(currentSource, screen, bitMat, 0);

        Graphics.Blit(screen, dest, bitMat, 1);
        RenderTexture.ReleaseTemporary(screen);

        for (int i = 0; i < downSamples; ++i) {
            RenderTexture.ReleaseTemporary(textures[i]);
        }
    }
}
