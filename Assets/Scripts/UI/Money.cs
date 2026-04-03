using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public Wallet wallet;
    [SerializeField] private TMP_Text text;
    private int money;

    // Update is called once per frame
    void Update()
    {
       money = wallet.money;
       text.text = money.ToString();
    }
}
