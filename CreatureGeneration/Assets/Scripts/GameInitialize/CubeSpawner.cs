using UnityEngine;
using System.Collections;


//Gaurav, you'll want to put your stuff here, basically
public class CubeSpawner : MonoBehaviour {
	private class patch{
		public string type;
		public GameObject cube;
		public patch(GameObject cb,string typ)
		{
			cube=cb;
			type=typ;
		}
	}
	private class point{
		public int x,y;
		public point(int x1,int y1)
		{
			x=x1;
			y=y1;
		}
	}
	static int[] N={1, 0, -1}, S={2, 0, 1}, E={4, 1, 0}, W={8, -1, 0};
	static string[] dir={"N","S","E","W"};
	private class MazeGenerator {
		int x;
		int y;
		public int[,] maze;
		Random rand = new Random();
		
		public int bit(string s)
		{
			switch(s)
			{
				case "N":return N[0];break;case "S":return S[0];break;case "E":return E[0];break;case "W":return W[0];break;
			}
			return 0;
		}
		public int dx(string s)
		{
			switch(s)
			{
				case "N":return N[1];break;case "S":return S[1];break;case "E":return E[1];break;case "W":return W[1];
			}
			return 0;
		}
 	 	public int dy(string s)
		{
			switch(s)
			{
				case "N":return N[2];break;case "S":return S[2];break;case "E":return E[2];break;case "W":return W[2];
			}
			return 0;
		}
		public string opposite(string s)
		{
			switch(s)
			{
				case "N":return "S";break;case "S":return "N";break;case "E":return "W";break;case "W":return "E";
			}
			return "";
		}
		public string[] shuffledValues()
		{
			string[] list=dir;
	        int n = Random.Range(1,40);
	        while (n > 1)
	        {
	            n--;
				int swap1 = Random.Range(0,4);
				int swap2 = Random.Range(0,4);
	            string val = list[swap1];
	            list[swap1] = list[swap2];
	            list[swap2] = val;
	        }
			return list;
		}
		public MazeGenerator(int x, int y) {
			this.x = x;
			this.y = y;
			maze = new int[this.x,this.y];
			generateMaze(0, 0);
		}
		private void generateMaze(int cx, int cy) {
			string[] dirs = shuffledValues();
			for (int i=0;i<dirs.Length;i++) {
				string dir=dirs[i];
				int nx = cx + dx(dir);
				int ny = cy + dy(dir);
				if (between(nx, x) && between(ny, y)
						&& (maze[nx,ny] == 0)) {
					maze[cx,cy] |= bit(dir);
					maze[nx,ny] |= bit(opposite(dir));
					generateMaze(nx, ny);
				}
			}
		}
	 
