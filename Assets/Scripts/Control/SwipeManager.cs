using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
	public enum Swipe { None, UpRight, DownRight,DownLeft,  UpLeft  };

	[RequireComponent(typeof(AudioSource))]
	public class SwipeManager : MonoBehaviour
	{
		public float minSwipeLength = 15f;
		Vector2 firstPressPos;
		Vector2 secondPressPos;
		Vector2 currentSwipe;

		public static Swipe swipeDirection;

		/// <summary>
		/// The snd move row.
		/// </summary>
		[SerializeField]
		private AudioClip sndMoveH;

		/// <summary>
		/// The snd move col.
		/// </summary>
		[SerializeField]
		private AudioClip sndMoveV;

		private AudioSource mAudioSource;
		private bool isSndPlay = false;

		/// <summary>
		/// 각도 조절용. ex) 90 - 15 = 75
		/// </summary>
		private float m_DirDegree = 0.1f;

		private void Start()
		{
			mAudioSource = GetComponent<AudioSource> ();
		}

		private void Update()
		{
			DetectSwipe();
		}

		public void DetectSwipe()
		{
			if( Input.GetMouseButtonDown( 0 ) )
			{
				firstPressPos = new Vector2( Input.mousePosition.x, Input.mousePosition.y );
			}

			if( Input.GetMouseButton( 0 ) )
			{
				secondPressPos = new Vector2( Input.mousePosition.x, Input.mousePosition.y );
				currentSwipe = new Vector3( secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y );

				if( currentSwipe.magnitude < minSwipeLength )
				{
					swipeDirection = Swipe.None;
					return;
				}

				currentSwipe.Normalize();

				if( currentSwipe.y > m_DirDegree && currentSwipe.x > m_DirDegree )
				{
					swipeDirection = Swipe.UpRight;
				}
				else if( currentSwipe.y < -m_DirDegree && currentSwipe.x < -m_DirDegree )
				{
					swipeDirection = Swipe.DownLeft;
				}
				else if( currentSwipe.x < -m_DirDegree && currentSwipe.y > m_DirDegree )
				{
					swipeDirection = Swipe.UpLeft;
				}
				else if( currentSwipe.x > m_DirDegree && currentSwipe.y < -m_DirDegree )
				{
					swipeDirection = Swipe.DownRight;
				}
			}
			else
			{
				swipeDirection = Swipe.None;
				isSndPlay = false;
			}

			//!< Sound
			if( false == isSndPlay )
			 	StartCoroutine( PlaySound(swipeDirection));
		}

	
		public IEnumerator PlaySound(Swipe direction)
		{
			if (isSndPlay)
				yield break;

			switch (direction) {
			case Swipe.DownLeft:
			case Swipe.UpRight:
				mAudioSource.clip = sndMoveV;
				break;

			case Swipe.DownRight:
			case Swipe.UpLeft:
				mAudioSource.clip = sndMoveH;
				break;

			default:
				yield break;
			}

			mAudioSource.Play ();
			isSndPlay = true;
			yield return new WaitForSeconds (mAudioSource.clip.length);

			mAudioSource.clip = null;
		}
	}
}