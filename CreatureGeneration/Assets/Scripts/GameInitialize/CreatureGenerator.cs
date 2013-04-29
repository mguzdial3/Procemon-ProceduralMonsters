using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

//Generates all the creatures, and passes them up to the CreatureHolder
//THIS IS THE ONE FOR NATHAN TO MESS WITH
public class CreatureGenerator {
	
	
	//Presenlty just does it randomly, but that needs to be changes
	public CreatureInfo[] generateCreatures(){
		
		CreatureInfo[] creatures = new CreatureInfo[60];
		
		string[] nameBits = {"un", "za", "muh", "ga","go","gi","da","do","di","du","fa","fo","fu","la","lo","lu","ma","mu","mi","naom","nurm",
		"nor","dude","bro","va","vo","vive","mon","bro","wes","gorn","florp","bag","dern","qui","ze","xon", "ley", "durn", "yui"};
		
		
		string[] fightBits = {"punch", "munch", "slam", "bam", "wham", "jam", "schism", "tackle", "strike", "pound", "slam", "jab", "bite", "slice",
			"blast", "kick", "charge", "chomp", "thrust", "blitz", "barrage", "onslaught", "foray", "skirmish", "encroach"};
		
		string[] boostBits = {"boost", "help", "raise", "expand", "extend", "increase", "aid", "succor", "lift", "cure", "heal"};
		string[] negativeBits = {"lower", "decrease", "abation", "cut", "shave", "down", "lessen", "harm", "ruin", "vandalize"};
		
		//public const int LIGHTNING= 0, GROUND =1, WATER =2, FIRE = 3, GRASS =4, AIR=5, NORMAL=6;
		string[] groundBits = {"ground", "rock", "earth", "dirt", "mud","grass", "leaf", "vine", "thorn", "branch"};
		string[] waterBits = {"water", "aqua", "rain", "tsunami", "shower"};
		string[] fireBits = {"fire", "flame", "fiery", "magma", "burn", "lightning", "thunder", "electric", "volt", "magnetic"};
		string[] airBits = {"air", "wind", "storm", "sky", "zephyr", "gust"};
		string[] normalBits = {"strong", "cunning", "killer", "death", "average"};
		
		string[][] elementalBits = {normalBits, fireBits, waterBits, groundBits, airBits};
		
		//Load the creatures from the last run
		List<SerializableCreature> oldCreatures = new List<SerializableCreature>();
		List<SerializableCreature> oldImages = new List<SerializableCreature>();
		int numCreatures = PlayerPrefs.GetInt("NumCreaturesSaved");
		for (int i = 0; i < numCreatures; i++) {
			Debug.Log("CREATURE LOADED");
			string temp = PlayerPrefs.GetString("CreatureStats" + i);
			string[] tempArray=temp.Split("*".ToCharArray());
			Debug.Log (tempArray[0]);
			if (tempArray.Length >= 7) {
				SerializableCreature newCreature = new SerializableCreature(System.Int32.Parse(tempArray[0]), System.Int32.Parse(tempArray[1]), System.Int32.Parse(tempArray[2]), System.Int32.Parse(tempArray[3]), System.Int32.Parse(tempArray[4]), System.Int32.Parse(tempArray[5]), float.Parse(tempArray[6], CultureInfo.InvariantCulture.NumberFormat));
				oldCreatures.Add(newCreature);	
			}
			
			string temp2 = PlayerPrefs.GetString("CreatureImages" + i);
			string[] temp2Array = temp2.Split("*".ToCharArray());
			if (temp2Array.Length >= 4) {
				int hasEyebrows = (temp2Array[4].Equals("True")) ? 1 : 0; 
				SerializableCreature newImageCreature = new SerializableCreature(System.Int32.Parse(temp2Array[0]), System.Int32.Parse(temp2Array[1]), System.Int32.Parse(temp2Array[2]), System.Int32.Parse(temp2Array[3]), hasEyebrows);	
				oldImages.Add(newImageCreature);
			}
		}
		
		/*List<SerializableCreature> oldCreatures = null;
		try
	    {
			using (Stream stream = File.Open("data.bin", FileMode.Open))
			{
		    	BinaryFormatter bin = new BinaryFormatter();
		    	oldCreatures = (List<SerializableCreature>)bin.Deserialize(stream);
			}
	    }
	    catch (IOException)
	    {
			Debug.LogError("Error loading creatures");
	    }*/
		
		//Indices in array correspond to the type and the value is the number of that type present
		int[] creatureTypeCount = new int[5];
		
		for(int i = 0; i<creatures.Length; i++){
			
			
			//Make creature Name
			int nameLength= Random.Range(2,4);
			int nameIndex = 0;
			
			
			string creatureName = "";
			while(nameIndex<nameLength){
				creatureName += nameBits[Random.Range(0, nameBits.Length)];
				nameIndex++;
			}
			
			
			//Set up the GA problem
			StatsProblem problem = new StatsProblem(oldCreatures, creatureTypeCount);
			GeneticAlgorithm ga = new GeneticAlgorithm(problem);
			
			//Run the GA
			Chromosome result = ga.evaluateProblem(50, 1000, true, 0.8, 0.015, 15);
			List<Feature> featureSet = result.chromosome;
			int attack = 0, speed = 0, defense = 0, special = 0, type = 0, hp = 0;
			float accuracy = 0;
			foreach(Feature f in result.chromosome)
			{
				//Debug.Log (f.label + ": " + f.value);
				if (f.label.Equals("Attack"))
					attack = (int) f.value;
				else if (f.label.Equals("Speed"))
					speed = (int) f.value;
				else if (f.label.Equals("Defense"))
					defense = (int) f.value;
				else if (f.label.Equals("Special"))
					special = (int) f.value;
				else if (f.label.Equals("Accuracy"))
					accuracy = (int) f.value;
				else if (f.label.Equals("HP"))
					hp = (int) f.value;
				else if (f.label.Equals("Type"))
					type = (int) f.value;
			}
			
			ImageProblem imageProblem = new ImageProblem(oldImages);
			GeneticAlgorithm imageGa = new GeneticAlgorithm(imageProblem);
			
			int bodySize = 0, headSize = 0, bodyType = 0, headType = 0, hasEyebrows = 0;
			Chromosome imageResult = imageGa.evaluateProblem(50, 1000, true, 0.8, 0.015, 15);
			
			foreach(Feature iRes in imageResult.chromosome)
			{
				if (iRes.label.Equals("bodySize"))
					bodySize = (int) iRes.value;
				else if (iRes.label.Equals("headSize"))
					headSize = (int) iRes.value;
				else if (iRes.label.Equals("bodyType"))
					bodyType = (int) iRes.value;
				else if (iRes.label.Equals("headType"))
					headType = (int) iRes.value;
				else if (iRes.label.Equals("eyebrows"))
					hasEyebrows = (int) iRes.value;
			}
			
			bool eyebrowsResult = hasEyebrows == 1;
			
			
		
			//All values sum to 10
			//int attack = Random.Range(1,8);
			//int speed = Random.Range(1, 9-attack);
			//int defense = Random.Range(1, 10-attack-speed);
			//int special = Random.Range(1, 11-attack-defense-speed);
			//Type of creature determined randomly
			//int type = Random.Range(0,5);
			//Randomly generate accuracy
			//float percentAccuracy = Random.Range(0.8f,1.0f);
			
			
			//Calculate hitpoints
			//int[] possibleHitPoints = {50, 55, 60, 65};
			//int hitpoints = possibleHitPoints[Random.Range(0, 4)];
			creatureTypeCount[type]++;
			CreatureImageData data = new CreatureImageData(type, bodySize, headSize, bodyType, headType, eyebrowsResult);
			CreatureInfo ci = new CreatureInfo(i, creatureName, attack, speed, defense, special, type, accuracy / 10, hp, data.image, data);

			
			
			//Four Basic Creature Attacks
			CreatureAttack[] fourBasic = new CreatureAttack[4];
			
			for(int f = 0; f<fourBasic.Length; f++){
				fourBasic[f] = generateAttack(ci.type, fightBits,elementalBits,boostBits,negativeBits);		
			}
			
			//Three Level Creature Attacks
			//TODO Fill this out
			CreatureAttack[] threeLevelGained = new CreatureAttack[4];
			
			for(int f = 0; f<threeLevelGained.Length; f++){
				threeLevelGained[f] = generateLevelAttack(ci.type, fightBits,elementalBits,boostBits,negativeBits);
				
			}
			
			
			ci.setAttacks(fourBasic,threeLevelGained);
			
			creatures[i]=ci;
			
		}
		for (int k = 0; k < creatureTypeCount.Length; k++)
		{
			Debug.Log(creatureTypeCount[k]);	
		}
		
		return creatures;
	}
	
