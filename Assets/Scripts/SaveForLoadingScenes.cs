using Unity.VisualScripting;
using UnityEngine;

public class SaveForLoadingScenes : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private Wallet wallet;
    [SerializeField]private PoliceAI policeAI;
    public void SaveAllData()
    {
        Vector3 pos = player.transform.position;
        PlayerPrefs.SetFloat("x",pos.x);
        PlayerPrefs.SetFloat("y",pos.y);
        PlayerPrefs.SetFloat("z",pos.z);

        PlayerPrefs.SetInt("wallet",wallet.money);

        PlayerPrefs.SetFloat("time",policeAI.shiftTimer);

        PlayerPrefs.Save();
    }

    public void DelAllData()
    {
        PlayerPrefs.DeleteKey("wallet");
        PlayerPrefs.DeleteKey("time");
        PlayerPrefs.DeleteKey("y");
        PlayerPrefs.DeleteKey("x");
        PlayerPrefs.DeleteKey("z");

        PlayerPrefs.Save();
    }

    public void SaveHeldItem(float liquidAmount,int plays)
    {
        PlayerPrefs.SetInt("plays", plays);
        PlayerPrefs.SetFloat("held_liquid", liquidAmount);
        PlayerPrefs.Save();
    }
}
