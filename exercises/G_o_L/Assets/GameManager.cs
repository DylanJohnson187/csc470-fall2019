using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public CellScript[,] grid;

	public GameObject cellPrefab;

    public PlayerScript player;

    public float sx;
    public float sy;

	public bool simulate = false;

	public int gridWidth = 50;
	public int gridHeight = 50;

	float cellDimension = 3.3f;
	float cellSpacing = 0.2f;

	float generationRate = 3f;
	float generationTimer;
    int time = 0;

    float tileMoveRate = .5f;
    float tilemoveTimer;

    BoxCollider lCollider;


    // Start is called before the first frame update
    void Start()
	{
        //Always spawn in the lower left hand corner, just for ease of creation
        int spawnX = 0;
        int spawnY = 0;

        sx = spawnX * (cellDimension + cellSpacing);
        sy = spawnY * (cellDimension + cellSpacing);

        //goal is 4x4 randomly generated sqaure this way it will stay as a constant
        int goalX = Random.Range(0, gridWidth - 1);
        int goalY = Random.Range(0, gridHeight - 1);

        while((goalX == spawnX && goalY == spawnY) || (goalX == spawnX + 1 && goalY == spawnY) ||
            (goalX == spawnX && goalY + 1 == spawnY) || (goalX == spawnX + 1 && goalY == spawnY +1))
        {
            goalX = Random.Range(0, 10);
            goalY = Random.Range(0, 10);
        }

        grid = new CellScript[gridWidth, gridHeight];

        /*Vector3 lossVector = new Vector3(gridWidth / 2f * (cellDimension + cellSpacing), 0,gridHeight / 2f * (cellDimension + cellSpacing));
        GameObject lossCollider = Instantiate(cellPrefab, lossVector, Quaternion.identity);
        Vector3 transVector = new Vector3(gridHeight*5, 0, gridWidth *5);
        lossCollider.transform.localScale += transVector;

        transVector = new Vector3(gridHeight * 5, 1.5f, gridWidth * 5);
        lossCollider.GetComponent<BoxCollider>().transform.position = lossVector;
        lossCollider.GetComponent<BoxCollider>().size = transVector;
        lossCollider.GetComponent<BoxCollider>().isTrigger = true;
        lossCollider.GetComponent<BoxCollider>().tag = "loss";*/
        /*Debug.Log("Current box collider size : " + lossCollider.GetComponent<BoxCollider>().size);
        Debug.Log("Current box collider position : " + lossCollider.GetComponent<BoxCollider>().transform.position);

        Debug.Log("Current floor size : " + lossCollider.transform.localScale);
        Debug.Log("Current floor position : " + lossCollider.transform.position);*/

        //Using nested for loops, instantiate cubes with cell scripts in a way such that
        //	each cell will be places in a top left oriented coodinate system.
        //	I.e. the top left cell will have the x, y coordinates of (0,0), and the bottom right will
        //	have the coodinate (gridWidth-1, gridHeight-1)
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++)
            {

                //Create a cube, position/scale it, add the CellScript to it.
                Vector3 pos = new Vector3(x * (cellDimension + cellSpacing),
                                            0,
                                            y * (cellDimension + cellSpacing));
                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity);
                CellScript cs = cellObj.AddComponent<CellScript>();
                cs.x = x;
                cs.y = y;
                cs.alive = (Random.value > 0.5f) ? true : false;
                if (x == spawnX && y == spawnY)
                {
                    cs.spawn = true;
                    cs.alive = true;
                }
                if (x == goalX && y == goalY || x == goalX+1 && y == goalY || 
                    x == goalX && y == goalY+1 || x == goalX +1  && y == goalY+1)
                {
                    cs.goal = true;
                    cs.alive = true;
                    //Should create a box collider for the goal -- dont need to do this i think
                    /*cellObj.GetComponent<BoxCollider>().tag = "Goal";
                    cellObj.GetComponent<BoxCollider>().size = new Vector3((cellDimension + cellSpacing),1, (cellDimension + cellSpacing));
                    cellObj.GetComponent<BoxCollider>().isTrigger = true;
                    Debug.Log("Current goal box collider size : " + cellObj.GetComponent<BoxCollider>().size);
                    Debug.Log("Current goal box collider position : " + cellObj.GetComponent<BoxCollider>().transform.position);*/
                }
                cs.updateColor();
                if (!cs.alive && !cs.spawn && !cs.goal)
                {
                    Vector3 dead_down = new Vector3(0, -1, 0);
                    Vector3 newPos = dead_down + pos;
                    cellObj.transform.position = newPos;
                }
                else
                { 
                    cellObj.transform.position = pos;
                }
				cellObj.transform.localScale = new Vector3(cellDimension,
																cellDimension,
																cellDimension);
				//Finally add the cell to it's place in the two dimensional array
				grid[x, y] = cs;
			}
		}

        //Initialize the timer
        generationTimer = generationRate;
        tilemoveTimer = tileMoveRate;


	}

	private void Update()
	{
		generationTimer -= Time.deltaTime;
        if (player.winCondition())
        {
            Debug.Log("YOU WIN");
        }
		if (generationTimer < 0 && simulate) { 
			//generate next state
			generate();

			//reset the timer
			generationTimer = generationRate;
		}
	}

	void generate()
	{
		//This isn't really being used, but why not have the simulation know how
		//many times it has "generated" new states?
		time++;

		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				List<CellScript> liveNeighbors = gatherLiveNeighbors(x, y);
                //Apply the 4 rules from Conway's Gaem of Life (https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)
                //1. Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
                if (grid[x, y].spawn)
                {
                    grid[x, y].nextAlive = true;
                }
                else if (grid[x, y].goal)
                {
                    grid[x, y].nextAlive = true;
                }
				else if (grid[x, y].alive && liveNeighbors.Count < 2) {
					grid[x, y].nextAlive = false;
				}
				//2. Any live cell with two or three live neighbours lives on to the next generation.
				else if (grid[x, y].alive && (liveNeighbors.Count == 2 || liveNeighbors.Count == 3)) {
					grid[x, y].nextAlive = true;
                    //this should be current alive but thats what it does
				}
				//3. Any live cell with more than three live neighbours dies, as if by overpopulation.
				else if (grid[x, y].alive && liveNeighbors.Count > 3) {
					grid[x, y].nextAlive = false;
				}
				//4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
				else if (!grid[x, y].alive && liveNeighbors.Count == 3) {
					grid[x, y].nextAlive = true;
				}
			}
		}
        //Now that we have looped through all of the cells in the grid, and stored what their alive status should
        //	be in each cell's nextAlivevariable, update each cell's alive state to be that value.
        

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //I think here is where i should nest the movement timer to have slow movement for the tiles up and down
                if (!grid[x, y].nextAlive && grid[x, y].alive && grid[x,y].transform.position.y >=-1)
                {
                    Vector3 dead_down = new Vector3(0, -1, 0);
                    grid[x, y].transform.position = grid[x, y].transform.position + dead_down;
                }
                else if (grid[x, y].nextAlive && grid[x, y].transform.position.y < 0)
                {
                    Vector3 live_up = new Vector3(0, 1, 0);
                    grid[x, y].transform.position = grid[x, y].transform.position + live_up;
                }
                grid[x, y].alive = grid[x, y].nextAlive;
            }
        }
	}

	//This function returns all the live neighbors
	List<CellScript> gatherLiveNeighbors(int x, int y)
	{
		List<CellScript> neighbors = new List<CellScript>();
		//Spend some time thinking about how this considers all surrounding cells in grid[x,y]
		//why now indexing bad values of grid.
		for (int i = Mathf.Max(0, x - 1); i <= Mathf.Min(gridWidth - 1, x + 1); i++) {
			for (int j = Mathf.Max(0, y - 1); j <= Mathf.Min(gridHeight - 1, y + 1); j++) {
				//Add all live neighbors of (x, y) excluding itself
				if (grid[i,j].alive && !(i == x && j == y)) {
					neighbors.Add(grid[i, j]);
				}
			}
		}
		return neighbors;
	}

	//This function is called by the UI toggle's event system (look at the ToggleSimulateButton
	//child of the Canvas)
	public void toggleSimulate(bool value)
	{
		simulate = value;
	}
}