using UnityEngine;

using System.Collections.Generic;

public class CardRewardInfoPopup : baseUI
{
    [SerializeField]
    GameObject Content;

    List<Card> cardlist = new List<Card>();

    public override void Init()
    {
      
    }

    public void Exit()
    {
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void Apply( StageReferenceData stageData )
    {
        gameObject.SetActive( true );

        for( int i=0 ; i < cardlist.Count ; i++ )
        {
            InvenCardObjectPool.Delete( cardlist[ i ].gameObject );            
        }
        cardlist.Clear();

        //if( cardlist.Count == 0 )
        //{
            for( int i = 0 ; i < stageData.CharRewardList.Length ; i++ )
            {
                Card TargetCard = InvenCardObjectPool.Get();
                TargetCard.transform.SetParent( Content.transform );
                TargetCard.transform.localPosition = Vector3.zero;
                TargetCard.transform.localScale = Vector3.one;
                TargetCard.cardData = new CardData();
            
                TargetCard.cardData.ApplyData( CardTBL.GetDataByCharacterID( stageData.CharRewardList[ i ] ).ReferenceID , -1 );

                TargetCard.cardData.Star = TargetCard.cardData.referenceData.star;
                TargetCard.Apply();                
                cardlist.Add( TargetCard );
                TargetCard.SetRewardMode();
                TargetCard.SetStar( TargetCard.cardData , true );
        }

            Content.GetComponent<RectTransform>().sizeDelta = new Vector2( cardlist.Count * 115 , Content.GetComponent<RectTransform>().sizeDelta.y );
        //}
        //else
        //{
        //    for( int i = 0 ; i < stageData.CharRewardList.Length ; i++ )
        //    {
        //        cardlist[i].cardData.ApplyData( CardTBL.GetDataByCharacterID( stageData.CharRewardList[ i ] ).ReferenceID , -1 );
        //    }
        //}
        
        

    }
}