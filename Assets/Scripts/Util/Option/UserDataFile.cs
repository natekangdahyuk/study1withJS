using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;


class UserDataFile : Singleton<UserDataFile>
{
    public string m_RootPath = Application.persistentDataPath;
    public void Init()
    {
        if (Directory.Exists(m_RootPath) == false)
        {
            Directory.CreateDirectory(m_RootPath);
        }

        LoadUserInfo();
    }


    void LoadUserInfo()
    {
        if (File.Exists(m_RootPath + "/userinfo") == true)
        {
            FileStream fstream = File.Open(m_RootPath + "/userinfo", FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fstream);
            PlayerData.I.UserID = r.ReadString();
            r.Close();
            fstream.Close();

            //PlayerData.I.GoogleLogin = PlayerPrefs.GetInt("GoogleLogin") == 1 ? true : false;
        }
    }

    public void SaveuserInfo()
    {
        FileStream fstream = File.Open(m_RootPath + "/userinfo", FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter w = new BinaryWriter(fstream);
        w.Write(PlayerData.I.UserID);

        w.Close();
        fstream.Close();

        //PlayerPrefs.SetInt("GoogleLogin", PlayerData.I.GoogleLogin == true ? 1 : 0);
    }

    public void SaveLog()
    {
        FileStream fstream = File.Open(m_RootPath + "/loginlog", FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter w = new BinaryWriter(fstream);
        //w.Write(GooglePlayGameManager.I.GetUserID());
        w.Close();
        fstream.Close();
    }

    public void ClearUserInfo()
    {
        File.Delete(m_RootPath + "/userinfo");
    }
}

