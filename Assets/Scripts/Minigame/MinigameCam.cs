using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameCam : MonoBehaviour
{
    [SerializeField] private GameObject vial;
    private Vector2 mousePos;
    [NonSerialized] public Vector2 trueMousePos;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetPos = vial.transform.position;

        transform.position = Vector3.Lerp(transform.position, targetPos + new Vector3(0, 0, -5), 5f * Time.deltaTime);

        Quaternion lookRot = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = lookRot;

        //Debug.Log($"mousePos: {mousePos}");
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        trueMousePos = Mouse.current.position.ReadValue();
        mousePos.x = Mouse.current.position.ReadValue().x / Screen.width - 0.5f;
        mousePos.y = Mouse.current.position.ReadValue().y / Screen.height - 0.5f;
    }
}
