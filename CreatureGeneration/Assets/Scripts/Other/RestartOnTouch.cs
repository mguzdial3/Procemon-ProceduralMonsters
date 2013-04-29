using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class RestartOnTouch : MonoBehaviour {
	

	void OnTriggerEnter(Collider other){
		//Save the CreatureMetrics
		List<SerializableCreature> creatureList = new List<SerializableCreature>();
		//Get all the creatures
		CreatureHolder holder = (CreatureHolder) FindObjectOfType(typeof(CreatureHolder));
		//Debug.Log(holder.allCreatures);
		//Add the dictionary entries to the list
		int index = 0;
		string temp = "";
		string temp2 = "";
		foreach(KeyValuePair<int, float> entry in CreatureMetrics.creaturePrefs)
		{
			temp = "";
			temp2 = "";
			if (holder.allCreatures.Length > entry.Key){
				temp += holder.allCreatures[entry.Key].speed.ToString() + "*";
				temp += holder.allCreatures[entry.Key].special.ToString() + "*";
				temp += holder.allCreatures[entry.Key].defense.ToString() + "*";
				temp += holder.allCreatures[entry.Key].type.ToString() + "*";
				temp += holder.allCreatures[entry.Key].attack.ToString() + "*";
				temp += holder.allCreatures[entry.Key].hitPoints.ToString() + "*";
				temp += entry.Value.ToString();
				PlayerPrefs.SetString("CreatureStats" + index, temp);
				
				temp2 += holder.allCreatures[entry.Key].data.bodySize.ToString() + "*";
				temp2 += holder.allCreatures[entry.Key].data.headSize.ToString() + "*";
				temp2 += holder.allCreatures[entry.Key].data.bodyType.ToString() + "*";
				temp2 += holder.allCreatures[entry.Key].data.headType.ToString() + "*";
				temp2 += holder.allCreatures[entry.Key].data.hasEyebrows.ToString() + "*";
				temp2 += entry.Value.ToString();
				PlayerPrefs.SetString("CreatureImages" + index, temp2);
				
				index++;
			}
		}
		PlayerPrefs.SetInt("NumCreaturesSaved", index);
		
		/*foreach(KeyValuePair<int,float> entry in CreatureMetrics.creaturePrefs)
		{
			Debug.Log ("Adding for: " + entry.Key);
			int speed = holder.allCreatures[entry.Key].speed;
			int special = holder.allCreatures[entry.Key].special;
			int defense = holder.allCreatures[entry.Key].defense;
			int type = holder.allCreatures[entry.Key].type;
			int attack = holder.allCreatures[entry.Key].attack;
			int hp = holder.allCreatures[entry.Key].hitPoints;
			float score = entry.Value;
			creatureList.Add(new SerializableCreature(speed, special, defense, type, attack, hp, score));
		}
		
		//Save the game data
		try
	    {
			using (Stream stream = File.Open("data.bin", FileMode.Create))
			{
			    BinaryFormatter bin = new BinaryFormatter();
			    bin.Serialize(stream, creatureList);
			}
	    }
	    catch (IOException)
	    {
			Debug.LogError("Error writng data to file");
	    }*/
		
		
		
		if(other.tag=="Player"){
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
