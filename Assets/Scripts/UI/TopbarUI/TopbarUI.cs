using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class TopbarUI : baseUI
{
    public baseUI CurrentScene;

    [SerializeField]
    GameObject BtnGroup;

    [SerializeField]
    GameObject DefaultGroup;

    [SerializeField]
    GameObject SubSceneGroup;

    [SerializeField]
    GameObject Stone;

    [SerializeField]
    GameObject Shoes;

    [SerializeField]
    Text GoldText;

    [SerializeField]
    Text CashText;

    [SerializeField]
    Text ShoesText;


    [SerializeField]
    Text GoldSubGroupText;

    [SerializeField]
    Text CashSubGroupText;

    [SerializeField]
    Text ShoesSubGroupText;

    [SerializeField]
    Text StoneSubGroupText;

    [SerializeField]
    Text CharaterNameText;

    [SerializeField]
    Text CharaterLevelText;

    MainSceneUI mainscene;
    void Awake()
    {
        OnExit();
        Apply();
    }
    public override void Init()
    {

    }

    public void Set(MainSceneUI main)
    {
        mainscene = main;
    }
    public void Apply()
    {
        GoldText.text = PlayerData.I.Gold.ToString("n0");
        CashText.text = PlayerData.I.Cash.ToString("n0");
        ShoesText.text = PlayerData.I.shoes.ToString("n0") + "/" + PlayerData.I.Maxshoes.ToString("n0");
        GoldSubGroupText.text = PlayerData.I.Gold.ToString("n0");
        CashSubGroupText.text = PlayerData.I.Cash.ToString("n0");
        ShoesSubGroupText.text = PlayerData.I.shoes.ToString("n0") + "/" + PlayerData.I.Maxshoes.ToString("n0");
        StoneSubGroupText.text = PlayerData.I.Stone.ToString("n0");
        CharaterNameText.text = PlayerData.I.UserID;
        CharaterLevelText.text = PlayerData.I.UserLevel.ToString();
    }

    public void ShowStone(bool bShow)
    {
        Stone.SetActive(bShow);
        Shoes.SetActive(!bShow);
    }
    public void ChangeScene(baseUI baseui)
    {

        Stone.SetActive(baseui.eType == UI_TYPE.SummonUI);
        Shoes.SetActive(!(baseui.eType == UI_TYPE.SummonUI));


        if (baseui.eSubType == UI_SUBTYPE.SUBSCENE)
        {
            if (mainscene != null)
                mainscene.OnExit();

            SubSceneGroup.SetActive(true);
            DefaultGroup.SetActive(false);
            CurrentScene = baseui;
        }
        else
        {
            if (mainscene != null)
                mainscene.Apply();

            DefaultGroup.SetActive(true);
            SubSceneGroup.SetActive(false);
            CurrentScene = null;
        }
    }
    public void OnHome(bool bClick = true)
    {

        if (CurrentScene != null)
        {
            if (CurrentScene.OnExit() == false)
                return;
        }

        DefaultGroup.SetActive(true);
        SubSceneGroup.SetActive(false);
        GlobalUI.I.ClearBackList();
        mainscene.OnEnter();
        if (bClick)
            SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_top_main", GameOption.EffectVoluem);
    }

    public void OnOption()
    {
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_top_option", GameOption.EffectVoluem);

        OptionPopup option = GlobalUI.GetUI<OptionPopup>(UI_TYPE.OptionPopup);
        option.Apply();


    }

    public static bool CheckCost(int cost)
    {
        if (PlayerData.I.Cash < cost)
        {
            TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
            GlobalUI.ShowOKCancelPupUp(StringTBL.GetData(800014), topbar.OnShopRuby);
            return false;
        }
        return true;
    }

    public static bool CheckRuby(int cost)
    {
        if (PlayerData.I.Cash < cost)
        {
            TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
            GlobalUI.ShowOKPupUp("루비가 부족합니다.");
            return false;
        }
        return true;
    }

    public void OnShopRuby()
    {
        Onshop(ShopPopup.SHOPTYPE.ruby);
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void OnShopGold()
    {
        Onshop(ShopPopup.SHOPTYPE.gold);
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void OnShopAP()
    {
        Onshop(ShopPopup.SHOPTYPE.ap);
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void OnShopStone()
    {
        Onshop(ShopPopup.SHOPTYPE.stone);
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void Onshop(ShopPopup.SHOPTYPE type = ShopPopup.SHOPTYPE.ruby)
    {

        ShopPopup shop = GlobalUI.GetUI<ShopPopup>(UI_TYPE.ShopPopup);

        if (CurrentScene != shop)
        {
            if (CurrentScene != null)
            {
                GlobalUI.I.AddSubSceneBack(CurrentScene);
                CurrentScene.OnExit();
            }

            shop.Apply(type);
        }
        //SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_topbar" , GameOption.EffectVoluem );
    }

    public void OnBack()
    {
        if (CurrentScene == null)
            return;


        if (GlobalUI.I.SubSceneBackList.Count == 0)
        {
            OnHome(false);
        }
        else
        {
            if (CurrentScene.OnExit() == false)
                return;

            baseUI baseui = GlobalUI.I.GetBackSubScene();
            CurrentScene = baseui;
            baseui.OnEnter();
        }
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_top_back", GameOption.EffectVoluem);
    }


}
