
public enum ShopType
{
    Ruby = 1,
    Gold,
    Ap,
    Stone,
	MAX = Stone
}

public enum ProductType
{
    Ruby = 1,
    Gold,
    Ap,
    Stone,
}

public enum CostType
{
    Money = 1,
    Ruby,
}
public class ShopReferenceData : IReferenceDataByKey, IReferenceDataByGroup
{    
    public object GetKey()
    {
        return ReferenceID;
    }

    public object GetGroupKey()
    {
        return shopType;
    }

    public int ReferenceID;

    public ShopType shopType;

    public string productImg;

    public ProductType productType;

    public int productNum;

    public CostType costType;

    public int costValue;    

	public string product_code;

    public string rewardString;

}
