using Unity.VisualScripting;
using UnityEngine;

public class loadmusic : MonoBehaviour
{
    [SerializeField]private AudioManager am;
    
    void Start()
    {
        am.mastervolume = PlayerPrefs.GetFloat("mainVolum");
        am.SetChannelVolume(0,PlayerPrefs.GetFloat("musicVolum"));
        am.SetChannelVolume(1,PlayerPrefs.GetFloat("sfxvolume",1f));
    }

   
}
