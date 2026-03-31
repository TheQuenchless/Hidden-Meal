using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingThrowing : MonoBehaviour
{
    [SerializeField] private float maxDistance = 2.5f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform hands;
    private Transform player;
    BasicLook basicLook;
    MoveHands moveHands;
    private Vector3 direction;
    private bool actionQueued = false;
    GameObject heldItem;
    Rigidbody rb;
    void Start()
    {
        basicLook = cam.GetComponent<BasicLook>();
        moveHands = GetComponentInChildren<MoveHands>();
        player = GetComponentInParent<Transform>();
    }

    void Update()
    {
        if (actionQueued)
        {
            if (heldItem == null)
            {
                TryGrab();
            }
            else
            {
                TryThrow();
            }

            actionQueued = false;
        }

        if (heldItem != null)
        {
            heldItem.transform.position = hands.position;

            Quaternion targetRotation = Quaternion.LookRotation(moveHands.direction);
            heldItem.transform.rotation = targetRotation;
        }
    }

    private void TryGrab()
    {
        if (heldItem != null) return;

        Ray ray = cam.ScreenPointToRay(basicLook.trueMousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.attachedRigidbody) return;

            Vector3 worldPos = hit.point;
            direction = worldPos - player.position;

            if (direction.magnitude > maxDistance) return;

            heldItem = hit.collider.gameObject;

            rb = hit.collider.attachedRigidbody;
            rb.isKinematic = true;
            rb.useGravity = false;

            //Debug.Log($"heldItem: {heldItem}");
        }
    }

    private void TryThrow()
    {
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(new Vector3(moveHands.direction.x * 4, 5, moveHands.direction.z * 4), ForceMode.Impulse);

        rb = null;
        heldItem = null;
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            actionQueued = true;
        }
    }
}
