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

        bool validItem = held != null && held.CompareTag("New tag");

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
            PrepareHeldItem();
            Liquid liquid = held.GetComponentInChildren<Liquid>();
            float amount = (liquid != null) ? liquid.liquidLevel : 1f;

            saveForLoadingScenes.SaveHeldItem(amount);
            sl.Loadscene("Minigame");
            interactQueued = false;
        }
    }

    private void PrepareHeldItem()
    {
        GameObject held = grabbingThrowing.heldItem;
        if (held == null) return;

        float liquidAmount = 1f;

        // Check if it already has Liquid
        if (held.TryGetComponent<Liquid>(out var existingLiquid))
        {
            liquidAmount = existingLiquid.liquidLevel;
        }

        // If it does NOT have Liquid, replace with drug prefab
        if (!held.TryGetComponent<Liquid>(out _))
        {
            GameObject newDrug = Instantiate(drug, held.transform.position, held.transform.rotation);

            // Assign liquid = 1 (default)
            Liquid newLiquid = newDrug.GetComponentInChildren<Liquid>();
            if (newLiquid != null)
            {
                newLiquid.liquidLevel = 1f;
            }

            Destroy(held);
            grabbingThrowing.heldItem = newDrug;
        }
        else
        {
            // Ensure value carries over (optional safety)
            Liquid l = held.GetComponent<Liquid>();
            l.liquidLevel = liquidAmount;
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