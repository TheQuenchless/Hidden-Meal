using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Commands;
using Unity.VisualScripting;
using UnityEngine;

public class JunkieAI : MonoBehaviour
{
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private GameObject YOINK;
    [SerializeField] private GameObject BOUGHT;
    [SerializeField] private HouseColScript houseTrigger;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip yoinkSound;

    private float shiftTimer;
    private float shiftStartTime;
    private Vector3 idlePos;
    private Vector3 targetPos;
    private PoliceAI police;
    private Wallet wallet;
    private Rigidbody rb;

    void Start()
    {
        police = GameObject.Find("Policeman").GetComponent<PoliceAI>();
        shiftStartTime = police.shiftStartTime;

        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<Wallet>();

        rb = GetComponent<Rigidbody>();

        idlePos = transform.position;
    }

    void Update()
    {
        shiftTimer = police.shiftTimer;

        if (houseTrigger.nonCollidingTargets.Count == 0 || shiftTimer < shiftStartTime)
        {
            targetPos = idlePos;
            MoveToTarget();
            return;
        }

        // Find closest target
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var obj in houseTrigger.nonCollidingTargets)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < closestDist)
            {
                closest = obj;
                closestDist = dist;
            }
        }

        if (closest != null)
        {
            targetPos = closest.transform.position;
        }
        else
        {
            targetPos = idlePos;
        }

        MoveToTarget();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("New tag")) return;

        if (shiftTimer >= shiftStartTime)
            Yoink(collision.gameObject);
        else
            Buy(collision.gameObject);
    }

    private void Yoink(GameObject obj)
    {
        houseTrigger.nonCollidingTargets.Remove(obj);

        GameObject popup = Instantiate(YOINK, transform.position, Quaternion.identity);
        Destroy(popup, 1f);

        audioSource.PlayOneShot(yoinkSound);

        Destroy(obj);
    }

    private void Buy(GameObject obj)
    {
        houseTrigger.nonCollidingTargets.Remove(obj);

        float costFac = obj.GetComponentInChildren<Liquid>().liquidLevel;
        int baseCost = 30;

        int cost = Mathf.RoundToInt(Mathf.Lerp(0, baseCost, costFac));

        wallet.money += cost;

        GameObject popup = Instantiate(BOUGHT, transform.position, Quaternion.identity);
        Destroy(popup, 1f);

        audioSource.PlayOneShot(buySound);

        Destroy(obj);
    }

    private void MoveToTarget()
    {
        targetPos = new Vector3(targetPos.x, 0.5f, targetPos.z);
        Vector3 direction = targetPos - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (direction.sqrMagnitude > 0.1f)
        {
            rb.isKinematic = true;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                lookRotation * Quaternion.Euler(-90f, 0f, 0f), 
                Time.deltaTime * 5f
            );
        }
        else
        {
            rb.isKinematic = false;
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                Quaternion.Euler(-90f, 0f, -15f), 
                Time.deltaTime * 5f
            );
        }
    }
}