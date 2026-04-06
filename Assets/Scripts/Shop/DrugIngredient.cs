using System;
using UnityEngine;

public class DrugIngredient : MonoBehaviour
{
    public int cost = 10;
    public GameObject BOUGHT;
    public GameObject POOR;
    [SerializeField] private AudioClip buySound;
    private AudioSource audioSource;

    private Wallet wallet;
    private Collider counter;
    [NonSerialized] public bool bought = false;

    void Awake()
    {
        wallet = GameObject.Find("Player").GetComponent<Wallet>();
        counter = GameObject.Find("Counter").GetComponent<Collider>();

        audioSource = GameObject.Find("Audio").GetComponentInChildren<AudioSource>();
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

                    audioSource.PlayOneShot(buySound);
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
