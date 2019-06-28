using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LimitCompletePopup : baseUI
{
    [SerializeField]
    Text Title;

    CardInfoGroup[] info = new CardInfoGroup[2];
    

    [SerializeField]
    GameObject[] cardPosition;

    [SerializeField]
    GameObject[] cardInfoPosition;

    Card[] card = new Card[2];

    void Awake()
    {
        for (int i = 0; i < card.Length; i++)
        {
            card[i] = InvenCardObjectPool.Get();
            card[i].transform.SetParent(cardPosition[i].transform);
            card[i].transform.localPosition = Vector3.zero;
            card[i].transform.localScale = Vector3.one;
        }

        for (int i = 0; i < card.Length; i++)
        {
            info[i] = CardInfoGroup.Load<CardInfoGroup>(cardInfoPosition[i], "CardInfoGroup");
        }

        gameObject.SetActive(false);
    }
    public override void Init()
    {

    }

    public void Apply(CardData carddata, CardData carddata2)
    {
        gameObject.SetActive(true);
        card[0].ApplyData(carddata);
        card[1].ApplyData(carddata2);

        info[0].Apply(carddata);
        info[1].Apply(carddata2, true);
    }

    public void OnOk()
    {
        OnExit();
    }
}