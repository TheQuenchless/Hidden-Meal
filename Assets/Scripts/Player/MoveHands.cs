using UnityEngine;

public class MoveHands : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0.67f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    BasicLook basicLook;
    public Vector3 direction;
    void Start()
    {
        basicLook = cam.GetComponent<BasicLook>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(basicLook.trueMousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 worldPos = hit.point;
            Vector3 pos = new Vector3(worldPos.x, player.transform.position.y - 0.3f, worldPos.z);
            direction = pos - player.transform.position;

            if (direction.magnitude > maxDistance)
            {
                direction = direction.normalized * maxDistance;
            }

            Vector3 clampedPos = player.transform.position + direction;

            transform.position = clampedPos;
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
