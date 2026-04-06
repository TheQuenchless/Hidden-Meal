using Unity.VisualScripting;
using UnityEngine;

public class LoadSceneWithSaves : MonoBehaviour
{
    [SerializeField]private PoliceAI policeAI;
    [SerializeField]private Wallet wallet;
    [SerializeField]private GameObject player;
    void Start()
    {
        if(PlayerPrefs.HasKey("x"))
        {
            player.transform.position = new Vector3 (PlayerPrefs.GetFloat("x"),PlayerPrefs.GetFloat("y"),PlayerPrefs.GetFloat("z"));
            wallet.money = PlayerPrefs.GetInt("wallet");
            policeAI.shiftTimer = PlayerPrefs.GetFloat("time");
        }
    }

    
}
