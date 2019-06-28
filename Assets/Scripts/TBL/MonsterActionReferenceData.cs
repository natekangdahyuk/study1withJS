
public enum ActionType
{
    None = 0,
    Default = 1,
    Special =2,
    barrier =11, //장애물설치 //안씀
    cardDestroy =12, //카드삭제
    trap =13,       // 폭발트랩설치 //안씀
    CardLock = 14, //이동불가
    CardDestroy2 = 15, //카드삭제
}

public class MonsterActionReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int Name;
        
    public int ActionDesc;

    public string ActionIcon;

    public ActionType actionType;

    public int turn; // 턴수

    public int AttValue; // 공격 값

    public string AttEffect; // 공격 이펙트

    public int DebuffTurn; //! 디버프 적용턴수

    public int DebuffNum; //! 디버프 갯수

    public int[] DebuffValue; //! 디버프 적용 범위

    public string AttackEffect;




}