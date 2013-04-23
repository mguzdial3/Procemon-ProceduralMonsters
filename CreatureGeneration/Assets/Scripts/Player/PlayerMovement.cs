using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	//The speed to move the player at
	public float speed = 5.0f;
	//In regular movement mode (as opposed to a menu, at which point you shouldn't be moving);
	public bool inMvmtMode=true;
	//Array of string tag names we're not allowed to go to
	public string[] cantMoveToTheseTags;
	public float width = 1.0f;
	
	//Setting up the original position tracking
	private Vector3 origPos;
	private bool originalPosSet;
	
	// Update is called once per frame
	void Update () {
		if(!originalPosSet){
			origPos=transform.position;
			originalPosSet=true;
		}
		
		if(inMvmtMode){
			Vector3 newPos = transform.position;
			if(Input.GetKey(KeyCode.UpArrow)){
				newPos += new Vector3(0,Time.deltaTime*speed,0);
			}
			else if(Input.GetKey(KeyCode.DownArrow)){
				newPos += new Vector3(0,Time.deltaTime*-speed,0);
			}
			
			else if(Input.GetKey(KeyCode.LeftArrow)){
				newPos += new Vector3(Time.deltaTime*-speed,0,0);
			}
			else if(Input.GetKey(KeyCode.RightArrow)){
				newPos += new Vector3(Time.deltaTime*speed,0,0);
			}
			
			RaycastHit hit, hit2, hit3;
			
			Vector3 difference = (newPos-transform.position);
			
			Vector3 perpNewPos = new Vector3(difference.y, -1*difference.x,0.0f);
			Vector3 perpNewPos2 = new Vector3(-1*difference.y, difference.x,0.0f);
			//Make sure we're not looking from the middle but from the edge
			
			//"Bottom"
			Vector3 edge1 = newPos + (newPos-transform.position).normalized*(width/2)+ perpNewPos.normalized*(width/2);
			
			//"Top"
			Vector3 edge2 = newPos + (newPos-transform.position).normalized*(width/2)+ perpNewPos2.normalized*(width/2);
			//Center
			Vector3 edge3 = newPos + (newPos-transform.position).normalized*(width/2);
			
			//Check the cube we're about to go to, whether or not it is an allowable position
			if(Physics.Raycast(edge1,Vector3.forward,out hit, 6) || Physics.Raycast(edge2,Vector3.forward,out hit2, 6)
				|| Physics.Raycast(edge3,Vector3.forward,out hit3, 6)){
				//If the tag is a non-allowable one, don't go there
				bool dontGoThere=false;
				
				foreach(string dontGoTag in cantMoveToTheseTags){
					if((hit.collider!=null && dontGoTag==hit.transform.tag)
					|| 	(hit2.collider!=null && dontGoTag==hit2.transform.tag)
						|| 	(hit3.collider!=null && dontGoTag==hit3.transform.tag)){
						dontGoThere=true;
					}
				}
				
				if(dontGoThere){
					//Don't go there! Duh!
				
				}
				else{
					transform.position = newPos;
				}
				
				
			}
			else{
				//don't go off the world!
				
			}
		}
	}
	//To be called when you heal
	public void setOrigPosition(){
		origPos=transform.position;
	}
	
	public void setToOrigPosition(){
		transform.position = origPos;
	}
}
