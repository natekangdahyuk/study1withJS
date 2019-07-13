using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSaveLoad : MonoBehaviour
{
    public Text txt_myScore;

    int myScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        myScore = PlayerPrefs.GetInt("myScore");

        Debug.Log("myScore Log : " + myScore);

        txt_myScore.GetComponent<Text>().text = "Score : " + myScore;        
        //txt_myScore.GetComponent<Text>().text = "xxxx : ";
    }

    public void btn_SaveScore()
    {
        myScore = myScore + 10;
        PlayerPrefs.SetInt("myScore", myScore);
        txt_myScore.GetComponent<Text>().text = "Score : " + myScore;
    }
}
