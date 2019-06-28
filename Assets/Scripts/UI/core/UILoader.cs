using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UILoader
{
    public baseUI Load(UI_TYPE eType)
    {
        switch (eType)
        {
            case UI_TYPE.InGameLoadingUI:
                return ResourceManager.Load<LoadingUI>(GameObject.Find("GlobalUI"), "InGameLoadingUI");
            case UI_TYPE.LoadingUI:
                return ResourceManager.Load<LoadingUI>(GameObject.Find("GlobalUI"), "LoadingUI");

            case UI_TYPE.LoadingUIEx:
                return ResourceManager.Load<LoadingUI>(GameObject.Find("GlobalUI"), "LoadingUIEx");

            case UI_TYPE.TopBarUI:
                {
                    TopbarUI topbar = ResourceManager.Load<TopbarUI>(GameObject.Find("TopbarCanvas"), "TopBarUI");

                    if (topbar == null)
                        return null;

                    topbar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    topbar.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                    return topbar;
                }

            case UI_TYPE.CreateAccountUI:
                return ResourceManager.Load<CreateAccountPopup>(GameObject.Find("GlobalUI"), "CreateAccountPopup");

            case UI_TYPE.PlayerInfoUI:
                return ResourceManager.Load<PlayerInfoUI>(GameObject.Find("SubCanvas"), "PlayerInfoUI");

            case UI_TYPE.InventoryUI:
                return ResourceManager.Load<InventoryUI>(GameObject.Find("SubCanvas"), "InventoryUI");

            case UI_TYPE.PopupOk:
                return ResourceManager.Load<PopupOk>(GameObject.Find("GlobalUI"), "PopupOk");

            case UI_TYPE.PopupOkCancel:
                return ResourceManager.Load<PopupOkCancel>(GameObject.Find("GlobalUI"), "PopupOkCancel");

            case UI_TYPE.ShopPopup:
                return ResourceManager.Load<ShopPopup>(GameObject.Find("SubCanvas"), "ShopPopup");

            case UI_TYPE.SummonUI:
                return ResourceManager.Load<SummonUI>(GameObject.Find("SubCanvas"), "SummonUI");

            case UI_TYPE.SummonCompletePopup:
                return ResourceManager.Load<SummonCompletePopup>(GameObject.Find("TopbarCanvas"), "SummonCompletePopup");

            case UI_TYPE.CardRewardPopup:
                return ResourceManager.Load<CardRewardPopup>(GameObject.Find("UICanvasTop"), "CardRewardPopup");

            case UI_TYPE.UserLevelUpPopup:
                return ResourceManager.Load<UserLevelUpPopup>(GameObject.Find("GlobalUI"), "UserLevelUpPopup");

            case UI_TYPE.illustUI:
                return ResourceManager.Load<illustUI>(GameObject.Find("SubCanvas"), "illustUI");

            case UI_TYPE.MissionUI:
                return ResourceManager.Load<MissionUI>(GameObject.Find("SubCanvas"), "MissionUI");

            case UI_TYPE.OptionPopup:
                return ResourceManager.Load<OptionPopup>(GameObject.Find("TopbarCanvas"), "OptionPopup");
            case UI_TYPE.ErrorOK:
                return ResourceManager.Load<ErrorOk>(GameObject.Find("GlobalUI"), "ErrorOk");
            case UI_TYPE.SpinnerUI:
                return ResourceManager.Load<SpinnerUI>(GameObject.Find("GlobalUI"), "SpinnerUI");
            case UI_TYPE.DevLoginUI:
                return ResourceManager.Load<DevLoginPopup>(GameObject.Find("DevLoginPopup"), "DevLoginPopup");

            case UI_TYPE.RankRewardPopup:
                return ResourceManager.Load<RankRewardPopup>(GameObject.Find("TopbarCanvas"), "RankRewardPopup");

            case UI_TYPE.CouponPopup:
                return ResourceManager.Load<CouponPopup>(GameObject.Find("TopbarCanvas"), "CouponPopup");

            case UI_TYPE.ShopPackagePopup:
                return ResourceManager.Load<ShopPackagePopup>(GameObject.Find("SubCanvas"), "ShopPackagePopup");

            case UI_TYPE.CouponRewardPopup:
                return ResourceManager.Load<CouponRewardPopup>(GameObject.Find("TopbarCanvas"), "CouponRewardPopup");

            case UI_TYPE.StoryMainUI:
                return ResourceManager.Load<StoryMainUI>(GameObject.Find("TopbarCanvas"), "StoryMainUI");

            case UI_TYPE.TutorialUI:
                return ResourceManager.Load<TutorialUI>(GameObject.Find("TopbarCanvas"), "GuideUI");

            case UI_TYPE.MailUI:
                return ResourceManager.Load<MailUI>(GameObject.Find("SubCanvas"), "MailUI");

            case UI_TYPE.NoticePopup:
                return ResourceManager.Load<NoticePopup>(GameObject.Find("SubCanvas"), "NoticePopup");

        }

        return null;

    }
}
