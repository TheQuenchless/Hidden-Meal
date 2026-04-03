using System.Threading;
using UnityEngine;

public class Restock : MonoBehaviour
{
    public GameObject drugIngredients;
    public Transform[] spawnPoints;
    public float restockInterval = 140f;

    private float timer = 0;
    void Start()
    {
        RestockItems();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= restockInterval)
        {
            timer = 0f;
            RestockItems();
        }
        
        //Debug.Log($"timer: {timer}");
    }

    void RestockItems()
    {
        DrugIngredient[] existingDrugs = Object.FindObjectsByType<DrugIngredient>(FindObjectsSortMode.None);

        foreach (DrugIngredient d in existingDrugs)
        {
            if (!d.bought && !d.gameObject.scene.IsValid()) continue; // skip prefab assets
            if (!d.bought)
            {
                Destroy(d.gameObject);
            }
        }

        // Spawn new ones at each spawn point
        foreach (Transform point in spawnPoints)
        {
            Quaternion rot = Quaternion.Euler(-34, -90, 90);
            Instantiate(drugIngredients, point.position, rot);
        }
    }
}
