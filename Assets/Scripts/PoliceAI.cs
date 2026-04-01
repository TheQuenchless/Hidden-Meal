using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{
    [SerializeField] private GameObject map;
    private float distance = 10f;
    private float coneAngle = 60f;
    private int rayCount = 20;
    float timer = 0f;
    int cycle = 0;
    private Dictionary<string, Transform> points;
    void Start()
    {
        points = new Dictionary<string, Transform>();

        foreach (Transform child in map.transform)
        {
            points[child.name] = child;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = Observe();

        if (hit.collider.CompareTag("New Tag"))
        {
            Loss();
        }
    }

    private void GetTime()
    {
        timer += Time.deltaTime;

        if (timer >= 120f)
        {
            cycle++;
            if (cycle == 0) return; // give one cycle headstart

            GetPosition();

            timer = 0f;
        }
    }

    private void GetPosition()
    {
        Transform based = points["Base"];
        Transform road = points["Road"];
        Transform entrance = points["Entrance"];
        Transform livingRoom = points["LivingRoom"];
        Transform kitchen = points["Kitchen"];
        Transform bathRoom = points["BathRoom"];
        Transform bedRoom = points["BedRoom"];


        
    }

    private RaycastHit Observe()
    {
        Vector3 origin = transform.position;
        float halfAngle = coneAngle / 2f;
 
        for (int i = 0; i < rayCount; i++)
        {
            // Interpolate angle across the cone
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            // Rotate forward direction
            Vector3 direction = RotateVector(transform.right, angle);

            Physics.Raycast(origin, direction, out RaycastHit hit, distance);

            return hit;
        }

        return default;
    }

    private void Loss()
    {
        
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        return new Vector2(
            cos * v.x - sin * v.y,
            sin * v.x + cos * v.y
        );
    }
}
