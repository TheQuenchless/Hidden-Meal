using UnityEngine;

public class buttonstart : MonoBehaviour
{
    [SerializeField] private GameObject tutorialpanle;
    [SerializeField] private GameObject settingspanle;
    [SerializeField] private GameObject quitpanle;

    private void CloseMenuePanles ()
    {
        quitpanle.SetActive(false);
        tutorialpanle.SetActive(false);
        settingspanle.SetActive(false);
    }

    void Start()
    {
        CloseMenuePanles();
    }

    public void Quitbtn()
    {
        CloseMenuePanles();
        quitpanle.SetActive(true);
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