		private static bool between(int v,int upper) 
		{
			return (v >= 0) && (v < upper);
		}
	}
	// Use this for initialization
	int sizeX=40,sizeY=40;
	patch[,,] mapContents=new patch[9,40,40];
	private Texture grassTexture,roadTexture,fenceTexture,waterTexture,beachTexture;
	
		
	public GameObject Cube;
	public GameObject RestartIcon;
	ArrayList paths=new ArrayList();
	// Use this for initialization
	void Start () {
		grassTexture = Resources.Load("grass") as Texture;
		roadTexture = Resources.Load("road") as Texture;
		fenceTexture = Resources.Load("fence") as Texture;
		waterTexture = Resources.Load("water") as Texture;
		beachTexture = Resources.Load("beach") as Texture;
		generateMap();
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	//The funcs I give are below this line
	public GameObject LoneCreature1;
	private void generateMap()
	{
		//Grassify everything and Add border
		createGrass(0,0,0);
		//Pathing
		createRoad(0,39,35);
		//Watering hole
		createWater(0);
		/*
		//Grassify everything and Add border
		createGrass(1,1,0);
		//Pathing
		createRoad(1,12,19);
		//Watering hole
		createWater(1);*/
		
		MazeGenerator m = new MazeGenerator(sizeX/4, sizeY/4);
		visualizeMaze(m.maze,1,1,0);
		//Connect two maps
		changeCube("grass",0,0,0,39,35);//Absolute values, right now
		changeCube("grass",0,0,0,39,36);
		changeCube("grass",1,1,0,0,35);
		changeCube("grass",1,1,0,0,36);
		//Block route
		GameObject legend1 = Instantiate(LoneCreature1,new Vector3(0*sizeX*13+(39-sizeX/2)*13,0*sizeY*13+(35-sizeY/2)*13+5.0f,-13.0f), transform.rotation) as GameObject;

		//Some trainers to make things interesting
		addTrainers(1,1,0,5);
		//End game
		GameObject completed = Instantiate(RestartIcon,new Vector3(1*sizeX*13+(35-sizeX/2)*13,0*sizeY*13+(5-sizeY/2)*13,-13.0f), RestartIcon.transform.rotation) as GameObject;

	}
	private void createGrass(int mapId,int originX,int originY)
	{
		for (int y = 0; y < sizeX; y++) {
			for (int x = 0; x < sizeY; x++) {
				//GameObject cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
				//cube.transform.position = new Vector3(x-sizeX/2, y-sizeY/2, 0);
				GameObject cube = Instantiate(Cube,new Vector3(originX*sizeX*13+(x-sizeX/2)*13,originY*sizeY*13+(y-sizeY/2)*13,0.0f), Cube.transform.rotation) as GameObject;

				if(x<1 || y<1 || x>sizeX-2 || y>sizeY-2)
				{
					cube.renderer.material.mainTexture=fenceTexture;
					cube.tag="Unpassable";
					mapContents[mapId,x,y]=new patch(cube,"fence");
				}
				else
				{
					cube.renderer.material.mainTexture=grassTexture;
					//if(Random.value<0.4)
					{
						cube.tag="ContainsCreatures";
					}
					mapContents[mapId,x,y]=new patch(cube,"grass");
				}
			}
		}
	}
	private void createRoad(int mapId,int seedX,int seedY)
	{
		string[] directions={"up","down","left","right"};
		string going=directions[2];//initial direction
		int count=0;
		while(count<280)
		{
			createRoadPatch(mapId,seedX,seedY);
			if(going=="up")
			{
				//createRoadPatch(seedX-1,seedY);
				seedY--;
			}
			if(going=="down")
			{
				//createRoadPatch(seedX-1,seedY);
				seedY++;
			}
			if(going=="left")
			{
				//createRoadPatch(seedX,seedY-1);
				seedX--;
			}
			if(going=="right")
			{
				//createRoadPatch(seedX,seedY-1);
				seedX++;
			}
			while(seedX<1 || seedX > sizeX-2 || seedY<1 || seedY>sizeY-2)
			{
				point branch=(point)paths[(int)(Random.value*(paths.Count-1))];
				seedX=branch.x;
				seedY=branch.y;
				going=directions[different(going)];
			}
			//if(Random.value>0.95)
			//	going=directions[different(going)];
			count++;
		}
	}
	private int different(string g)
	{
		if(g=="up" || g=="down")
		{
			if(Random.value>0.5)
				return 2;
			return 3;
		}
		if(Random.value>0.5)
			return 1;
		return 0;
	}
	private void createRoadPatch(int mapId,int seedX,int seedY)
	{
		patch p=mapContents[mapId,seedX,seedY];
		paths.Add(new point(seedX,seedY));
		GameObject cube=p.cube;
		cube.renderer.material.mainTexture=roadTexture;
		p.type="road";
		cube.tag="Road";
	}
	private void createWater(int mapId)
	{
		int noLakes=Random.Range(2,4);
		do
		{
			int waterSizeX=Random.Range(6,15);
			int waterSizeY=Random.Range(6,15);
			int count=0;
			do
			{
				int seedX=Random.Range(3,sizeX-3);
				int seedY=Random.Range(3,sizeY-3);
				bool canPlace=checkPlacement(mapId,seedX,seedY,waterSizeX,waterSizeY);
				if(canPlace)
				{
					placeWater(mapId,seedX,seedY,waterSizeX,waterSizeY);
					count=100;
				}
				count++;
			}while(count<100);
			noLakes--;
		}while(noLakes>0);
	}
	private bool checkPlacement(int mapId,int left,int top,int right,int bottom)
	{
		right=left+right;
		bottom=top+bottom;
		for(int x=left;x<right;x++)
		{
			for(int y=top;y<bottom;y++)
			{
				if(mapContents[mapId,x,y].type!="grass")
					return false;
			}
		}
		return true;
	}
	private void placeWater(int mapId,int left,int top,int right,int bottom)
	{
		right=left+right;
		bottom=top+bottom;
		for(int x=left;x<right;x++)
		{
			for(int y=top;y<bottom;y++)
			{
				if(x==left || x==right-1 || y==top || y==bottom-1)
				{
					mapContents[mapId,x,y].cube.renderer.material.mainTexture=beachTexture;
					mapContents[mapId,x,y].type="beach";
					mapContents[mapId,x,y].cube.tag="Beach";
				}
				else
				{
					mapContents[mapId,x,y].cube.renderer.material.mainTexture=waterTexture;
					mapContents[mapId,x,y].type="water";
					mapContents[mapId,x,y].cube.tag="Unpassable";
				}
			}
		}
	}
	private void changeCube(string s,int mapId,int originX,int originY,int x,int y)
	{
		if(s=="fence")
		{
			mapContents[mapId,x,y].cube.renderer.material.mainTexture=fenceTexture;
			mapContents[mapId,x,y].cube.tag="Unpassable";
			mapContents[mapId,x,y].type="fence";
		}
		else if(s=="grass")
		{
			mapContents[mapId,x,y].cube.renderer.material.mainTexture=grassTexture;
			mapContents[mapId,x,y].cube.tag="ContainsCreatures";
			mapContents[mapId,x,y].type="grass";
		}		
	}
	private void blankMap(int mapId,int originX,int originY)
	{
		for(int x=0;x<sizeX;x++)
		{
			for(int y=0;y<sizeY;y++)
			{
				GameObject cube = Instantiate(Cube,new Vector3(originX*sizeX*13+(x-sizeX/2)*13,originY*sizeY*13+(y-sizeY/2)*13,0.0f), Cube.transform.rotation) as GameObject;
				cube.renderer.material.mainTexture=grassTexture;
				cube.tag="ContainsCreatures";
				mapContents[mapId,x,y]=new patch(cube,"grass");
			}
		}
	}
	private void visualizeMaze(int[,] maze,int mapId,int originX,int originY)
	{
		blankMap(mapId,originX,originY);
		for (int i = 0; i < sizeY/4; i++) 
		{
            for (int j = 0; j < sizeX/4; j++) 
			{
				int lx=4*i,ly=4*j;
                if((maze[i,j] & 1) == 0)
				{
					//"+---"
					changeCube("fence",mapId,originX,originY,lx,ly);lx++;
					changeCube("fence",mapId,originX,originY,lx,ly);lx++;
					changeCube("fence",mapId,originX,originY,lx,ly);lx++;
					changeCube("fence",mapId,originX,originY,lx,ly);lx++;
				}
				else
				{
					//"+   "
					changeCube("fence",mapId,originX,originY,lx,ly);lx++;
				}
            }
            for (int j = 0; j < sizeX/4; j++) 
			{
				int lx=4*i,ly=4*j;
                if((maze[i,j] & 8) == 0)
				{
					//"|   " 
					changeCube("fence",mapId,originX,originY,lx,ly);
					ly++;
					changeCube("fence",mapId,originX,originY,lx,ly);
					ly++;
					changeCube("fence",mapId,originX,originY,lx,ly);
					ly++;
					changeCube("fence",mapId,originX,originY,lx,ly);
					lx++;ly++;
				} 
				else
				{
					//"    "
				}
            }
        }
		for(int x=0;x<sizeX;x++)
		{
        	for (int y = 0; y < sizeY; y++) 
			{
               //"+---"
				if(x<1 || y<1 || x>sizeX-2 || y>sizeY-2)
				{
					changeCube("fence",mapId,originX,originY,x,y);
				}
        	}
		}
        //"+"
	}
	public Rigidbody trainer;
	private void addTrainers(int mapId,int originX,int originY,int noTrainers)
	{
		while(noTrainers>0)
		{
			noTrainers--;
			int x=0,y=0;
			do
			{
				x=Random.Range(5,35);
				y=Random.Range(5,35);
			}while(!clearPosition(mapId,x,y));
			
			Rigidbody trainer1 = (Rigidbody) Instantiate(trainer, new Vector3(originX*sizeX*13+(x-sizeX/2)*13,originY*sizeY*13+(y-sizeY/2)*13,-13.0f), transform.rotation);
			trainer1.GetComponent<CreatureTrainer>().setup(5,3,2,0);//All trainers are the same!
		}
	}
	bool clearPosition(int mapId,int x,int y)
	{
		for(int i=x-1;i<x+2;i++)
		{
			for(int j=y-1;j<y+2;j++)
			{
				if(mapContents[mapId,x,y].type!="grass")
					return false;
			}
		}
		return true;
	}
}
