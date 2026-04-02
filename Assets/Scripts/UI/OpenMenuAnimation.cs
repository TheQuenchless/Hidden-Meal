using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class OpenMenuAnimation : MonoBehaviour
{
    [Header("animation")]
    [SerializeField]public RectTransform panel;
    [SerializeField]public GameObject menu;      
    [SerializeField]public float duration = 1f;   
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float time;

    private PlayerInput playerInput;
    private bool isOpen = false;
    private bool isMoving = false;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.actions["toggleMenu"].performed += ctx => ToggleMenu();
    }


    

    void Start()
    {
        
        RectTransform parentRect = panel.parent.GetComponent<RectTransform>();
        targetPosition = Vector2.zero;

        startPosition = new Vector2(0, -parentRect.rect.height / 2f - panel.rect.height / 2f);
        panel.anchoredPosition = startPosition;
        menu.SetActive(false);
    }

    

    void ToggleMenu()
    {
         if (isMoving) return;

        isOpen = !isOpen;

        Vector2 target = isOpen ? targetPosition : startPosition;

        StartCoroutine(AnimatePanel(target));
        

    }

    public void ButtonClick(){ToggleMenu();}

    IEnumerator AnimatePanel(Vector2 target)
    {
        isMoving = true;
        if (isOpen) menu.SetActive(true);

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
        if (!isOpen) menu.SetActive(false);
        isMoving = false;
    }
}
