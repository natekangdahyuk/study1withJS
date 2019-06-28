using UnityEngine;
using System;
using UnityEngine.UI;


class DevLoginPopup : baseUI
{
    [SerializeField]
    InputField input;

	Action cb = null;

	public override void Init()
    {
		input.text = PlayerPrefs.GetString("id", SystemInfo.deviceUniqueIdentifier);
	}

	public void Set(Action closeCB)
	{
		cb = closeCB;		
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

		PlayerPrefs.SetString("id", input.text);

		if(cb != null)
		{
			cb();
		}

		OnExit();
    }    	
}
