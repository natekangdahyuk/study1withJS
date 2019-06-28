using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IAP
using UnityEngine.Purchasing;
#endif


public class ShopItem : MonoBehaviour
{
    [SerializeField]
    Text cost;

    [SerializeField]
    RawImage rewardImage;
    //Image rewardImage;

    [SerializeField]
    Text reward;

    [SerializeField]
    Text won;

    [SerializeField]
    Image ruby;

    ShopReferenceData data;

    byte currentPayNow = 0;
    public void OnBuy()
    {
        currentPayNow = (byte)data.costType;
		
		if ( currentPayNow == 2 )
        {
            currentPayNow = 0;
            if( TopbarUI.CheckRuby( data.costValue ) == false )
                return;
        }

        

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

#if USE_SNS_LOGIN
		if (string.IsNullOrEmpty(data.product_code) == false)
		{
#if UNITY_IAP
        UnityIAP_StoreListener.I.RequestPurchase( data.product_code , GoogleProduct );
#else
        StartCoroutine(coPurchase());
#endif
		}
		else
		{
            PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
            popup.OnEnter();
            popup.SetEx( StringTBL.GetData( 902168 ) , data.costValue.ToString() , BuyShop , null , false, PopupOkCancel.SubType.Ruby );
		}        
#else

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();
        popup.SetEx( StringTBL.GetData( 902168 ) , data.costValue.ToString() , BuyShop , null , false, PopupOkCancel.SubType.Ruby );

        
#endif
    }

    public void BuyShop()
    {
        NetManager.BuyShop( currentPayNow , data.ReferenceID );
    }

#if UNITY_IAP
    public void GoogleProduct( Product product , bool bSuccess )
    {
        if( bSuccess )
        {
            NetManager.BuyShop( 0 , data.ReferenceID );
        }
        else
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData(902126) );
        }
        
    }
#endif



    IEnumerator coPurchase()
	{
		
		bool success = false;
		yield return StartCoroutine(PurchaseManager.I.InGamePurchase(data.product_code, (result) =>
		{
			success = result;
		}));

		if(success == true)
		{
			NetManager.BuyShop(0, data.ReferenceID);
		}
	}

    public void Apply(ShopReferenceData shopdata, int index )
    {
        data = shopdata;

		rewardImage.texture = ResourceManager.LoadTexture(shopdata.productImg);        
		//rewardImage.sprite = SpritePackerLoader.Instance.GetSprite(shopdata.productImg);
		cost.text = data.costValue.ToString("n0");
        ruby.gameObject.SetActive( true );
        won.gameObject.SetActive(false);

        switch( shopdata.productType )
        {
            case ProductType.Gold:
            //reward.text = data.productNum.ToString( "n0" ) + " G";
            cost.text = data.costValue.ToString( "n0" );
            ResourceManager.Load( this.gameObject , "ShopItem_gold_" + index.ToString());
            break;
            case ProductType.Ruby:
            //reward.text = data.productNum.ToString( "n0" ) + " RUBY";
            cost.text = data.costValue.ToString( "n0" );
            won.gameObject.SetActive( true );
            ruby.gameObject.SetActive( false );
            ResourceManager.Load( this.gameObject , "ShopItem_rubi_" + index.ToString() );
            break;
            case ProductType.Ap:
            //reward.text = data.productNum.ToString( "n0" ) + " AP";
            cost.text = data.costValue.ToString( "n0" );
            ResourceManager.Load( this.gameObject , "ShopItem_power_" + index.ToString() );
            break;
            case ProductType.Stone:
            //reward.text = data.productNum.ToString( "n0" ) + " STONE";
            cost.text = data.costValue.ToString( "n0" );
            ResourceManager.Load( this.gameObject , "ShopItem_summonstone_" + index.ToString() );
            break;
            default:
            //reward.text = data.productNum.ToString( "n0" );
            cost.text = data.costValue.ToString( "n0" );
            break;
        }
        
    }

}