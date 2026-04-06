using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Interact : MonoBehaviour
{
    [SerializeField] private Collider counter;
    [SerializeField] private GameObject COOK;
    [SerializeField] private GameObject drug;
    [SerializeField] private GameObject drugIngredient;
    [SerializeField] private GrabbingThrowing grabbingThrowing;
    [SerializeField] private SceneLoader sl;
    [SerializeField] private SaveForLoadingScenes saveForLoadingScenes;

    private bool interactQueued = false;
    private bool billboard = false;

    private GameObject popup;

    void Update()
    {
        GameObject held = grabbingThrowing.heldItem;

        bool validItem = held != null && held.GetComponent<DrugIngredient>();

        if (!validItem)
        {
            if (billboard)
            {
                Destroy(popup);
                billboard = false;
            }
            return;
        }

        float distance = (counter.bounds.center - transform.position).sqrMagnitude;

        if (distance < 9f)
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

        if (interactQueued && distance < 9f)
        {
            if (held == null)
            {
                return;
            }

            saveForLoadingScenes.SaveAllData();
            sl.Loadscene("Minigame");
        }

        interactQueued = false;
    }

    

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactQueued = true;
        }
    }
}