	//Creates a boss creature
	public CreatureInfo generateBossCreature(int type, int level, int ID){
		string[] bossNameBits = {"big", "boss", "star", "chief", "king", "top", "czar"};
		string[] nameBits = {"un", "za", "muh", "ga","go","gi","da","do","di","du","fa","fo","fu","la","lo","lu","ma","mu","mi","naom","nurm",
		"nor","dude","bro","va","vo","vive","mon","bro","wes","gorn","florp","bag","dern","qui","ze","xon", "ley", "durn", "yui"};
		
		
		string[] fightBits = {"punch", "munch", "slam", "bam", "wham", "jam", "schism", "tackle", "strike", "pound", "slam", "jab", "bite", "slice",
			"blast", "kick", "charge", "chomp", "thrust", "blitz", "barrage", "onslaught", "foray", "skirmish", "encroach"};
		
		string[] boostBits = {"boost", "help", "raise", "expand", "extend", "increase", "aid", "succor", "lift", "cure", "heal"};
		string[] negativeBits = {"lower", "decrease", "abation", "cut", "shave", "down", "lessen", "harm", "ruin", "vandalize"};
		
		//public const int LIGHTNING= 0, GROUND =1, WATER =2, FIRE = 3, GRASS =4, AIR=5, NORMAL=6;
		string[] groundBits = {"ground", "rock", "earth", "dirt", "mud","grass", "leaf", "vine", "thorn", "branch"};
		string[] waterBits = {"water", "aqua", "rain", "tsunami", "shower"};
		string[] fireBits = {"fire", "flame", "fiery", "magma", "burn", "lightning", "thunder", "electric", "volt", "magnetic"};
		string[] airBits = {"air", "wind", "storm", "sky", "zephyr", "gust"};
		string[] normalBits = {"strong", "cunning", "killer", "death", "average"};
		string[][] elementalBits = {normalBits, fireBits, waterBits, groundBits, airBits};

		
		//Make creature Name
			int nameLength= Random.Range(1,3);
			int nameIndex = 0;
			
			
			string creatureName = bossNameBits[Random.Range(0, bossNameBits.Length)];
			while(nameIndex<nameLength){
				creatureName += nameBits[Random.Range(0, nameBits.Length)];
				nameIndex++;
			}
			
			
			//All values sum to 10
			int attack = Random.Range(1,8);
			int speed = Random.Range(1, 9-attack);
			int defense = Random.Range(1, 10-attack-speed);
			int special = Random.Range(1, 11-attack-defense-speed);
			//Type of creature determined randomly
			
			//Randomly generate accuracy
			float percentAccuracy = Random.Range(0.8f,1.0f);
			
			
			//Calculate hitpoints
			int[] possibleHitPoints = {60, 65,70, 75};
			int hitpoints = possibleHitPoints[Random.Range(0, 4)];
			CreatureImageData data = new CreatureImageData(type,true);
			CreatureInfo ci = new CreatureInfo(ID, creatureName, attack, speed, defense, special, type, percentAccuracy , 100, data.image, data);

			
			
			//Four Basic Creature Attacks
			CreatureAttack[] fourBasic = new CreatureAttack[4];
			
			for(int f = 0; f<fourBasic.Length; f++){
				fourBasic[f] = generateAttack(ci.type, fightBits,elementalBits,boostBits,negativeBits);		
			}
			
			//Three Level Creature Attacks
			//TODO Fill this out
			CreatureAttack[] threeLevelGained = new CreatureAttack[4];
			
			for(int f = 0; f<threeLevelGained.Length; f++){
				threeLevelGained[f] = generateLevelAttack(ci.type, fightBits,elementalBits,boostBits,negativeBits);
				
			}
			
			
			ci.setAttacks(fourBasic,threeLevelGained);
		
		
		ci.levelTo(level);
		
		return ci;
	}
	
	
	private CreatureAttack generateAttack(int element, string[] fightBits, string[][] elementalBits, string[] boostBits,string[] negativeBits ){
			//First, determine what the type of attack will be
				int attackType = Random.Range(-2,3);
				
				//Damage type
				if(attackType<=0){
					attackType=0;
					int fightIndex=0;
					int fightLength = Random.Range(1,3);
					
					
					//NAME OF ATTACK
					string basicAttack = "";
					
					while(fightIndex<fightLength){
						
						basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
						
						fightIndex++;
					}
					
					basicAttack= (basicAttack[0]).ToString().ToUpper() + basicAttack.Substring(1);
					
					
					//(string _name, int _attackType, int _elementalType, int _power, float _accuracyOfAttack, int _maxTimes, int _statTarget){
					
					//Element type for all basic moves is normal
					int elementalType = 0;
					
					//Calculate Power
					int power = Random.Range(15,36);
					
					//Accuracy of attack
					float accuracy = Random.Range(0.98f,1.0f);
					
					//Number of times you can do this
					int[] possibleNumberOfTimes = {20,25,30, 40, 60};
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					//Stat to target is hitpoints
					int statsToTarget = 0;
					
					return new CreatureAttack(basicAttack,attackType,elementalType,power,accuracy,numberOfTimes,statsToTarget);

					
					
				}
				//Boosts my stats type
				else if(attackType==1){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, boostBits.Length);
					
					nameOfAttack+= "-"+boostBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to boost
					int statBoostAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statBoostAmnt = Random.Range(1,4);
					}
					else{//We're looking at accuracy, needs to be higher
						statBoostAmnt = Random.Range(3,7);
						
					}
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30, 40};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statBoostAmnt,accuracyOfAttack,numberOfTimes,statsWerePointingAt);
					
				}
				//Harms your stats type
				else if(attackType==2){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, negativeBits.Length);
					
					nameOfAttack+= "-"+negativeBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to hurt
					int statHurtAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statHurtAmnt = Random.Range(1,4);
					}
					else{//We're looking at accuracy, needs to be higher
						statHurtAmnt = Random.Range(3,7);
						
					}
					
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statHurtAmnt,accuracyOfAttack,numberOfTimes,(int)statsWerePointingAt);
				}	
			return null;
	}
	
	private CreatureAttack generateLevelAttack(int element, string[] fightBits, string[][] elementalBits, string[] boostBits,string[] negativeBits ){
			//First, determine what the type of attack will be
				int attackType = Random.Range(-5,3);
				
				//Damage type
				if(attackType<=0){
					attackType=0;
					int fightIndex=0;
					int fightLength = Random.Range(1,3);
					
					
					//NAME OF ATTACK
					string basicAttack = "";
					
			
					int elementalType = element;
			
					if(elementalType==0){
			
					while(fightIndex<fightLength){
						
						basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
						
						fightIndex++;
					}
					
					}
					else{
						fightLength=2;
						basicAttack +=elementalBits[elementalType][Random.Range(0,elementalBits[elementalType].Length)];
						fightIndex++;
						
						while(fightIndex<fightLength){
							basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
						
							fightIndex++;
						}
				
					}
			
					basicAttack= (basicAttack[0]).ToString().ToUpper() + basicAttack.Substring(1);
					
					//Debug.Log("Elemental type: "+basicAttack);
					
					//Calculate Power
					int power = Random.Range(15,36);
					
					//Accuracy of attack
					float accuracy = Random.Range(0.98f,1.0f);
					
					//Number of times you can do this
					int[] possibleNumberOfTimes = {20,25,30, 40, 60};
			
					
			
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					power += Random.Range(5,8);
					numberOfTimes-=5;
					
			
			
					//Stat to target its hitpoints
					int statsToTarget = 0;
					
					return new CreatureAttack(basicAttack,attackType,elementalType,power,accuracy,numberOfTimes,statsToTarget);

					
					
				}
				//Boosts my stats type
				else if(attackType==1){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, boostBits.Length);
					
					nameOfAttack+= "-"+boostBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to boost
					int statBoostAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statBoostAmnt = Random.Range(7,14);
					}
					else{//We're looking at accuracy, needs to be higher
						statBoostAmnt = Random.Range(10,20);
						
					}
					
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30, 40};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statBoostAmnt,accuracyOfAttack,numberOfTimes,statsWerePointingAt);
					
				}
				//Harms your stats type
				else if(attackType==2){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, negativeBits.Length);
					
					nameOfAttack+= "-"+negativeBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to hurt
					int statHurtAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statHurtAmnt = Random.Range(7,14);
					}
					else{//We're looking at accuracy, needs to be higher
						statHurtAmnt = Random.Range(10,20);
						
					}
					
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statHurtAmnt,accuracyOfAttack,numberOfTimes,(int)statsWerePointingAt);
				}	
			return null;
	}
	
	
	
	
	//Pass in an attribute type, get a string out of it
	public string getStringGivenType(int statType){
		CreatureAttack c = new CreatureAttack();
		
		return c.getStringForStatType(statType);
	}
	
	
}
