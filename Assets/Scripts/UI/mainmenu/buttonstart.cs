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
        sl.Loadscene(1);
    }

    

    public void Tutorialbtn()
    {
        CloseMenuePanles();
        tutorialpanle.SetActive(true);
    }

    public void Playbtn()
    {
        CloseMenuePanles();

    }

    public void Settingbtn()
    {
        CloseMenuePanles();
        settingspanle.SetActive(true);
    }
}
