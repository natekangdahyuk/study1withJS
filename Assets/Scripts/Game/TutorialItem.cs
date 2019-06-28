using UnityEngine;
using System;


public class TutorialItem : MonoBehaviour
{
    public Action action;

    [SerializeField]
    GameObject[] Page;

    [SerializeField]
    GameObject[] Page_indicator;

    public ScrollController scroll;

    int current = 0;

    private void Start()
    {
        RectTransform form = GetComponent<RectTransform>();
        form.anchoredPosition = Vector2.zero;
        form.sizeDelta = Vector2.zero;

        if(scroll!= null )
            scroll.actoin = OnNextPage;
    }

    public void OnExit()
    {
        gameObject.SetActive( false );
        if(action != null )
            action();
    }

    public void OnNextPage(bool bNext)
    {
        Page[current].SetActive(false);
        Page_indicator[current].SetActive(false);

        current++;

        if (current >= Page.Length)
        {
            current = 0;
        }

        Page[current].SetActive(true);
        Page_indicator[current].SetActive(true);

    }
}