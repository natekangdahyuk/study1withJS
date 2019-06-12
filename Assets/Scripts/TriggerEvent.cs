using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEvent : MonoBehaviour {

    public string level = "";
    int i = 0;

    // Use this for initialization
    void Start () {
        Debug.Log(gameObject.name);
        Debug.Log(transform.position);
        Debug.Log(gameObject.tag);

        Debug.Log("objNum: " +objNum);



    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space!!");

            if (gameObject.tag == "mojoBlack")
            {
                

            }
            
        }
		
	}

    static int objNum = 0;

    
    //Trigger Event
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("level : " + level);
        if (collision.CompareTag("Player"))
        {
            //Application.LoadLevel(level) //이건 안됨 그래서  using UnityEngine.SceneManagement; 을 사용
            //SceneManager.LoadScene(level);
            //Destroy(gameObject, 3f);
            //transform.Translate(0f, 1f, 0);\
            //transform.Translate(Vector3.up);
            Debug.Log("충돌");

            if (objNum == 0)
            {
                transform.Rotate(0f, 0f, 45f); // 45도 돌리고
                Instantiate(gameObject); //복제하고 
                transform.Translate(0f, 1f, 0); //현재 게임 오브젝트는 이동하라....
                objNum++;

                GetComponent<SpriteRenderer>().color = Color.red;

                Destroy(gameObject, 3f);

            }

        }

    }
}
