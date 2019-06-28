using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEffect : MonoBehaviour {

    public GameObject obj;

    public GameObject target;
    public bool SetTargetPosition = false;
    void Awake ()
    {
		target = Instantiate<GameObject>(obj, new Vector3 (0, 0, 0), Quaternion.identity);
        target.SetActive(false);

    }

    public void Play()
    {
        target.SetActive(true);
    }
}
