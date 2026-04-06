using UnityEngine;

public class Popup : MonoBehaviour
{
    private float speedY = 0.15f;
    private float speedX = 0.08f;
    private Vector3 rand;
    void Start()
    {
        rand = Random.onUnitCircle;
    }
    void Update()
    {
        transform.position += new Vector3(rand.x * speedX, speedY, rand.z * speedX);
        speedY *= 0.9f;
        speedX *= 0.85f;
    }
}
