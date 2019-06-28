using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIUtil
{
    public static void LoadProperty(RawImage image, PROPERTY property)
    {
        switch (property)
        {
            case PROPERTY.FIRE:
                image.texture = ResourceManager.LoadTexture("icon_element_fire");
                break;

            case PROPERTY.WATER:
                image.texture = ResourceManager.LoadTexture("icon_element_water");
                break;

            case PROPERTY.WIND:
                image.texture = ResourceManager.LoadTexture("icon_element_wind");
                break;

            case PROPERTY.WHITE:
                image.texture = ResourceManager.LoadTexture("icon_element_light");
                break;

            case PROPERTY.BLACK:
                image.texture = ResourceManager.LoadTexture("icon_element_dark");
                break;
        }
    }

    public static void LoadBit( RawImage image , int bit )
    {
        string str = "tex_card_number_" + bit.ToString();
        image.texture = ResourceManager.LoadTexture( str );
    }


    public static string PropertyString(PROPERTY property)
    {
        switch (property)
        {
            case PROPERTY.FIRE:
                return "불속성";
            case PROPERTY.WATER:
                return "물속성";

            case PROPERTY.WIND:
                return "풍속성";

            case PROPERTY.WHITE:
                return "빛속성";

            case PROPERTY.BLACK:
                return "암속성";
        }

        return "";
    }
    public static void LoadStarEx( RawImage image , int star )
    {
        image.gameObject.SetActive( true );
        image.texture = ResourceManager.LoadTexture("icon_star_" + star);
        
    }


    public static void LoadStar(RawImage image, int star)
    {
        image.gameObject.SetActive( true );
        //image.texture = ResourceManager.LoadTexture("icon_star_" + star);
        image.texture = ResourceManager.LoadTexture( "tex_card_star_" + star );
    }

    public static void LoadClass(RawImage image, CLASS Class)
    {
        image.gameObject.SetActive( true );
        switch (Class)
        {
            case CLASS.ATTACK:
                image.texture = ResourceManager.LoadTexture("icon_class_attack");
                break;
            case CLASS.DEFENCE:
                image.texture = ResourceManager.LoadTexture("icon_class_defense");
                break;
            case CLASS.HEAL:
                image.texture = ResourceManager.LoadTexture("icon_class_heal");
                break;
        }
    }

    public static void LoadCurrencyType( RawImage image , MissionReferenceData.RewardType type )
    {
        switch( type )
        {
            case MissionReferenceData.RewardType.AP:
            image.texture = ResourceManager.LoadTexture( "icon_topbar_power" );
            break;
            case MissionReferenceData.RewardType.Gold:
            image.texture = ResourceManager.LoadTexture( "icon_topbar_gold" );
            break;
            case MissionReferenceData.RewardType.Ruby:
            image.texture = ResourceManager.LoadTexture( "icon_topbar_ruby" );
            break;
            case MissionReferenceData.RewardType.Stone:
            image.texture = ResourceManager.LoadTexture( "icon_topbar_summonstone" );
            break;
        }
    }


    public static string ClassString(CLASS Class)
    {
        switch (Class)
        {
            case CLASS.ATTACK:
                return "공격형";                
            case CLASS.DEFENCE:
                return "방어형";
                
            case CLASS.HEAL:
                return "회복형";                
        }

        return "";
    }

    public static string LeaderBuffString( LeaderBuff type , int value , PROPERTY property , CLASS Class )
    {
        switch( type )
        {
            case LeaderBuff.AllAttack:
            return "전체 공격력 " + value.ToString() + "% 상승";

            case LeaderBuff.AttributeAttack:
            return PropertyString(property) + " 공격력 " + value.ToString() + "% 상승";

            case LeaderBuff.ClassAttack:
            return ClassString(Class) + " 공격력 " + value.ToString() + "% 상승";

            case LeaderBuff.AllHp:
            return "전체 체력 " + value.ToString() + "% 상승";

            case LeaderBuff.AttributeHp:
            return PropertyString( property ) +" 체력 "+ value.ToString() + "% 상승";

            case LeaderBuff.ClassHp:
            return "직업 체력 " + value.ToString() + "% 상승";

            case LeaderBuff.ComboUp:
            return "콤보 데미지 " + value.ToString() + "% 상승";
        }

        return "";
    }

    public static string GetTimeEx(int Time)
    {
        int sec = Time % 60;
        int min = Time / 60;
        int hour = 0;

        if (min >= 60)
        {
            hour = min / 60;
            min = min % 60;
        }

        return string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
    }


    public static string GetTimeEx2( float fTime )
    {
        fTime /= 100f;
        int time = (int)fTime;

        int sec = time % 60;
        int min = time / 60;
        int hour = 0;

        if( min >= 60 )
        {
            hour = min / 60;
            min = min % 60;
        }

        float value = fTime - time;

        if( hour <= 0)
            return string.Format( "{0:00}:{1:00}.{2:00}" , min , sec , (int)( value * 100 ) );

        return string.Format( "{0:00}:{1:00}:{2:00}.{3:00}" , hour , min , sec , (int)( value * 100 ) );
    }


    public static string GetTime( int value )
    {                
        DateTime reward = DateTime.Today;
        DateTime current2 = DateTime.Now;

        //if( (int)reward.DayOfWeek == 0 )
        //    reward = reward.AddDays( 1 );
        //else
       
       
        int day = value - (int)reward.DayOfWeek+1;

        if( day <= 0 )
            day = 7 + day;
        reward = reward.AddDays( day );


        DateTime te = new DateTime( reward.Ticks - current2.Ticks  );
        
        if( te.Day - 1 == 0 )
            return string.Format( "{1:00}시간 {2:00}분" , te.Day - 1 , te.Hour , te.Minute );
        else
            return string.Format( "{0}일 {1:00}시간 {2:00}분" , te.Day-1 , te.Hour , te.Minute );
    }


    //public static int GetTimeValue(int value)
    //{
    //    DateTime reward = DateTime.Today;
    //    DateTime current2 = DateTime.Now;

    //    //if( (int)reward.DayOfWeek == 0 )
    //    //    reward = reward.AddDays( 1 );
    //    //else


    //    int day = value - (int)reward.DayOfWeek;

    //    if (day <= 0)
    //        day = 7 + day;
    //    reward = reward.AddDays(day+1);


    //    DateTime te = new DateTime(reward.Ticks - current2.Ticks);
    //    return te.Day;

     
    //}


    //public static int CalcLevel( int AddExp )
    //{
    //    int CurrentExp = PlayerData.I.Exp + AddExp;
    //    ExpUserReferenceData Data = ExpUserTBL.GetDataByExp(PlayerData.I.Exp);

    //    ExpUserReferenceData Data2 = ExpUserTBL.GetData(CurrentExp);

    //    ExpUserReferenceData OldData = ExpUserTBL.GetData(Data2.ReferenceID-1);


    //    int OldExp = 0;

    //    if (OldData != null)
    //        OldExp = OldData.exp;


    //    int per = (int)(((float)(CurrentExp - OldExp) / Data2.exp - OldExp) * 100);

    //}
}