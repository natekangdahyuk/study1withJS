using UnityEngine;
using System.Collections;

public class TutorialUI : baseUI
{
    [SerializeField]
    GameObject[] Page;

    int current = 0;
    public override void Init()
    {

    }
    // Use this for initialization
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        base.OnExit();
    }

    public override bool OnExit()
    {
        for (int i = 0; i < Page.Length; i++)
        {
            if (Page[i].activeSelf)
            {
                Page[i].SetActive(false);
                return false;
            }
        }

        return base.OnExit();
    }
    public void Apply()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);
        OnEnter();

        for (int i = 0; i < Page.Length; i++)
        {
            Page[i].SetActive(false);
        }
    }

    public void OnGuide( int index )
    {
        for( int i =0; i < Page.Length; i++)
        {            
            Page[i].SetActive(index == i ? true : false);
        }
    }
  

}
