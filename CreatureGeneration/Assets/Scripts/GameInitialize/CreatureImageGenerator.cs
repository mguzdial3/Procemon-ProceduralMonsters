using UnityEngine;
using System.Collections;

public class CreatureImageGenerator  {
	
	
	public Texture2D MakeACreature(int type){
		Texture2D t = new Texture2D(100,100);
		
		
		int headSize = Random.Range(20,40);
		int headStartY = Random.Range(90-headSize, 100-headSize);
		int headStartX = Random.Range(10, 50-headSize/2);
		
		
		
		//int lineStart = headStartX;
		//int lineLength = headSize;
		
		//Draw head
		
		int prevX = headStartX;
		//NORMAL, FIRE/LIGHTNING, WATER, GRASS/GROUND, AND AIR
		Color[] choices = {Color.grey, Color.red, Color.blue,Color.green,Color.white};
		
		int colorChoice = type;
		
		Color toSet = choices[colorChoice];
		
		for(int y = headStartY; y<headStartY+headSize; y++){
			int start = prevX + Random.Range(-1,2);
			prevX=start;
			
			
			
			for(int x= start; x<headStartX+headSize-(start-headStartX); x++){
				toSet = toSet+new Color(Random.Range(-0.01f,0.01f),Random.Range(-0.01f,0.01f),Random.Range(-0.01f,0.01f),1.0f);	
				
				//Debug.Log("Got here");
				t.SetPixel(x,y,toSet);
			}
		}
		
		
		int eyeSize = Random.Range(3,7);
		bool firstSet=false, secondSet=false;
		
		//Eyes in middle of head (plus a little bit off)
		int eyeY = headStartY+headSize/2 + Random.Range(-5,5);
		
		int eye1X = headStartX+headSize/2 - (eyeSize*2 + Random.Range(2,6));
		
		int eye2X = headStartX+headSize/2 + (eyeSize*2 + Random.Range(2,6));
		
		for(int x = eye1X; x<eye1X+eyeSize; x++){
			for(int y = eyeY; y<eyeY+eyeSize; y++){
				t.SetPixel(x,y,Color.black);
			}
		}
		
		for(int x = eye2X; x<eye2X+eyeSize; x++){
			for(int y = eyeY; y<eyeY+eyeSize; y++){
				if(x==eye2X+eyeSize/2-1 && y==eyeY+eyeSize/2){
					t.SetPixel(x,y,Color.white);
				}
				else{
					t.SetPixel(x,y,Color.black);
				}
			}
		}
		
		
		
		
		//Draw body
		
		int bodyWidth = Random.Range(headSize, headSize+20);
		
		
		if(bodyWidth>100){
			bodyWidth = Random.Range(30,50);
		}
		
		int bodyLength = Random.Range(10, headStartY);
		
		
		int bodyStartX = headStartX-10;
		
		if(bodyStartX+bodyWidth>100){
			bodyStartX=100-bodyWidth;
		}
		
		int bodyStartY = headStartY-bodyLength;//Random.Range(headStartY+headSize-Random.Range(0,5)-bodyLength, headStartY+headSize-bodyLength);
		
		
		for(int x = bodyStartX; x<bodyStartX+bodyWidth; x++){
			for(int y = bodyStartY; y<bodyStartY+bodyLength; y++){
				t.SetPixel(x,y,choices[colorChoice]);
			}
		}
		
		
		
		
		
		
		t.Apply();
		
		
		return t;
		
	}
}
