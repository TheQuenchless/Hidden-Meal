using Unity.VisualScripting;
using UnityEngine;

public class LoadSceneWithSaves : MonoBehaviour
{
    [SerializeField]private PoliceAI policeAI;
    [SerializeField]private Wallet wallet;
    [SerializeField]private GameObject player;
    [SerializeField]private SaveForLoadingScenes sfl;
    [SerializeField]private GameObject drug;
    [SerializeField]private Transform spawner;

    private int amountofdrugs;
    void Start()
    {
        if(PlayerPrefs.HasKey("x"))
        {
            player.transform.position = new Vector3 (PlayerPrefs.GetFloat("x"),PlayerPrefs.GetFloat("y"),PlayerPrefs.GetFloat("z"));
            wallet.money = PlayerPrefs.GetInt("wallet");
            policeAI.shiftTimer = PlayerPrefs.GetFloat("time");
            amountofdrugs = PlayerPrefs.GetInt("drugs");
            Debug.Log(amountofdrugs);
            for (int i = 0; i < amountofdrugs; i++)
            {
                float x = PlayerPrefs.GetFloat("drug_"+i+"_x",spawner.position.x);
                float y = PlayerPrefs.GetFloat("drug_"+i+"_y",spawner.position.y);
                float z = PlayerPrefs.GetFloat("drug_"+i+"_y",spawner.position.y);

                Vector3 pos = new Vector3(x,y,z);
                Instantiate(drug,pos,Quaternion.identity);
            }


            sfl.DelAllData();
        }
    }

    
}
