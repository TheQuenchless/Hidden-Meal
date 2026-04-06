using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] Collider counter;
    [SerializeField] GameObject COOK;

    private bool interactQueued = false;
    private bool billboard = false;

    private GameObject popup;

    void Update()
    {
        float distance = (counter.bounds.center - transform.position).sqrMagnitude;

        if (distance < 4f)
        {
            if (!billboard)
            {
                popup = Instantiate(COOK, counter.bounds.center + Vector3.up, Quaternion.identity);
                billboard = true;
            }
        }
        else
        {
            if (billboard)
            {
                Destroy(popup);
                billboard = false;
            }
        }

        if (interactQueued)
        {
            // handle interaction here
            interactQueued = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactQueued = true;
        }
    }
}