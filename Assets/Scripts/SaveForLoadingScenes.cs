using Unity.VisualScripting;
using UnityEngine;

public class SaveForLoadingScenes : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private Wallet wallet;
    [SerializeField]private int drugIngredient;
    public void SaveAllData()
    {
        Vector3 pos = player.transform.position;
        PlayerPrefs.SetFloat("x",pos.x);
        PlayerPrefs.SetFloat("y",pos.y);
        PlayerPrefs.SetFloat("z",pos.z);

        PlayerPrefs.SetInt("wallet",wallet.money);

        PlayerPrefs.Save();
    }

    public void DelAllData()
    {
        PlayerPrefs.DeleteKey("wallet");
        PlayerPrefs.DeleteKey("y");
        PlayerPrefs.DeleteKey("x");
        PlayerPrefs.DeleteKey("z");

        PlayerPrefs.Save();
    }

}
