using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if(renderer.isVisible){
			collider.enabled=true;
		}
		else{
			collider.enabled=false;
		}
	}
}
