using UnityEngine;
using System;
using UnityEngine.UI;


class CreateAccountPopup : baseUI
{
    [SerializeField]
    InputField input;

    public override void Init()
    {       
        
    }

    public void OnOk()
    {
        if( input.text.Length < 2 )
        {
            return;
        }
        

        if( badWrodTBL.CheckWord(input.text) == false )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902121 ));
            return;
        }
        PlayerData.I.UserID = input.text;
        NetManager.CharacterCreate( PlayerData.I.UserID );
    }

    public void OnSubmit()
    {
        //PlayerInfo.I.UserNickName = input_NickName.value;
    }
    public void ReceiveAccount(bool bSuccess = true)
    {
        if (bSuccess)
        {
            UserDataFile.I.SaveuserInfo();
            OnExit();           
        }
        else
        {
            //if (PlayerInfo.I.GoogleLogin)
            //{
            //    UIManager.I.ShowOKPupUp(" ", "The ID that already exists");
            //}
            //else
            //{
            //    int ran = NGUITools.RandomRange(1, 1000000000);
            //    PlayerInfo.I.UserID = ran.ToString();
            //    PlayerInfo.I.UserPassword = ran.ToString();
            //    NetManager.CreateAccount(PlayerInfo.I.UserID, PlayerInfo.I.UserPassword, PlayerInfo.I.UserNickName);
            //}
        }
    }
}
