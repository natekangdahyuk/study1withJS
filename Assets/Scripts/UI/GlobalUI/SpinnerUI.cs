using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerUI : baseUI
{
	[SerializeField]
	TweenRotation _rot;

    public override void Init()
    {
		_rot.Play();
	}
}

