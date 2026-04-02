using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicLook : MonoBehaviour
{
    [SerializeField] private float yOffset = 48f;
    [SerializeField] private float zOffset = 13f;
    [SerializeField] private Texture2D Crosshair;
    [SerializeField] private GameObject player;
    BasicMove basicMove;
    private Vector2 mousePos;
    public Vector2 trueMousePos;
    void Start()
    {
        basicMove = player.GetComponent<BasicMove>();

        Vector2 hotspot = new Vector2(Crosshair.width / 2f, Crosshair.height / 2f);
        Cursor.SetCursor(Crosshair, hotspot, CursorMode.ForceSoftware);
    }

    void Update()
    {
        Vector3 playerPos = basicMove.col.transform.position;

        transform.position = new Vector3(0, yOffset, -zOffset) + playerPos;

        Quaternion pitch = Quaternion.AngleAxis(-mousePos.y + 45f, Vector3.right);
        Quaternion yaw = Quaternion.AngleAxis(mousePos.x, Vector3.forward); // use forward because cam is looking down

        transform.rotation = yaw * pitch;

        //Debug.Log($"mousePos: {mousePos}");
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        trueMousePos = Mouse.current.position.ReadValue();
        mousePos.x = Mouse.current.position.ReadValue().x / Screen.width - 0.5f;
        mousePos.y = Mouse.current.position.ReadValue().y / Screen.height - 0.5f;
    }
}
