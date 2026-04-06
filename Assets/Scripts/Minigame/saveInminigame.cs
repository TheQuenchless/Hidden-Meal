using Unity.VisualScripting;
using UnityEngine;

public class saveInminigame : MonoBehaviour
{
    [SerializeField]private WobblyHand wh;
    void Start()
    {
        PlayerPrefs.SetFloat("time",PlayerPrefs.GetFloat("time")+wh.gameLength);
        int amountofdrugs = PlayerPrefs.GetInt("drug",0);
        amountofdrugs ++;
        PlayerPrefs.SetInt("drugs",amountofdrugs);

    }

    
}
