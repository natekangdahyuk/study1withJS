using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MailUI : baseUI
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    GameObjectPool ObjectPool;

    [SerializeField]
    Text stone;

    [SerializeField]
    Text GoldStone;

    List<MailItem> mailItem = new List<MailItem>();

    RectTransform rectT = null ;

    public void Awake()
    {
        if( rectT == null )
            rectT = content.GetComponent<RectTransform>();

        ObjectPool.Init();
    }

    private void Start()
    {
        RectTransform form = GetComponent<RectTransform>();
        form.anchoredPosition = Vector2.zero;
        form.sizeDelta = Vector2.zero;
    }


    public override void Init()
    {

    }

    public void Apply()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );
        OnEnter();

        stone.text = PlayerData.I.Stone.ToString( "n0" );
        GoldStone.text = PlayerData.I.mileage.ToString( "n0" );
        //NetManager.GetMailList();     
        RecvMailList();

        MailManager.I.SetNew( false );
    }

    public void OnRecvMail( long index )
    {
        for( int i = 0 ; i < mailItem.Count ; i++ )
        {
            if( mailItem[ i ].maildata.UID == index )
            {
                ObjectPool.Delete( mailItem[ i ].gameObject );
                mailItem.Remove( mailItem[ i ] );
                break;
            }            
        }

        if( mailItem.Count * 174 > 980 )
            rectT.sizeDelta = new Vector2( rectT.sizeDelta.x , mailItem.Count * 174 );
        else
            rectT.sizeDelta = new Vector2( rectT.sizeDelta.x , 980 );

        stone.text = PlayerData.I.Stone.ToString( "n0" );
        GoldStone.text = PlayerData.I.mileage.ToString( "n0" );
    }

    public void RecvMailList()
    {
        for( int i=0 ; i < mailItem.Count ; i++ )
        {
            ObjectPool.Delete( mailItem[ i ].gameObject );
        }

        mailItem.Clear();

        foreach( var vale in MailManager.I.maliList )
        {
            GameObject go = ObjectPool.New();
            go.SetActive( true );
            go.transform.SetParent( content.transform );
            MailItem item = go.GetComponent<MailItem>();
            item.Apply( vale.Value );
            mailItem.Add( item );
        }


        if( MailManager.I.maliList.Count * 174  > 980 )
            rectT.sizeDelta = new Vector2( rectT.sizeDelta.x , MailManager.I.maliList.Count * 174 );
        else
            rectT.sizeDelta = new Vector2( rectT.sizeDelta.x , 980 );
    }
}