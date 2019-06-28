using UnityEngine;
using System.Collections;

public class NoticePopup : baseUI
{
    [SerializeField]
    GameObject[] Page;

    [SerializeField]
    GameObject[] Page_indicator;


    public ScrollController scroll;

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

        scroll.actoin = OnNextPage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        base.OnExit();
    }

    public void Apply()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );
        OnEnter();
    }

    public void OnNextPage( bool bNext )
    {
        Page[ current ].SetActive( false );
        Page_indicator[ current ].SetActive( false );

        current++;

        if( current >= Page.Length )
        {
            current = 0;
        }

        Page[ current ].SetActive( true );
        Page_indicator[ current ].SetActive( true );

    }

}
