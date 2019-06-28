using UnityEngine;
using System.Collections.Generic;
#if UNITY_IAP
using UnityEngine.Purchasing;
#endif
public class UnitylapManager : Singleton<UnitylapManager>
{
#if UNITY_IAP
    List<UnityIAP_StoreListener.ProductData> ProductList = new List<UnityIAP_StoreListener.ProductData>();

	public bool IsServiceAvailable { get { return UnityIAP_StoreListener.I.IsServiceAvailable; } }
#endif
   public void Init()
    {
#if UNITY_IAP
        List<IReferenceDataByGroup> list = ShopTBL.GetGroup( ShopType.Ruby );

        for( int i= 0 ; i < list.Count ; i++ )
        {
            ShopReferenceData shopdata = ( ShopReferenceData )list[ i ];
            IDs ids = new IDs();
            ids.Add( shopdata.product_code , GooglePlay.Name );
            UnityIAP_StoreListener.ProductData data = new UnityIAP_StoreListener.ProductData( shopdata.product_code , UnityEngine.Purchasing.ProductType.Consumable , ids );

            ProductList.Add( data );
        }

        PackageTBL TBL = TBLManager.I.GetTable<PackageTBL>( TABLELIST_TYPE.Packeage );

        for( int i=0 ; i < TBL.list.Count ; i++)
        {
            IDs ids = new IDs();
            ids.Add( TBL.list[i].productcode , GooglePlay.Name );
            UnityIAP_StoreListener.ProductData data = new UnityIAP_StoreListener.ProductData( TBL.list[ i ].productcode , UnityEngine.Purchasing.ProductType.Consumable , ids );

            ProductList.Add( data );
        }


        UnityIAP_StoreListener.I.InitializeIAP( ProductList );
#endif
    }
}