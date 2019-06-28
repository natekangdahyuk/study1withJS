using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopPackageItem : MonoBehaviour
{
    public Button btn;
    public Text[] value;
    public Text price;
    public RawImage TitleImage;
    public RawImage BGImage;
    public Text Count;

    public PackageData data = null;
    public Action<PackageData> onBuy;
    public void Apply( PackageData _data )
    {
        if( _data.IsBuy() == false )
            SetInteractable( false );


        Count.text = _data.buyCount.ToString() + "/" + _data.maxCnt.ToString();

        if( data == _data )
            return;

        data = _data;
        
        price.text = data.price.ToString( "n0" ) ;
        value[ 0 ].text = data.ruby.ToString( "n0" ) + "<color=#FFFFFFFF> <size=20> 개 </size></color>";
        value[ 1 ].text = data.stone.ToString( "n0" ) + "<color=#FFFFFFFF> <size=20> 개 </size></color>";
        value[ 2 ].text = data.gold.ToString( "n0" ) + "<color=#FFFFFFFF> <size=20> 개 </size></color>";

        BGImage.texture = ResourceManager.LoadTexture( data.BGImage );
        TitleImage.texture = ResourceManager.LoadTexture( data.TitleImage );

    }

    public void OnClick()
    {
        onBuy( data );        
    }

    public void SetInteractable( bool bValue )
    {
        btn.interactable = bValue;
    }

}