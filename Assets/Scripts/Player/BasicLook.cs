using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicLook : MonoBehaviour
{
    [SerializeField] private float height = 20f;
    [SerializeField] private float length = 20f;
    [SerializeField] private Texture2D Crosshair;
    [SerializeField] private GameObject player;
    BasicMove basicMove;
    private Vector2 mousePos;
    [NonSerialized] public Vector2 trueMousePos;
    private bool turnLeft;
    private bool turnRight;
    private float orbitAngle = -90f;
    void Start()
    {
        basicMove = player.GetComponent<BasicMove>();

        Vector2 hotspot = new Vector2(Crosshair.width / 2f, Crosshair.height / 2f);
        Cursor.SetCursor(Crosshair, hotspot, CursorMode.ForceSoftware);
    }

    void Update()
    {
        Vector3 playerPos = basicMove.col.transform.position;

        if (turnLeft)
        {
            orbitAngle -= 90f;
            turnLeft = false;
        }
        else if (turnRight)
        {
            orbitAngle += 90f;
            turnRight = false;
        }

        float angleRad = orbitAngle * Mathf.Deg2Rad;

        float x = Mathf.Cos(angleRad) * length;
        float z = Mathf.Sin(angleRad) * length;

        Vector3 targetPos = playerPos + new Vector3(x, height, z);
        transform.position = Vector3.Lerp(transform.position, targetPos, 5f * Time.deltaTime);

        Quaternion lookRot = Quaternion.LookRotation(playerPos - transform.position);

        Quaternion pitch = Quaternion.AngleAxis(-mousePos.y, Vector3.right);
        Quaternion yaw = Quaternion.AngleAxis(mousePos.x, Vector3.up); // adjust axis if needed

        Quaternion finalRot = lookRot * yaw * pitch;
        transform.rotation = finalRot;

        //Debug.Log($"mousePos: {mousePos}");
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        trueMousePos = Mouse.current.position.ReadValue();
        mousePos.x = Mouse.current.position.ReadValue().x / Screen.width - 0.5f;
        mousePos.y = Mouse.current.position.ReadValue().y / Screen.height - 0.5f;
    }

    public void TurnLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("fuckyoufuckyoufuckyoufuckyoufucjoyoduckyoufuckyoufuckyoufucktoufuckyou");
            turnLeft = true;
        }
    }

    public void TurnRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            turnRight = true;
        }
    }
}
