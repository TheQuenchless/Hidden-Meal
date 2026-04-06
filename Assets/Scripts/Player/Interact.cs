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

        Liquid liquid = held != null ? held.GetComponentInChildren<Liquid>() : null;

        bool hasPlaysLeft = (liquid == null) || (liquid.plays < 3);

        bool validItem = held != null && held.CompareTag("New tag") && hasPlaysLeft;

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
            if (held == null)
            {
                interactQueued = false;
                return;
            }

            PrepareHeldItem();

            held = grabbingThrowing.heldItem;
            liquid = held != null ? held.GetComponentInChildren<Liquid>() : null;

            int plays = 0;
            float amount = 1f;

            if (liquid != null)
            {
                liquid.plays++;
                plays = liquid.plays;

                if (plays >= 3)
                {
                    interactQueued = false;
                    return;
                }

                amount = liquid.liquidLevel;
            }

            saveForLoadingScenes.SaveHeldItem(amount, plays);

            sl.Loadscene("Minigame");
            interactQueued = false;
        }
    }

    private void PrepareHeldItem()
    {
        GameObject held = grabbingThrowing.heldItem;
        if (held == null) return;

        Liquid existingLiquid = held.GetComponentInChildren<Liquid>();

        // If it does NOT have Liquid, replace with drug prefab
        if (existingLiquid == null)
        {
            GameObject newDrug = Instantiate(drug, held.transform.position, held.transform.rotation);

            Liquid newLiquid = newDrug.GetComponentInChildren<Liquid>();
            if (newLiquid != null)
            {
                newLiquid.liquidLevel = 1f;
            }

            Destroy(held);
            grabbingThrowing.heldItem = newDrug;
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