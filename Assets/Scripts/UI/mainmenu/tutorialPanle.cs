using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Collections;

public class tutorialPanle : MonoBehaviour
{
    [SerializeField]private TMP_Text text;
    [SerializeField]private float delay= 0.05f;
    [TextArea(3, 7)]
    [SerializeField]private string[] pages;
    private int currentPage = 0;
    private int maxPage;
    private Coroutine typingRoutine;


    void Start()
    {
        maxPage = pages.Length;
    }


    private void UpdatePage()
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(typetext(pages[currentPage]));
    }


    public void Back()
    {
        if (currentPage >= 0 && currentPage != 0)
        {
            currentPage --;
            UpdatePage();
        }
    }

    public void Continue()
    {
        if(currentPage < maxPage && currentPage != maxPage)
        {
            currentPage ++;
            UpdatePage();
        }
    }

    private IEnumerator typetext(string pagetext)
    {
        text.text = "";

        foreach (char c in pagetext)
        {
            text.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
