using UnityEngine;
using System.Collections;

//For right now this just contains the list of the players creatures
//Later will be used to move creatures around in order for battle and check out their descriptions
public class PlayerCreatureHolder : MonoBehaviour {
	public CreatureInfo[] myCreatures;
	public int maxNumOfCreatures = 4;
	
	
	
	
	//Swap the indexes of the creatures
	public void swapIndexes(int firstIndex, int secondIndex){
		CreatureInfo firstPos = myCreatures[firstIndex];
		myCreatures[firstIndex] = myCreatures[secondIndex];
		myCreatures[secondIndex] = firstPos;
	}
	
	//Add creature
	public void addCreature(CreatureInfo c){
		if(myCreatures.Length<maxNumOfCreatures){
			CreatureInfo[] newCreatures = new CreatureInfo[myCreatures.Length+1];
			//Copy old creatures reference into new Creatures
			for(int i = 0; i<myCreatures.Length; i++){
				newCreatures[i] = myCreatures[i];
			}
			
			//Add additional Creature to the end
			newCreatures[myCreatures.Length] = c;
			
			
			//Set the reference of myCreatures
			myCreatures = newCreatures;
		}
	}
	
	//Set creature to given index
	//TODO; Store replaced creature somewhere
	public void setCreature(CreatureInfo c, int index){
		if(index>0 && index<myCreatures.Length){
			print("Got to here");
			myCreatures[index]=c;
		}
	}
}
