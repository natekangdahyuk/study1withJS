using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public Text txt_myScore;

    public AudioSource backSound;
    public AudioSource btnSound;

    int myScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        myScore = PlayerPrefs.GetInt("myScore");

        Debug.Log("myScore Log : " + myScore);

        txt_myScore.GetComponent<Text>().text = "Score : " + myScore;


        string backSoundState = PlayerPrefs.GetString("backSoundState");
        if (backSoundState == "off")
        {
            backSound.GetComponent<AudioSource>().Stop();            
        }

        string btnSoundState = PlayerPrefs.GetString("btnSoundState");
        if (btnSoundState == "off") 
        {
            btnSound.GetComponent<AudioSource>().volume = 0;            
        }        
    }

    public void btn_SaveScore()
    {
        myScore = myScore + 10;
        PlayerPrefs.SetInt("myScore", myScore);
        txt_myScore.GetComponent<Text>().text = "Score : " + myScore;
    }

    public void btn_BackSoundControl()
    {
        string backSoundState = PlayerPrefs.GetString("backSoundState");
        

        if (backSoundState != "off") 
        {
            backSound.GetComponent<AudioSource>().Stop();
            PlayerPrefs.SetString("backSoundState", "off");            
        }
        else
        {
            backSound.GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("backSoundState", "on");            
        }              
    }

    public void btn_SaveBtnSoundControl()
    {
        string btnSoundState = PlayerPrefs.GetString("btnSoundState");
        
        if (btnSoundState != "off") // on 인 상태면 off 볼륨 0 으로, on 볼륨 1로
        {        
            btnSound.GetComponent<AudioSource>().volume = 0;
            PlayerPrefs.SetString("btnSoundState", "off");
            
        }
        else //off 인 상태라면
        {  
            btnSound.GetComponent<AudioSource>().volume = 1;
            PlayerPrefs.SetString("btnSoundState", "on");
            
        }
        
    }
}
