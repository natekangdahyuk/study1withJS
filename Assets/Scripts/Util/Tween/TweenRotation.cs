using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TweenRotation : TweenBase
{
	[SerializeField]
	float _start;
	[SerializeField]
	float _end;
	[SerializeField]
	bool _infinity;
	

	Quaternion _rot;

	private void Awake()
	{
		_rot = transform.rotation;
	}

	public void Play(float start, float end)
	{
		_start = start;
		_end = end;

		transform.rotation = Quaternion.AngleAxis(_start, new Vector3(0f, 0f, 1f));

		Play();
	}

	protected override bool Tween()
	{
		if (base.Tween() == false)
		{
			if(_infinity == true)
			{
				Play();
			}
			else
			{
				return false;
			}			
		}

		var cur_rot = Mathf.Lerp(_start, _end, CurrentTime / PlayTime);
		transform.rotation = Quaternion.AngleAxis(cur_rot, new Vector3(0f, 0f, 1f));
		return true;
	}

	public override void Reset()
	{
		transform.rotation = Quaternion.AngleAxis(_start, new Vector3(0f, 0f, 1f));
		base.Reset();		
	}
}