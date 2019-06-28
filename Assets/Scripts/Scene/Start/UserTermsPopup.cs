using UnityEngine;
using UnityEngine.UI;

public class UserTermsPopup : baseUI
{
    [SerializeField]
    Toggle toggle1;

    [SerializeField]
    Toggle toggle2;

    [SerializeField]
    GameObject[] check;

    int count = 0;
    public void Start()
    {
        gameObject.SetActive( GameOption.FirstEnter );
    }

    public override void Init()
    {
        
    }

    public void OnOk1()
    {
        if( toggle1.isOn )
        {
            count++;
            toggle1.interactable = false;
            check[ 0 ].SetActive( true );

            if( count == 2 )
                gameObject.SetActive( false );
        }
    }

    public void OnOk2()
    {
        if( toggle2.isOn )
        {
            count++;
            toggle2.interactable = false;
            check[ 1 ].SetActive( true );

            if( count == 2 )
                gameObject.SetActive( false );
        }
    }
}