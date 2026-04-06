using Unity.VisualScripting;
using UnityEngine;

public class buttonstart : MonoBehaviour
{
    [SerializeField] private GameObject tutorialpanle;
    [SerializeField] private GameObject settingspanle;
    [SerializeField] private GameObject losspanle;
    [SerializeField] private SceneLoader sl;
    

    private void CloseMenuePanles ()
    {
        tutorialpanle.SetActive(false);
        settingspanle.SetActive(false);
        losspanle.SetActive(false);
    }

    void Start()
    {
        CloseMenuePanles();
        if(PlayerPrefs.GetString("loss") == "true")
        {
            PlayerPrefs.DeleteKey("loss");
            PlayerPrefs.Save();


            losspanle.SetActive(true);
        }
    }

    public void btnOk()
    {
        losspanle.SetActive(false);
    }

    public void delbtn()
    {
        PlayerPrefs.DeleteAll();
    }
    

    public void Tutorialbtn()
    {
        CloseMenuePanles();
        tutorialpanle.SetActive(true);
    }

    public void Playbtn()
    {
        CloseMenuePanles();
        sl.Loadscene("SampleScene");
    }

    public void Settingbtn()
    {
        CloseMenuePanles();
        settingspanle.SetActive(true);
    }
}
