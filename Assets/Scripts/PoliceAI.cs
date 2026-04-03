using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private float speed = 3f;
    private Transform police;
    private float distance = 10f;
    private float coneAngle = 75f;
    private int rayCount = 20;
    [HideInInspector]public float shiftTimer = 0f;
    public int shiftStartTime = 120;
    public int shiftEndTime = 140;
    private bool checkedBed = false;
    private bool checkedBath = false;
    Vector3 targetPos = new Vector3(-20, 0, 0);
    private Dictionary<string, Transform> points;
    void Start()
    {
        police = GetComponentInParent<Transform>();
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

        GetTime();

        if (hit.collider != null && hit.collider.CompareTag("New tag"))
        {
            Loss();
        }

        Move();

        //Debug.Log($"time: {GetTime()} targetPos: {targetPos}");
    }

    private float GetTime()
    {
        shiftTimer += Time.deltaTime;

        GetTargetPos(shiftTimer);

        if (shiftTimer >= shiftEndTime)
        {
            shiftTimer = 0f;
        }

        return shiftTimer;
    }

    private Vector3 GetTargetPos(float timer)
    {
        Transform based = points["Base"];
        Transform road = points["Road"];
        Transform entrance = points["Entrance"];
        Transform livingRoom = points["LivingRoom"];
        Transform bedRoom = points["BedRoom"];
        Transform kitchen = points["Kitchen"];
        Transform bathRoom = points["BathRoom"];

        float threshold = 0.05f; // distance tolerance
        Vector3 policePos = police.position;

        if (timer >= shiftEndTime)
        {
            checkedBath = false;
            checkedBed = false;
            targetPos = based.position;
            return targetPos;
        }

        if (timer < shiftStartTime)
        {
            targetPos = based.position;
            return targetPos;
        }

        if (Vector3.Distance(policePos, based.position) < threshold)
        {
            targetPos = road.position;
        }
        else if (Vector3.Distance(policePos, road.position) < threshold)
        {
            targetPos = entrance.position;
        }
        else if (Vector3.Distance(policePos, entrance.position) < threshold)
        {
            targetPos = livingRoom.position;
        }
        else if (Vector3.Distance(policePos, bedRoom.position) < threshold)
        {
            targetPos = livingRoom.position;
        }
        else if (Vector3.Distance(policePos, bathRoom.position) < threshold)
        {
            targetPos = kitchen.position;
        }
        else if (Vector3.Distance(policePos, livingRoom.position) < threshold)
        {
            if (!checkedBed)
            {
                int choice = Random.Range(1, 4);
                if (choice == 1)
                {
                    targetPos =  kitchen.position;
                }
                else if (choice == 2)
                {
                    targetPos =  bedRoom.position;
                }
                else
                {
                    targetPos =  entrance.position;
                }
            }
            else
            {
                targetPos = (Random.value < 0.5f) ? entrance.position : kitchen.position;
            }
        }
        else if (Vector3.Distance(policePos, kitchen.position) < threshold)
        {
            if (!checkedBath)
            {
                targetPos = (Random.value < 0.5f) ? livingRoom.position : bathRoom.position;
                if (targetPos == bathRoom.position) checkedBath = true;
            }
            else
            {
                targetPos = livingRoom.position;
                if (targetPos == bathRoom.position) checkedBed = true;
            }
        }

        return targetPos;
    }

    private void Move()
    {
        Vector3 dir = targetPos - police.position;
        if (dir.magnitude <= speed * Time.deltaTime)
        {
            police.transform.position = targetPos;
        }

        dir.Normalize();

        police.transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-dir);
            police.rotation = Quaternion.Slerp(police.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private RaycastHit Observe()
    {
        Vector3 origin = transform.position + Vector3.up * 0.501f;
        float halfAngle = coneAngle / 2f;

        RaycastHit hit = default;

        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            // Rotate forward around the up axis for horizontal sweep
            Vector3 direction = Quaternion.Euler(0, angle, 0) * -transform.forward;

            if (Physics.SphereCast(origin, 1f, direction, out RaycastHit rayHit, distance))
            {
                hit = rayHit;
            }

            Debug.DrawRay(origin, direction.normalized * distance, Color.red, 0.1f);
        }

        return hit;
    }

    private void Loss()
    {
        Debug.Log("YOU LOST /n SO BAD /n LEAVE");
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
