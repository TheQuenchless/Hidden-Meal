using System.Collections.Generic;
using UnityEngine;

public class HouseColScript : MonoBehaviour
{
    [SerializeField] private string targetTag = "New tag";
    public List<GameObject> nonCollidingTargets = new List<GameObject>();

    private void Start()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(targetTag);
        nonCollidingTargets.AddRange(allTargets);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
            nonCollidingTargets.Remove(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (!nonCollidingTargets.Contains(other.gameObject))
                nonCollidingTargets.Add(other.gameObject);
        }
    }
}