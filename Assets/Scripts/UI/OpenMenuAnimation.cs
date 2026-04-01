using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class OpenMenuAnimation : MonoBehaviour
{
    [Header("animations")]
    public RectTransform panel;      // Dein Panel hier im Inspector ziehen
    public float duration = 1f;   

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float time;
    [Header("input")]

    public UIDocument input;
    
    void Start()
    {
        
        RectTransform parentRect = panel.parent.GetComponent<RectTransform>();
        targetPosition = Vector2.zero; // AnchoredPosition (0,0) = Mitte bei mittigen Anchors

        
        startPosition = new Vector2(0, -parentRect.rect.height / 2f - panel.rect.height / 2f);
        panel.anchoredPosition = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, time / duration); // Smooth Bewegung
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
        }
    }
}
