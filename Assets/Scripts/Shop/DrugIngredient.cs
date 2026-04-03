using System;
using UnityEngine;

public class DrugIngredient : MonoBehaviour
{
    public int cost = 10;
    public GameObject BOUGHT;
    public GameObject POOR;

    private Wallet wallet;
    private Collider counter;
    [NonSerialized] public bool bought = false;

    void Awake()
    {
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<Wallet>();
        counter = GameObject.Find("Counter").GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == counter)
        {
            Debug.Log("Collided with counter");

            if (!bought)
            {
                if (wallet.money >= cost)
                {
                    wallet.money -= cost;
                    bought = true;

                    GameObject popup = Instantiate(BOUGHT, transform.position, Quaternion.identity);
                    Destroy(popup, 1f);
                    Debug.Log("BOUGHT");
                }
                else
                {
                    GameObject popup = Instantiate(POOR, transform.position, Quaternion.identity);
                    Destroy(popup, 1f);
                    Debug.Log("POOR");
                }
            }
        }
    }
}
