using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEvent : MonoBehaviour {

    public string level = "";

    // Use this for initialization
    void Start () {
        Debug.Log(gameObject.name);
        Debug.Log(transform.position);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Trigger Event
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("level : " + level);
        if (collision.CompareTag("Player"))
        {
            //Application.LoadLevel(level) //이건 안됨 그래서  using UnityEngine.SceneManagement; 을 사용
            //SceneManager.LoadScene(level);
            //Destroy(gameObject, 3f);

        }

    }
}
