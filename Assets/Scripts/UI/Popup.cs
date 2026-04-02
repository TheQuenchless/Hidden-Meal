using UnityEngine;

public class Popup : MonoBehaviour
{
    float speed = 0.15f;
    void Update()
    {
        transform.position += Vector3.up * speed;
        speed *= 0.9f;
    }
}
