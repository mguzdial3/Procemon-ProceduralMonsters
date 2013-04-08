using UnityEngine;
using System.Collections;


//Gaurav, you'll want to put your stuff here, basically
public class CubeSpawner : MonoBehaviour {
	public GameObject Cube;
	int cubeNum = 0;
	// Use this for initialization
	void Start () {
		for(int x =-120; x<500; x+=13){
			for(int y = -85; y<500; y+=13){
				GameObject c = Instantiate(Cube,new Vector3(x,y,0.0f), Cube.transform.rotation) as GameObject;
				
				int rand = Random.Range(0,6);
				c.name="Cube: "+cubeNum;
				cubeNum++;
				if(rand==0){
					c.tag="Unpassable";
					c.renderer.material.color = Color.red;
				}
				else if(rand==1){
					c.tag="ContainsCreatures";
					c.renderer.material.color = Color.green;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
