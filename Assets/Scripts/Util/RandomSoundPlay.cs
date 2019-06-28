 using UnityEngine;
 using System.Collections;
 
 public class RandomSoundPlay : MonoBehaviour {

     public AudioSource randomSound;
     public AudioClip[] audioSources;

	 public bool PlayOnAwake = false;
	 
     void Awake ()
	 {
		randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
		
		if (PlayOnAwake)
			randomSound.Play ();
     }
 
//     void RandomSoundness()
//     {
//        randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];/
//         randomSound.Play ();
//     }

 }