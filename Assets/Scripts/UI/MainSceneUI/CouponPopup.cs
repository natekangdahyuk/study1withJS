using UnityEngine;
using System;
using UnityEngine.UI;

public class CouponPopup : baseUI
{
    [SerializeField]
    InputField input;
    public override void Init()
    {

    }

    private void Awake()
    {
      
    }
    // Use this for initialization
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnOk()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( input.text.Length > 5)
        {
            NetManager.UseCoupon( input.text );
        }
        
        OnExit();
    }
}