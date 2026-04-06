using Unity.VisualScripting;
using UnityEngine;

public class LoadingSettingsInScene : MonoBehaviour
{
    [SerializeField]private OneBit shader;
    [SerializeField]private AudioManager am;
    
    void Start()
    {
        UpdateShader(PlayerPrefs.GetString("color1","#FFFFFF"),PlayerPrefs.GetString("color2","#000000"));
        am.mastervolume = PlayerPrefs.GetFloat("mainVolum");
        am.SetChannelVolume(0,PlayerPrefs.GetFloat("musicVolum"));
    }


    //copied from setting panle
    private void UpdateShader(string lightHex, string darkHex)
    {
        if (UnityEngine.ColorUtility.TryParseHtmlString(lightHex, out Color lightColor))
            shader.Color1 = lightColor;
            PlayerPrefs.SetString("color1",lightHex);

        if (UnityEngine.ColorUtility.TryParseHtmlString(darkHex, out Color darkColor))
            shader.Color2 = darkColor;
            PlayerPrefs.SetString("color2",darkHex);

        PlayerPrefs.Save();
    }

    
}
