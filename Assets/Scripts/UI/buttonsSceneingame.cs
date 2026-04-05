using System;
using Unity.VisualScripting;
using UnityEngine;

public class buttonsSceneingame : MonoBehaviour
{
    [SerializeField]private OneBit shader;
    [SerializeField]private GameObject setting;
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
}
