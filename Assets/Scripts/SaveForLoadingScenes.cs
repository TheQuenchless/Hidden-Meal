using Unity.VisualScripting;

using UnityEditorInternal;

using UnityEngine;

public class SaveForLoadingScenes : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private Wallet wallet;
    [SerializeField]private PoliceAI policeAI;
    private Drugmarker[] items ;
    public void SaveAllData()
    {
        items = FindObjectsOfType<Drugmarker>();
        Vector3 pos = player.transform.position;
        PlayerPrefs.SetFloat("x",pos.x);
        PlayerPrefs.SetFloat("y",pos.y);
        PlayerPrefs.SetFloat("z",pos.z);

        PlayerPrefs.SetInt("wallet",wallet.money);

        PlayerPrefs.SetFloat("time",policeAI.shiftTimer);
        int drugs = items.Length ;
        PlayerPrefs.SetInt("drug",drugs -- );

        for (int i = 0; i < items.Length; i++)
        {
            Vector3 position = items[i].transform.position;

            PlayerPrefs.SetFloat("drug_"+i+"_x",position.x);
            PlayerPrefs.SetFloat("drug_"+i+"_y",position.y);
            PlayerPrefs.SetFloat("drug_"+i+"_z",position.z);
        }
        Debug.Log("found drugs:"+items.Length);

        PlayerPrefs.Save();
    }

    public void DelAllData()
    {
        items = FindObjectsOfType<Drugmarker>();
        for (int i = 0; i < items.Length; i++)
        {
            PlayerPrefs.DeleteKey("drug_"+i+"_x");
            PlayerPrefs.DeleteKey("drug_"+i+"_y");
            PlayerPrefs.DeleteKey("drug_"+i+"_z");
        }
        int drugs = items.Length;
        PlayerPrefs.DeleteKey("wallet");
        PlayerPrefs.DeleteKey("time");
        PlayerPrefs.DeleteKey("y");
        PlayerPrefs.DeleteKey("x");
        PlayerPrefs.DeleteKey("z");
        PlayerPrefs.DeleteKey("drug");

        PlayerPrefs.Save();
    }
}
