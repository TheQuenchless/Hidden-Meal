using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Arrays")]
    public AudioSource[] audioChannels;
    private float[] channelVolume;
    public AudioClip[] audioClip;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float mastervolume = 1f;
    
    public bool volumeDirty = true;
    
    


    private void Start()
    {
        channelVolume = new float[audioChannels.Length];

        for (int i = 0; i < audioChannels.Length; i++)
        {
            channelVolume[i] = audioChannels[i].volume;
        }
        
        
    }

    void Update()
    {
        if(volumeDirty){Updatevolume();}
        
    }

    private void Updatevolume()
    {
        for (int i = 0; i < audioChannels.Length; i++)
        {            
            audioChannels[i].volume = channelVolume[i]*mastervolume;
            //Debug.Log($"Changed volume of channel {i} to {audioChannels[i].volume}");

        }
        volumeDirty = false;
    }

    public void PlaySound(int playerObjIndex,int indexOfAudio)
    {
        
        if (CheckValidObj(playerObjIndex) && CheckValidClip(indexOfAudio))
        {
            audioChannels[playerObjIndex].clip = audioClip[indexOfAudio];
            audioChannels[playerObjIndex].Play();    
        }
        
    }

    public void PauseSound(int playerObjIndex)
    {
        if (CheckValidObj(playerObjIndex)){audioChannels[playerObjIndex].Pause();}
    }


    public void SetChannelVolume(int playerObjIndex, float volume)
    {   
        
        if (CheckValidObj(playerObjIndex))
        {
            if (volume > 1f){volume=1f;}
            audioChannels[playerObjIndex].volume = volume;
            channelVolume[playerObjIndex] =volume;
            volumeDirty = true;
        }

        
    }

    public void addAudioPlayer(AudioSource audioSource)
    {
        var list  = audioChannels.ToList();
        list.Add(audioSource);
        audioChannels = list.ToArray();

        var volList = channelVolume.ToList();
        volList.Add(audioSource.volume);
        channelVolume = volList.ToArray();
        
        volumeDirty = true;
    }

    // Performs a safety check to verify that the provided index is valid
    private bool CheckValidObj(int index)
    {
        bool valid = index >= 0 && index < audioChannels.Length;
        if (!valid) Debug.LogWarning("Invalid AudioSource index!");
        return valid;
            
    }
    private bool CheckValidClip(int index)
    {
        bool valid = index >= 0 && index < audioClip.Length;
        if (!valid) Debug.LogWarning("Invalid AudioClips index!");
        return valid;
            
    }
    
}
