﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class UnityAdsManager_Rewarded : MonoBehaviour
{
	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
            // 광고가 끝난 뒤 콜백함수 "HandleShowResult" 호출
            var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

    // 광고가 종료된 후 자동으로 호출되는 콜백 함수
	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
            // 광고를 성공적으로 시청한 경우 보상 지급
			Debug.Log ("The ad was successfully shown.");
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}
