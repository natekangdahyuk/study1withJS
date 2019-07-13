using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginCheck : MonoBehaviour
{
    public InputField id_field;
    public InputField pw_field;

    string loginID;
    string loginPW;

    // Start is called before the first frame update
    void Start()
    {
        loginID = PlayerPrefs.GetString("loginID");
        loginPW = PlayerPrefs.GetString("loginPW");

        if (loginID == "aaa" && loginPW == "1234")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void ChangeScene()
    {
        loginID = id_field.GetComponent<InputField>().text;
        loginPW = pw_field.GetComponent<InputField>().text;

        if (loginID == "aaa" && loginPW == "1234")
        {
            PlayerPrefs.SetString("loginID", loginID);
            PlayerPrefs.SetString("loginPW", loginPW);

            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            //
            Debug.Log("아이디 또는 패스워드가 잘못되었습니다.");
            //팝업 활성화
        }
    }


}
