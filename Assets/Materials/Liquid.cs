using Unity.VisualScripting;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    Renderer rend;
    [Range(0, 1)]
    public float liquidLevel = 0.5f;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        Bounds bounds = rend.bounds;

        rend.material.SetFloat("_BoundsMinY", bounds.min.y);
        rend.material.SetFloat("_BoundsMaxY", bounds.max.y);
        rend.material.SetFloat("_LiquidLevel", liquidLevel);
    }
}