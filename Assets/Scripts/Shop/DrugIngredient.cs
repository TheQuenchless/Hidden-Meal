using System;
using UnityEngine;

public class DrugIngredient : MonoBehaviour
{
    public Collider counter;
    public int cost = 10;
    public GameObject BOUGHT;
    public GameObject POOR;

    private Wallet wallet;
    [NonSerialized] public bool bought = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        wallet = player.GetComponent<Wallet>();
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

                    Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                    GameObject popup = Instantiate(BOUGHT, transform.position, rot);
                    Destroy(popup, 1f);
                    Debug.Log("BOUGHT");
                }
                else
                {
                    
                    Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                    GameObject popup = Instantiate(POOR, transform.position, rot);
                    Destroy(popup, 1f);
                    Debug.Log("POOR");
                }
            }
        }
    }
}
