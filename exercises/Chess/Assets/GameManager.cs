using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // This will hold a reference to whichever Unit was selected last.
    UnitScript selectedUnit;


    //so far i dont need this chunk
    // References to a handful of UI elements.
    public GameObject talkBox;
    public Text talkText;
    public ToggleGroup actionSelectToggleGroup;
    public GameObject selectedPanel;
    public Text nameText;
    public Image portraitImage;



    public GameObject cell_board;


    // Set the fillAmount of this Image to a value between 0 and 1 to set the meter.
    public Image meterFG;
    // We will move this object around and turn it on and off (done in UnitScript OnMouse function).
    public GameObject healthMeterObject;

    TileScript[] tileboard;

    string current_piece;

    public int[,] board = new int[8, 8] { 
        { 3, 4, 5, 7, 6, 5, 4, 3},
        { 1, 1, 1, 1, 1, 1, 1, 1},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 8, 8, 8, 8, 8, 8, 8, 8},
        {10, 11, 12, 14, 13, 12, 11, 10 } };

    //so i am going to need a list of pieces to iterate through, get their positions x,z
    //then use these positions to assign values to the matrix board 


    // Start is called before the first frame update
    void Start()
    {
        //TileScript[,] tile_board = new TileScript[8,8];

        

        /*
        int count = 0;
        foreach(TileScript child in allchildren)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (child.X_position == j && child.Z_position == i)
                    {
                        tile_board[i, j] = child;
                        count++;
                    }
                }
            }
        }
        Debug.Log("Number of children in tile_board is :" + count);
        Debug.Log("tile_board at in array has X_position :" + tile_board[0, 1].X_position + " and z_position: " + tile_board[0, 1].Z_position);*/

    }

    // Update is called once per frame
    void Update()
    {
        // Input.GetMouseButtonDown(0) is how you detect that the left mouse button has been clicked.
        //
        // The IsPointerOverGameObject makes sure the pointer is over the UI. In this case,
        // we don't want to register clicks over the UI when determining what unit is 
        // selected or deselected.

        //will constantly update the board
        TileScript[] tileboard = cell_board.GetComponentsInChildren<TileScript>();
        /*int count = 0;
        foreach (TileScript child in tileboard)
        {
            count++;
            Debug.Log("Child at position (" + child.X_position + ", " + child.Z_position + ")");
        }
        Debug.Log("Number of children in tile_board is :" + count);*/

        

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //So this is the way to get the tag for the selected unit
            current_piece = selectedUnit.tag;
            Debug.Log("selected unit has this tag : " + current_piece + " at location " + selectedUnit.transform.position);
            board = UpdatedBoard(board, tileboard);
            int[,] movelist = generateMoves();
            int X_length = movelist.GetLength(0);
            setValidTiles(movelist, tileboard);
            for (int i = 0; i < X_length; i++)
            {
                Debug.Log("Current available moves are: " + movelist[i, 0] + " and " + movelist[i, 1]);
            }


            // Create a ray from the mouse position (in camera/ui space) to 3d world space.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // After the Raycast, 'hit' will store information about what the raycast hit.
            RaycastHit hit;
            // The line below actually performs the "raycast". This will 'shoot' a line from the
            // mouse position into the world, and it if hits something marked with the layer 'ground', 
            // return true.
            if (Physics.Raycast(ray, out hit, 9999))
            {
                // Check to see if the thing the raycast hit was marked with the layer 'ground'.
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    // If so, set the destination of the selectedUnit to the point on the ground
                    // that the raycast hit.
                    if (selectedUnit != null)
                    {
                        selectedUnit.destination =  Vector3Int.RoundToInt(hit.point);
                    }
                }
            }
            else
            {
                // If we got here, it means that the raycast didn't hit anything, so let's deselect.
                if (selectedUnit != null)
                {
                    selectedUnit.selected = false;
                    selectedUnit.setColorOnMouseState();
                    selectedUnit = null;

                    updateSelectedPanelUI();
                }
            }
        }
    }

    public void selectUnit(UnitScript unit)
    {
        // If we have selected something previously, unselect it and update the color.
        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit.setColorOnMouseState();
        }

        // Set selected unit to the one we just passed in.
        selectedUnit = unit;

        if (selectedUnit != null)
        {
            // If there is a selected unit, update its color.
            selectedUnit.selected = true;
            selectedUnit.setColorOnMouseState();
        }

        updateSelectedPanelUI();
    }

    // This function updates the UI elements based on what was clicked on.
    void updateSelectedPanelUI()
    {
        // Only update the UI is there is a unit selected.
        if (selectedUnit != null)
        {
            nameText.text = selectedUnit.name;
            portraitImage.sprite = selectedUnit.portrait;
            selectedPanel.SetActive(true);
        }
        else
        {
            // If there is no selected unit, turn the panel off.
            selectedPanel.SetActive(false);
        }
    }

    // This function is called by the EventSystem when the player clicks on the PerformActionButton.
    public void TakeAction()
    {
        // Compute the screen position 2 units above the unit and place the talkBox.
        Vector3 pos = selectedUnit.transform.position + Vector3.up * 2;
        pos = Camera.main.WorldToScreenPoint(pos);
        talkBox.transform.position = pos;

        // Figure out which toggle button is selected in the action select toggleGroup
        // and store the text value of the button in a string.
        IEnumerable<Toggle> activeToggles = actionSelectToggleGroup.ActiveToggles();
        string action = "";
        foreach (Toggle t in activeToggles)
        {
            if (t.IsActive())
            {
                action = t.gameObject.GetComponentInChildren<Text>().text;
            }
        }

        // This registers a function with Unity's coroutine system (see notes above the function definition)
        StartCoroutine(displayTalkBoxMessages(new string[] { action, "I'm done talking", "One more thing...", "Nevermind." }));
    }

    // This type of function is registered with Unity's coroutine system. It doesn't run like
    // other functions (from top to bottom), but instead each update cycle is first
    // ran until some "yield return..." command is reached. After that point, the function
    // is "checked in" with automatically starting from the line after the "yield 
    // return...". This happens until the end of the function is reached.
    //
    // This particular coroutine recieves an array of string messages and displays each
    // until the mouse is pressed.
    IEnumerator displayTalkBoxMessages(string[] messages)
    {
        talkBox.SetActive(true);
        for (int i = 0; i < messages.Length; i++)
        {
            talkText.text = messages[i];

            // Wait for the mouse to be pressed
            while (!Input.GetMouseButtonDown(0))
            {
                // Tell the coroutine system that we are done for this update cycle.
                yield return null;
            }

            // If we get here, it means that the mouse was just pressed. Tell the coroutine
            // system that we are done for this update cycle.
            yield return null;
        }
        talkBox.SetActive(false);
    }

    public int [,] UpdatedBoard( int[,]array, TileScript[] Tiles)
    {
        //So here we want to read from the collision handing in tile script
            //by reading from the triggers from tile script we should be able to determine the occupied values on the board
        //take the new occupied value then use an adjusted value for transform.x (x+1) find transform.z
        foreach(TileScript child in Tiles)
        {
            array[child.Z_position, child.X_position] = child.occupied;
        }
        return array;
    }

    public int[,] generateMoves()
    /*
     * this will return a list of moves based on the selected piece and the current board
     */
    {
        //Generate an 2d-array for each piece with different sizes for each to denote the multitude of moves possible
        //int[,] moves = new int[,];
        int[,] default_arr = new int[3,2];
        //SO THE PIECES ARE LINED UP WITH THE BOARD REPRESENTATION HOWEVER THE TILE LOCATIONS SOMEHOW ARE NOT TILES WILL REQUIRE A MINUS 1 TO THE X
        //location_x = location_x + 1;                            //this is to translate from in-game tile representation to board representation
        int location_x = (int)selectedUnit.transform.position.x;
        int location_z = (int)selectedUnit.transform.position.z;

        if (current_piece == "White_pawn")                      //White Pawns can generate movelists now
        {
            int[,] moves = new int[4,2];
            moves = NegativeArr(moves);
            if(location_z == 1)                                 //This is how we would know it hasnt moved
            {
                //allow for double move
                if (board[location_z+1, location_x] == 0)
                {
                    moves[0, 0] = location_x;
                    moves[0, 1] = location_z+1;
                }
                if (location_x - 1 >= 0 && board[location_z + 1, location_x - 1] > 7)//if the piece diagnally in front of it is black
                {
                    moves[1, 0] = location_x - 1;
                    moves[1, 1] = location_z + 1;
                }
                if (location_x + 1 <= 7 && board[location_z + 1, location_x + 1] > 7 )
                {
                    moves[2, 0] = location_x + 1;
                    moves[2, 1] = location_z + 1;
                }
                if (board[location_z + 2, location_x] == 0)
                {
                    moves[3, 0] = location_x;
                    moves[3, 1] = location_z + 2;
                }
                return moves;

            }
            else if(location_z >=2 && location_z<7)
            {
                if (board[location_z + 1, location_x] == 0)
                {
                    moves[0, 0] = location_x;
                    moves[0, 1] = location_z + 1;
                }
                if (location_x - 1 >= 0 && board[location_z + 1, location_x - 1] > 7)//if the piece diagnally in front of it is black
                {
                    moves[1, 0] = location_x - 1;
                    moves[1, 1] = location_z + 1;
                }
                if (location_x + 1 <= 7 && board[location_z + 1, location_x + 1] > 7)
                {
                    moves[2, 0] = location_x + 1;
                    moves[2, 1] = location_z + 1;
                }
                return moves;
            }
            else
            {
                return moves;
            }
        }
        else if(current_piece == "White_knight")
        {
            int[,] moves = new int[8, 2];
            moves = NegativeArr(moves);
            //Knights have at most 8 possible moves
            // + oo - 1 in a direction and + or - 2 in another
            if (location_z + 1 <= 7 && location_x + 2 <= 7) {
                if (board[location_z + 1, location_x + 2] == 0 || board[location_z + 1, location_x + 2] > 7)
                {
                    moves[0, 0] = location_x + 2;
                    moves[0, 1] = location_z + 1;
                }
            }
            if (location_z + 1 <= 7 && location_x - 2 >= 0) {
                if (board[location_z + 1, location_x - 2] == 0 || board[location_z + 1, location_x - 2] > 7)
                {
                    moves[1, 0] = location_x - 2;
                    moves[1, 1] = location_z + 1;
                }
            }
            if (location_z - 1 >= 0 && location_x + 2 <= 7) {
                if (board[location_z - 1, location_x + 2] == 0 || board[location_z - 1, location_x + 2] > 7)
                {
                    moves[2, 0] = location_x + 2;
                    moves[2, 1] = location_z - 1;
                }
            }
            if (location_z - 1 >= 0 && location_x - 2 >= 0) {
                if (board[location_z - 1, location_x - 2] == 0 || board[location_z - 1, location_x - 2] > 7)
                {
                    moves[3, 0] = location_x - 2;
                    moves[3, 1] = location_z - 1;
                }
            }
            if (location_z + 2 <= 7 && location_x + 1 <= 7) {
                if (board[location_z + 2, location_x + 1] == 0 || board[location_z + 2, location_x + 1] > 7)
                {
                    moves[4, 0] = location_x + 1;
                    moves[4, 1] = location_z + 2;
                }
            }
            if (location_z + 2 <= 7 && location_x - 1 >= 0) {
                if (board[location_z + 2, location_x - 1] == 0 || board[location_z + 2, location_x - 1] > 7)
                {
                    moves[5, 0] = location_x - 1;
                    moves[5, 1] = location_z + 2;
                }
            }
            if (location_z - 2 >= 0 && location_x + 1 <= 7) {
                if (board[location_z - 2, location_x + 1] == 0 || board[location_z - 2, location_x + 1] > 7)
                {
                    moves[6, 0] = location_x + 1;
                    moves[6, 1] = location_z - 2;
                }
            }
            if (location_z - 2 >= 0 && location_x - 1 >= 0) {
                if (board[location_z - 2, location_x - 1] == 0 || board[location_z - 2, location_x - 1] > 7)
                {
                    moves[7, 0] = location_x - 1;
                    moves[7, 1] = location_z - 2;
                }
            }
            return moves;
        }
        else if (current_piece == "White_bishop")
        {
            int[,] moves = new int[13, 2];
            moves = NegativeArr(moves);
            //So we use 4 while true loops
            int i = location_z;
            int j = location_x;
            int loops = 0;
            while (true)
            {
                i--;
                j++;
                if (i < 0 || j > 7) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }

            i = location_z;
            j = location_x;
            while (true)
            {
                i++;
                j++;
                if (i > 7 || j > 7) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }

            i = location_z;
            j = location_x;
            while (true)
            {
                i++;
                j--;
                if (i > 7 || j < 0) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }
            i = location_z;
            j = location_x;
            while (true)
            {
                i--;
                j--;
                if (i < 0 || j < 0) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if(board[i,j]>0||board[i,j]<=7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }
            return moves;
        }
        else if (current_piece == "White_rook")
        {
            int[,] moves = new int[14, 2];
            moves = NegativeArr(moves);
            //backwards
            int loops = 0;
            int i = location_z;
            while (true)
            {
                i--;
                if (i < 0) { break; }
                if (board[i, location_x] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[i, location_x] > 7)
                    {
                        moves[loops, 0] = location_x;
                        moves[loops, 1] = i;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            //forwards
            i = location_z;
            while (true)
            {
                i++;
                if (i > 7) { break; }
                if (board[i, location_x] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[i, location_x] > 7)
                    {
                        moves[loops, 0] = location_x;
                        moves[loops, 1] = i;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            //right
            i = location_x;
            while (true)
            {
                i++;
                if (i > 7) { break; }
                if (board[location_z, i] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[location_z, i] > 7)
                    {
                        moves[loops, 0] = i;
                        moves[loops, 1] = location_z;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            //left
            i = location_x;
            while (true)
            {
                i--;
                if (i < 0) { break; }
                if (board[location_z, i] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[location_z, i] > 7)
                    {
                        moves[loops, 0] = i;
                        moves[loops, 1] = location_z;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            return moves;
        }
        else if (current_piece == "White_queen")
        {
            int[,] moves = new int[27, 2];
            moves = NegativeArr(moves);

            int loops = 0;
            int i = location_z;
            while (true)
            {
                i--;
                if (i < 0) { break; }
                if (board[i, location_x] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[i, location_x] > 7)
                    {
                        moves[loops, 0] = location_x;
                        moves[loops, 1] = i;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            //forwards
            i = location_z;
            while (true)
            {
                i++;
                if (i > 7) { break; }
                if (board[i, location_x] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[i, location_x] > 7)
                    {
                        moves[loops, 0] = location_x;
                        moves[loops, 1] = i;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            //right
            i = location_x;
            while (true)
            {
                i++;
                if (i > 7) { break; }
                if (board[location_z, i] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[location_z, i] > 7)
                    {
                        moves[loops, 0] = i;
                        moves[loops, 1] = location_z;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            //left
            i = location_x;
            while (true)
            {
                i--;
                if (i < 0) { break; }
                if (board[location_z, i] == 0)
                {
                    moves[loops, 0] = location_x;
                    moves[loops, 1] = i;
                }
                else
                {
                    if (board[location_z, i] > 7)
                    {
                        moves[loops, 0] = i;
                        moves[loops, 1] = location_z;
                        loops++;
                        break;
                    }
                    else { break; }
                }
                loops++;
            }
            i = location_z;
            int j = location_x;
            while (true)
            {
                i--;
                j++;
                if (i < 0 || j > 7) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }

            i = location_z;
            j = location_x;
            while (true)
            {
                i++;
                j++;
                if (i > 7 || j > 7) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }

            i = location_z;
            j = location_x;
            while (true)
            {
                i++;
                j--;
                if (i > 7 || j < 0) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }
            i = location_z;
            j = location_x;
            while (true)
            {
                i--;
                j--;
                if (i < 0 || j < 0) { break; }
                if (board[i, j] == 0)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                }
                else if (board[i, j] > 0 || board[i, j] <= 7)
                {
                    break;
                }
                else if (board[i, j] > 7)
                {
                    moves[loops, 0] = j;
                    moves[loops, 1] = i;
                    loops++;
                    break;
                }
                loops++;
            }
            return moves;
        }
        else if (current_piece == "White_king")
        {
            int[,] moves = new int[8, 2];
            moves = NegativeArr(moves);
            int i, j;

            //Check the top
            i = location_z+1;
            j = location_x-1;
            int loops = 0;
            if (location_z < 7)
            {
                for(int k = 0; k < 3; k++)
                {
                    if(j>=0 && j < 8)
                    {
                        if (board[i, j] == 0 || board[i, j] > 7)
                        {
                            moves[loops, 0] = j;
                            moves[loops, 1] = i;
                            loops++;
                        }
                        j++;
                    }
                }
            }
            //check the bottom 
            i = location_z - 1;
            j = location_x - 1;
            if (location_z > 0)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (j >= 0 && j < 8)
                    {
                        if (board[i, j] == 0 || board[i, j] > 7)
                        {
                            moves[loops, 0] = j;
                            moves[loops, 1] = i;
                            loops++;
                        }
                        j++;
                    }
                }
            }
            //check the left
            if (location_x > 0)
            {
                if(board[location_z,location_x-1]==0 || board[location_z, location_x - 1] > 7)
                {
                    moves[loops, 0] = location_x -1;
                    moves[loops, 1] = location_z;
                    loops++;
                }
            }
            //check the right
            if (location_x < 7)
            {
                if (board[location_z, location_x + 1] == 0 || board[location_z, location_x + 1] > 7)
                {
                    moves[loops, 0] = location_x + 1;
                    moves[loops, 1] = location_z;
                    loops++;
                }
            }
            return moves;
        }
        else
        {
            return default_arr;
        }
    }

    public int [,] NegativeArr(int [,]array)
    {
        int X_length = array.GetLength(0);
        int Y_length = array.GetLength(1);
        for(int i = 0; i < X_length; i++)
        {
            for(int j = 0; j < Y_length; j++)
            {
                array[i, j] = -1;
            }
        }
        return array;
    }
    public void setValidTiles(int[,]moves, TileScript[]Tiles)
    {
        //double foreach?
        int numberOfMoves = moves.GetLength(0);
        foreach (TileScript tile in Tiles) 
        {
            for (int i = 0; i < numberOfMoves; i++) 
            {
                if (moves[i, 0] != -1)
                {
                    if (tile.X_position == moves[i, 0] && tile.Z_position == moves[i, 1])
                    {
                        tile.valid = true;
                    }
                    else
                    {
                        tile.valid = false;
                    }
                }
            }
        }
    }

}