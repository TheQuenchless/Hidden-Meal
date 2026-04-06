using System;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonsSceneingame : MonoBehaviour
{
    [SerializeField]private OneBit shader;
    [SerializeField]private GameObject setting;
    [SerializeField]private SceneLoader sl;
    [SerializeField]private SaveForLoadingScenes sfl;
    void Start()
    {
        shader.downSamples = 2;
        setting.SetActive(false);
    }

   
    public void SettingsBtn()
    {
        shader.downSamples = 0;
        setting.SetActive(true);
    }

    public void CloseSettingsBtn()
    {
        shader.downSamples = 2;
        setting.SetActive(false);
    }

    public void MainMenueBtn()
    {
        sfl.DelAllData();
        sl.Loadscene("MainMenu");
    }
}
