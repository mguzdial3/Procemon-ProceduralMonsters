using UnityEngine;
using System.Collections;

public class RestartOnTouch : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
