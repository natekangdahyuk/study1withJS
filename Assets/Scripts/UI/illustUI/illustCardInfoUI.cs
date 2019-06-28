using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Live2D.Cubism.Rendering;

public partial class illustCardInfoUI : baseUI
{
    [SerializeField]
    Text CharacterName;
    
    [SerializeField]
    RawImage Star;

    [SerializeField]
    RawImage Property;

    [SerializeField]
    RawImage Class;

    [SerializeField]
    Text bit;

    [SerializeField]
    Text Hp;

    [SerializeField]
    Text Defence;

    [SerializeField]
    Text Attack;

    [SerializeField]
    Text Critical;

    [SerializeField]
    Text Heal;

    [SerializeField]
    Text LeaderBuff;

    [SerializeField]
    Text ClassText;

    [SerializeField]
    Text CharCode;

    [SerializeField]
    GameObject minbtn;

    [SerializeField]
    GameObject maxbtn;

    CardData card = new CardData();
    public override void Init()
    {

    }

    public void ApplyInfo(CardReferenceData referenceData )
    {
        maxbtn.SetActive(true);

        card.Init(referenceData.ReferenceID, -1);
        
        CharacterName.text = card.Name;
        bit.text = card.bit.ToString();
       
        ClassText.text = UIUtil.ClassString(card.Class) + "/" + UIUtil.PropertyString(card.property);
        LeaderBuff.text = UIUtil.LeaderBuffString(card.leaderBuff, card.leaderBuffValue, card.property , card.Class );
        CharCode.text = card.referenceData.Code_Info;
        UIUtil.LoadProperty(Property, card.property);
        UIUtil.LoadClass(Class, card.Class);
        bit.color = new Color(1f, 1f, 75f / 255f, 1f);
        SetLevel(1);
    }

    public void SetLevel( int level )
    {
        card.Level = level;
        Hp.text = card.TotalHp.ToString("n0");
        Defence.text = card.TotalDefence.ToString("n0");
        Attack.text = card.Totaldamage.ToString("n0");
        Critical.text = card.Critical.ToString("n0");
        Heal.text = card.TotalHeal.ToString("n0");

        if (level == 1)
            UIUtil.LoadStarEx(Star, card.DefaultStar );
        else
            UIUtil.LoadStarEx(Star, 6);

    }

    public void OnMin()
    {
        minbtn.SetActive(false);
        maxbtn.SetActive(true);
        SetLevel(1);
    }

    public void OnMax()
    {
        minbtn.SetActive(true);
        maxbtn.SetActive(false);

        GradeDataReferenceData data = GradeDataTBL.GetData(6);
        SetLevel(data.maxlv);
    }
}