using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LeaderSettingItem : MonoBehaviour
{

    public GameObject SelectLeader;
    public GameObject SelectSubLeader;

    public GameObject OnSelectLeader;

    public void SetLeaderMode()
    {
        gameObject.SetActive( true );
        SelectLeader.SetActive( true );
        SelectSubLeader.SetActive( false );
        OnSelectLeader.SetActive( false );
    }

    public void SetSubLeaderMode()
    {
        SelectLeader.SetActive( false );
        SelectSubLeader.SetActive( true );
    }

    public void SetSelectLeader()
    {
        OnSelectLeader.SetActive( true );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
    }
}
