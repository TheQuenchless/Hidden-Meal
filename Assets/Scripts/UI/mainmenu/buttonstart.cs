using Unity.VisualScripting;
using UnityEngine;

public class buttonstart : MonoBehaviour
{
    [SerializeField] private GameObject tutorialpanle;
    [SerializeField] private GameObject settingspanle;
    [SerializeField] private SceneLoader sl;
    

    private void CloseMenuePanles ()
    {
        tutorialpanle.SetActive(false);
        settingspanle.SetActive(false);
    }

    void Start()
    {
        CloseMenuePanles();
        
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
