using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public PoliceAI police;
    [SerializeField]private TMP_Text text;
    private float startshift;
    private float shifttimer;
    private float  timeleft;
    private int remainingSeconds;


    void Start()
    {
        startshift = police.shiftStartTime;
    }

    // Update is called once per frame
    void Update()
    {
       getValue();
       calculateCountdown();       
    }

    void getValue()
    {
        shifttimer = police.shiftTimer;
    }

    void calculateCountdown()
    {
        timeleft = Mathf.Max(0,startshift - shifttimer);
        remainingSeconds = Mathf.CeilToInt(timeleft);
        text.text = remainingSeconds.ToString();
    }
}
