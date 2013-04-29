using UnityEngine;
using System.Collections;

public class GodModeSetter : MonoBehaviour {
	public PlayerMovement movement;
	
	
	void Start(){
		movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
	}
	
	
	void OnGUI(){
		
		if(movement.inMvmtMode){
			bool toggled = GUI.Toggle(new Rect(Screen.width-80,0,80,40),(PlayerPrefs.GetInt("GodMode")==1),"God Mode");
			
			if(toggled){
				PlayerPrefs.SetInt("GodMode",1);
			}
			else{
				PlayerPrefs.SetInt("GodMode",0);
			}
		}
	}
	
	
}
