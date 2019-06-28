using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ErrorOk : baseUI
{
	public Text _desc;	

	public override void Init()
	{
	}

	public void Set(string sDesc)
	{		
		_desc.text = sDesc;		
	}	

	public void OnClose()
	{		
		OnExit();
	}

}