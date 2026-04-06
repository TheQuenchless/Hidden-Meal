using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private float speed = 3f;
    private Transform police;
    private float distance = 8f;
    private float coneAngle = 75f;
    private int rayCount = 40;
    [HideInInspector]public float shiftTimer = 0f;
    public int shiftStartTime = 90;
    public int shiftEndTime = 115;
    Vector3 targetPos;
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

        bool validItem = hit.collider != null &&
                 hit.collider.CompareTag("New tag") &&
                 (!hit.collider.TryGetComponent(out DrugIngredient ingredient) || !ingredient.bought);

        if (validItem)
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

        float threshold = 0.07f; // distance tolerance
        Vector3 policePos = police.position;

        if (timer >= shiftEndTime)
        {
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
            targetPos = (Random.value < 0.5f) ? bedRoom.position : kitchen.position;
        }
        else if (Vector3.Distance(policePos, kitchen.position) < threshold)
        {
            targetPos = (Random.value < 0.5f) ? livingRoom.position : bathRoom.position;
        }

        return targetPos;
    }

    private void Move()
    {
        Vector3 dir = targetPos - police.position;

        Vector3 move = dir.normalized * speed * Time.deltaTime;
        if (move.magnitude > dir.magnitude)
        {
            move = dir;
        }
        police.position += move;

        if (dir.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-dir);
            police.rotation = Quaternion.Slerp(police.rotation, targetRotation, Time.deltaTime * 3f);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0, -60, 0);
            police.rotation = Quaternion.Slerp(police.rotation, targetRotation, Time.deltaTime * 3f);
        }

        //Debug.Log(targetPos);
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

            int playerLayer = LayerMask.NameToLayer("Player");
            int layerMask = ~(1 << playerLayer);

            if (Physics.SphereCast(origin, radius: 1f, direction, out RaycastHit rayHit, distance, layerMask, QueryTriggerInteraction.Ignore))
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
}
