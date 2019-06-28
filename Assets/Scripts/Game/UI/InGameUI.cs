using UnityEngine;
using System.Collections;
using System;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private BossUI bossUI;

    [SerializeField]
    private PlayerUI playerUI;

    [SerializeField]
    private StageInfoUI uiStageInfo;

    [SerializeField]
    private GameOptionUI optionUI;

    [SerializeField]
    private GameStartUI StartUI; 

    private GameEndUI EndUI;

    [SerializeField]
    private CutUI cutUI;

    public Action LoadingComplete;
    public bool IsLoading = false;

    private void Awake()
    {
        IsLoading = false;
        StartCoroutine("Loading");
    }

    public IEnumerator Loading()
    {
        EndUI = GameEndUI.Load<GameEndUI>("UICanvasTop" ,"GameEndUI");
        yield return null;

        IsLoading = true;        
        LoadingComplete();
        GlobalUI.CloseUI(UI_TYPE.InGameLoadingUI);
    }

    public void PlayStart(Action call)
    {
        StartUI.EndAnim = call;
        StartUI.PlayStart();
    }

    public void SetBossInfo( Boss boss )
    {
        bossUI.ApplyInfo(boss);
    }
    public void SetBossMaxHP( int hp )
    {
        bossUI.SetMaxHp( hp );
    }

    public void InitBoss()
    {
        bossUI.TurnEnd();
    }


    public void SetPlayerInfo( Player player )
    {
        playerUI.SetMaxHp(player.Hp);
        optionUI.Apply(player.ItemList);
    }

    public bool IsOptionShow()
    {
        return optionUI.IsOptionShow();
    }
    public void TurnEnd()
    {
        bossUI.TurnEnd();
    }
    public void SetPlayerAttack( int damage , int hp, int maxHP , int count , int comboCount,  Vector3 pos , int turn, bool bCritical, int maxAttackerCount , string Name ,int bit )
    {
        bossUI.ApplyDamage(damage, hp, maxHP , count , comboCount, pos, turn , bCritical, maxAttackerCount);
        cutUI.Play( bCritical , comboCount , Name ,bit );
    }

    public void SetBossAttack(int damage , int hp, int maxHP , Vector3 pos )
    {        
        playerUI.ApplyDamage(damage, hp , maxHP, pos );
    }

    public void SetHeal(int value , int hp , int maxHP )
    {
        playerUI.SetHeal( value , hp , maxHP );
    }

    public void SetStageInfo( StageBase stageBase )
    {
        uiStageInfo.Apply(stageBase);        
    }

    public void UpdateTime( StageBase stageBase )
    {
        uiStageInfo.UpdateTime( stageBase );
    }

    public void GameEnd( bool bClear , int ocard , int oUidx , bool isFull )
    {
        EndUI.GameEnd(bClear , ocard  , oUidx, isFull );
    }

    public void GameEndSpecial( bool bClear = true , bool isFull =true)
    {
        EndUI.GameEndSpecial( bClear, isFull );
    }

    public void OnBack()
    {
        optionUI.OnBack();
    }
}