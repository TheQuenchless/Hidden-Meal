using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class OpenMenuAnimation : MonoBehaviour
{
    [Header("animations")]
    public RectTransform panel;      
    public float duration = 1f;   
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float time;

    [Header("input")]

    private UiInputs input;
    private bool isOpen = false;
    private bool isMoving = false;

    void Awake()
    {
        input = new UiInputs();

        input.UI.ToggleMenu.performed += ctx => ToggleMenu();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        
        RectTransform parentRect = panel.parent.GetComponent<RectTransform>();
        targetPosition = Vector2.zero;

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

    void ToggleMenu()
    {
         if (isMoving) return;

        isOpen = !isOpen;

        Vector2 target = isOpen ? targetPosition : startPosition;

        StartCoroutine(AnimatePanel(target));

    }

    IEnumerator AnimatePanel(Vector2 target)
    {
        isMoving = true;

        Vector2 current = panel.anchoredPosition;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, time / duration);

            panel.anchoredPosition = Vector2.Lerp(current, target, t);

            yield return null;
        }

        panel.anchoredPosition = target;
        isMoving = false;
    }
}
