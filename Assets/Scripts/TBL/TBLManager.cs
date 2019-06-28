using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum TABLELIST_TYPE
{
    Character = 0,
    Stage,
    Monster,
    MonsterDetail,
    Card,
    Skin,
    DefaultCard,
    DefaultData,
    Thema,
    text,
    Stat,
    ExpUser,
    ExpChar,
    GradeData,
    limitbreak,
    SummonGroup,
    Shop,
    MonsterAction,
    Combo,
    Collection,
    Item,
    AttributeMode,
    RankModeStage,
    RankModeReward,
    ModeSet,
    MissionDaily,
    SpriteAtlas,
    BadWord,
    Packeage,
    LoadingString,
    Achievement,
    MAX,
}

public class TBLManager : Singleton<TBLManager>
{
    ITBL[] m_TableList = new ITBL[(int)TABLELIST_TYPE.MAX];

    public void Init()
    {
        Debug.Log("## table manager initliaze");
        for (TABLELIST_TYPE i = 0; i < TABLELIST_TYPE.MAX; i++)
        {
            CreateTable(i);
        }
    }

    void CreateTable(TABLELIST_TYPE eType)
    {
        int idx = (int)eType;

        switch (eType)
        {
            case TABLELIST_TYPE.Character:
                {
                    m_TableList[(int)TABLELIST_TYPE.Character] = new CharacterTBL();
                }
                break;

            case TABLELIST_TYPE.Stage:
                {
                    m_TableList[(int)TABLELIST_TYPE.Stage] = new StageTBL();
                }
                break;

            case TABLELIST_TYPE.Monster:
                {
                    m_TableList[(int)TABLELIST_TYPE.Monster] = new MonsterTBL();
                }
                break;

            case TABLELIST_TYPE.MonsterDetail:
                {
                    m_TableList[(int)TABLELIST_TYPE.MonsterDetail] = new MonsterDetailTBL();
                }
                break;

            case TABLELIST_TYPE.Card:
                {
                    m_TableList[(int)TABLELIST_TYPE.Card] = new CardTBL();
                }
                break;


            case TABLELIST_TYPE.Skin:
                {
                    m_TableList[(int)TABLELIST_TYPE.Skin] = new SkinTBL();
                }
                break;

            case TABLELIST_TYPE.DefaultCard:
                {
                    m_TableList[(int)TABLELIST_TYPE.DefaultCard] = new DefaultCardTBL();
                }
                break;

            case TABLELIST_TYPE.DefaultData:
                {
                    m_TableList[(int)TABLELIST_TYPE.DefaultData] = new DefaultDataTBL();
                }
                break;

            case TABLELIST_TYPE.Thema:
                {
                    m_TableList[(int)TABLELIST_TYPE.Thema] = new ThemaTBL();
                }
                break;

            case TABLELIST_TYPE.text:
                {
                    m_TableList[(int)TABLELIST_TYPE.text] = new StringTBL();
                }
                break;

            case TABLELIST_TYPE.Stat:
                {
                    m_TableList[(int)TABLELIST_TYPE.Stat] = new StatTBL();
                }
                break;
            case TABLELIST_TYPE.ExpUser:
                {
                    m_TableList[(int)TABLELIST_TYPE.ExpUser] = new ExpUserTBL();
                }
                break;
            case TABLELIST_TYPE.ExpChar:
                {
                    m_TableList[(int)TABLELIST_TYPE.ExpChar] = new ExpCharTBL();
                }
                break;
            case TABLELIST_TYPE.GradeData:
                {
                    m_TableList[(int)TABLELIST_TYPE.GradeData] = new GradeDataTBL();
                }
                break;
            case TABLELIST_TYPE.limitbreak:
                {
                    m_TableList[(int)TABLELIST_TYPE.limitbreak] = new LimitbreakTBL();
                }
                break;

            case TABLELIST_TYPE.SummonGroup:
                {
                    m_TableList[(int)TABLELIST_TYPE.SummonGroup] = new Summon_groupTBL();
                }
                break;

            case TABLELIST_TYPE.Shop:
                {
                    m_TableList[(int)TABLELIST_TYPE.Shop] = new ShopTBL();
                }
                break;

            case TABLELIST_TYPE.MonsterAction:
                {
                    m_TableList[(int)TABLELIST_TYPE.MonsterAction] = new MonsterActionTBL();
                }
                break;

            case TABLELIST_TYPE.Combo:
                {
                    m_TableList[(int)TABLELIST_TYPE.Combo] = new ComboTBL();
                }
                break;

            case TABLELIST_TYPE.Collection:
                {
                    m_TableList[(int)TABLELIST_TYPE.Collection] = new CollectionTBL();
                }
                break;

            case TABLELIST_TYPE.Item:
                {
                    m_TableList[(int)TABLELIST_TYPE.Item] = new ItemTBL();
                }
                break;

            case TABLELIST_TYPE.AttributeMode:
                {
                    m_TableList[(int)TABLELIST_TYPE.AttributeMode] = new AttributeModeTBL();
                }
                break;
            case TABLELIST_TYPE.RankModeStage:
                {
                    m_TableList[(int)TABLELIST_TYPE.RankModeStage] = new RankModeTBL();
                }
                break;
            case TABLELIST_TYPE.RankModeReward:
                {
                    m_TableList[(int)TABLELIST_TYPE.RankModeReward] = new RankModeRewardTBL();
                }
                break;
            case TABLELIST_TYPE.ModeSet:
                {
                    m_TableList[(int)TABLELIST_TYPE.ModeSet] = new ItemTBL();
                }
                break;

            case TABLELIST_TYPE.MissionDaily:
                {
                    m_TableList[(int)TABLELIST_TYPE.MissionDaily] = new MissionTBL();
                }
                break;
            case TABLELIST_TYPE.SpriteAtlas:
                {
                    m_TableList[(int)TABLELIST_TYPE.SpriteAtlas] = new SpriteAtlasTBL();
                }
                break;

            case TABLELIST_TYPE.BadWord:
                {
                    m_TableList[(int)TABLELIST_TYPE.BadWord] = new badWrodTBL();
                }
                break;

            case TABLELIST_TYPE.Packeage:
                {
                    m_TableList[(int)TABLELIST_TYPE.Packeage] = new PackageTBL();
                }
                break;

            case TABLELIST_TYPE.LoadingString:
                {
                    m_TableList[(int)TABLELIST_TYPE.LoadingString] = new LoadingStringTBL();
                }
                break;

            case TABLELIST_TYPE.Achievement:
                {
                    m_TableList[(int)TABLELIST_TYPE.Achievement] = new AchievementTBL();
                }
                break;

            default:
                {
                    Debug.LogError("can`t reward data " + eType);
                }
                break;
        }

        m_TableList[(int)eType].LoadData();
    }

    public T GetTable<T>(TABLELIST_TYPE eType) where T : ITBL
    {
        if (m_TableList[(int)eType] == null)
        {
            CreateTable(eType);
        }

        return (T)m_TableList[(int)eType];
    }
}

