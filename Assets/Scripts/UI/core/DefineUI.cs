using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum UI_TYPE : int //추가되는거 무조건 젤 밑으로 넣어야됨
{
    NONE = -1,
    InGameLoadingUI,
    LoadingUI,
    MainStageUI,
    InventoryUI,
    PlayerInfoUI,
    TopBarUI,
    CreateAccountUI,
    MainStageReadyUI,
    PopupOk,
    PopupOkCancel,
    LevelUpCompletePopup,
    LimitCompletePopup,
    PromotionCompletePopup,
    SummonUI,
    SummonCompletePopup,
    ShopPopup,
    MainStageDetailStarPopup,
    CardRewardInfoPopup,
    CardRewardPopup,
    UserLevelUpPopup,
    illustUI,
    RankModeMainUI,
    RankingUI,
    RankingModeReadyUI,
    MissionUI,
    OptionPopup,
    ErrorOK,
    SpinnerUI,
    DevLoginUI,
    RankRewardPopup,
    CouponPopup,
    ShopPackagePopup,
    CouponRewardPopup,
    LoadingUIEx,
    StoryMainUI,
    TutorialUI,
    MonsterInfoPopup,
    MailUI,
    NoticePopup,
}


public enum UI_SUBTYPE : int
{
    POPUP = 0,
    DEFAULT,
    SUBSCENE,
}
