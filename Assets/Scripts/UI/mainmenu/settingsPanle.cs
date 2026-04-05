using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class settingsPanle : MonoBehaviour
{
    [Header("Settings Menue")]
    [SerializeField] private GameObject[] panles;
    private int currentPanle = 0;
    private int maxPanle;

    [Header("Audio settings")]
    [SerializeField]private Slider mainVolumeSlider;
    [SerializeField]private Slider sfxVolumeSlider;
    [SerializeField]private Slider musicVolumeSlider;

    [Header("Color settings")]
    [SerializeField]private TMP_InputField lightcolore;
    [SerializeField]private TMP_InputField darkcolore;
    [SerializeField]private OneBit shader;
   
    void Start()
    {
        maxPanle = panles.Length;
        UpdateSettingPanle();

        mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolum", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolum", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolum", 1f);

        lightcolore.text = PlayerPrefs.GetString("color1","#FFFFFF");
        darkcolore.text = PlayerPrefs.GetString("color2","#000000");
        UpdateShader(PlayerPrefs.GetString("color1","#FFFFFF"),PlayerPrefs.GetString("color2","#000000"));

        mainVolumeSlider.onValueChanged.AddListener(OnSliderChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSliderChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    //===================================================================
    //                  Setting Panle
    //===================================================================

    public void ContinueSPbtn()
    {
       currentPanle++;

        if (currentPanle >= panles.Length)
            currentPanle = 0;

        UpdateSettingPanle();

    }

    public void BackSPbtn()
    {
       currentPanle--;

        if (currentPanle < 0)
            currentPanle = panles.Length - 1;

        UpdateSettingPanle();
    }

    private void UpdateSettingPanle()
    {
        for (int i = 0; i < panles.Length; i++)
        {
            panles[i].SetActive(false);
        }
        panles[currentPanle].SetActive(true);
    }

    //===================================================================
    //                  volum settings
    //===================================================================
    private void OnSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("mainVolum",mainVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolum",sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolum",musicVolumeSlider.value);

        PlayerPrefs.Save();
    }
    
    //===================================================================
    //                  Color settings
    //===================================================================

    public void ApplyColorbtn()
    {
        
        string lightColor = "#FFFFFF";
        string darkColor = "#000000";

        
        if (IsHexColor(lightcolore.text))
            lightColor = lightcolore.text;

        if (IsHexColor(darkcolore.text))
            darkColor = darkcolore.text;

        
        UpdateShader(lightColor, darkColor);
}

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


    public static bool IsHexColor(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        // Falls ein # am Anfang ist → entfernen
        if (input.StartsWith("#"))
            input = input.Substring(1);

        // Gültige Länge: 3, 4, 6 oder 8 Zeichen
        if (input.Length != 3 && input.Length != 4 &&
            input.Length != 6 && input.Length != 8)
            return false;

        // Prüfen ob alle Zeichen Hex-Zeichen sind
        foreach (char c in input)
        {
            bool isHex = 
                (c >= '0' && c <= '9') ||
                (c >= 'A' && c <= 'F') ||
                (c >= 'a' && c <= 'f');

            if (!isHex)
                return false;
        }

        return true;
    }

}
