using UnityEngine;
using System.Collections;

public class CreatureViewer : MonoBehaviour {
	//Reference to the player's movement
	public PlayerMovement movement;
	//Reference to player's creatures
	public PlayerCreatureHolder holder;
	
	//Whether or not we're in viewing creatures mode
	public bool viewingCreatures, switchingCreatures;
	
	//Indexes to switch with
	public int firstIndex, secondIndex;
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI(){
		
		if(!viewingCreatures && movement.inMvmtMode){
			//If in regular gameplay mode, see if we want to switch into viewing creatures mode
			if(GUI.Button(new Rect(0,0, 150,40), "View Creatures")){
				viewingCreatures=true;
				movement.inMvmtMode=false;
			}
		}
		else if(viewingCreatures){
			//If we're viewing the creatures, let's see if we want to switch back
			if(GUI.Button(new Rect(0,0, 150,40), "Back to Game")){
				viewingCreatures=false;
				movement.inMvmtMode=true;
			}
			
			
			CreatureInfo[] creatures = holder.myCreatures;
			
			for(int i =0; i<creatures.Length; i++){//Go through creatures of player and show each one
				if(creatures[i]!=null){
					GUI.DrawTexture(new Rect(10+i*(Screen.width/6),Screen.height/4, Screen.width/8, Screen.width/8),creatures[i].image);
					GUI.TextArea(new Rect(10+i*(Screen.width/6),(Screen.height/4)+(Screen.width/8), Screen.width/8,Screen.width/4), 
					"Name: "+creatures[i].name+"\nHP: "+creatures[i].hitPoints +"/"+creatures[i].maxHitPoints+"\nLevel: "+creatures[i].level+"\nXP: "+creatures[i].experience+"\nAttack: "+creatures[i].attack+"\nArmor: "+creatures[i].defense +"\nSpeed: "+creatures[i].speed +"\nSpecial: "+creatures[i].special +"\nType: "+creatures[i].getType()); 
					
				}
			}
			
			//Figure out if the user wants to switch creatures?
			if(!switchingCreatures){
				if(GUI.Button(new Rect(Screen.width/2-75,0, 150,30), "Swap Creatures")){
					switchingCreatures=true;
				}
			}
			else{//If we are switching
				int numCreatures = creatures.Length;
				
				if(GUI.Button(new Rect(Screen.width/2-75,0, 150,30), "Nevermind")){
					switchingCreatures=false;
				}
				
				//Display first index
				GUI.TextArea(new Rect(Screen.width/2-75, 30, 80,20), "First: "+(firstIndex+1));
				
				for(int i = 0; i<numCreatures; i++){//Set up the first index we're switching with
					if(GUI.Button(new Rect(Screen.width/2-75 +i*30,50, 30,20), ""+(i+1))){
						firstIndex = i;
					}
				}
				
				//Display Second index
				GUI.TextArea(new Rect(Screen.width/2-75, 70, 80,20), "Second: "+(secondIndex+1));
				
				for(int i = 0; i<numCreatures; i++){//Set up the second index we're switching with
					if(GUI.Button(new Rect(Screen.width/2-75 +i*30,90, 30,20), ""+(i+1))){
						secondIndex = i;
					}
				}
				
				//The swap button!
				if(GUI.Button(new Rect(Screen.width/2,110, 150,30), "Swap")){
					if(firstIndex!=secondIndex){//If the indexes are non-equal, swap them
						holder.swapIndexes(firstIndex,secondIndex);
						switchingCreatures=false;
					}
				}
				
			}
			
			
			
			
		}
	}
}
