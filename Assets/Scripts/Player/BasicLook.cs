using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicLook : MonoBehaviour
{
    [SerializeField] private float yOffset = 12f;
    [SerializeField] private Texture2D Crosshair;
    [SerializeField] private GameObject player;
    BasicMove basicMove;
    private Vector2 mousePos;
    void Start()
    {
        basicMove = player.GetComponent<BasicMove>();

        Cursor.SetCursor(Crosshair, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        Vector3 playerPos = basicMove.col.transform.position;

        transform.position = Vector3.up * yOffset + playerPos;

        Quaternion pitch = Quaternion.AngleAxis(-mousePos.y * 2 + 90, Vector3.right);
        Quaternion yaw = Quaternion.AngleAxis(mousePos.x * 2, Vector3.forward); // use forward because cam is looking down

        transform.rotation = yaw * pitch;
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        mousePos.x = Mouse.current.position.ReadValue().x / Screen.width;
        mousePos.y = Mouse.current.position.ReadValue().y / Screen.height;
    }
}
