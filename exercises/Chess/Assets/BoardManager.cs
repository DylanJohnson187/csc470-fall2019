using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves;
    public ChessPiece[,] ChessPieces { get; set; }      //create matrix of chess piece locations
    private ChessPiece selectedPiece;

    private const float tile_size = 1.0f;
    private const float tile_offset = .5f;              //offset will always be tile size /2

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessPiecesPrefabs;
    private List<GameObject> activeChessPieces;

    public bool WhiteTurn = true;

    public Text text;
    

    private void Start()
    {
        Instance = this;
        SpawnAllChessPieces();
    }
    private void Update()
    {
        updateSelection();
        DrawBoard();

        if (Input.GetMouseButtonDown(0))            //gets input from left click on mouse
        {
            if(selectionX>=0 &&selectionY >= 0)     //so long as selecting on the board
            {
                if (selectedPiece == null)          //nothing currently  selected
                {
                    //So we want to select something
                    SelectPiece(selectionX, selectionY);
                }
                else
                {
                    // A piece is currently selected so we want to move it
                    movePiece(selectionX, selectionY);
                }
            }
        }
        if (WhiteTurn)
        {
            text.text = "White Turn";
        }
        else
        {
            text.text = "Black Turn";
        }
    }

    private void SelectPiece(int x, int y)
    {
        if (ChessPieces[x, y] == null)
            return;
        if (ChessPieces[x, y].isWhite != WhiteTurn)     //trying to pick a different piece than turn
            return;

        bool hasMove = false;

        allowedMoves = ChessPieces[x, y].possibleMove();
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasMove = true;
                }
            }
        }
        if (!hasMove) { return; }
        selectedPiece = ChessPieces[x, y];
        BoardColoring.Instance.HighlightAllowedMoves(allowedMoves);
    }
    private void movePiece(int x, int y)
    {
        if (allowedMoves[x,y])
        {
            ChessPiece c = ChessPieces[x, y];
            //Dealing with captured pieces
            if(c!= null && c.isWhite !=WhiteTurn)
            {
                if (c.GetType() == typeof(King_script)) {
                    //end game
                    endGame();
                    return;
                }
                activeChessPieces.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            ChessPieces [selectedPiece.CurrentX, selectedPiece.CurrentY] = null;
            selectedPiece.transform.position = getTileCenter(x, y);
            selectedPiece.SetPosition(x, y);
            ChessPieces[x, y] = selectedPiece;
            WhiteTurn = !WhiteTurn;         //set the turn to the other player
        }
        BoardColoring.Instance.HideColor();
        selectedPiece = null;               //if you dont try a possible move then the selected piece will now be unselected
        
    }

    private Vector3 getTileCenter(int X, int Y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (tile_size * X) + tile_offset;
        origin.z += (tile_size * Y) + tile_offset;
        return origin;
    }
    private void SpawnChessPieces(int idx, int x, int y)
    {
        /*
         * Given an index to choose from the public prefab list (aka what piece) and a position it will spawn pieces at this locaiton
         * This is going to be called 32 times in a start up function that spawns all
         */
        GameObject go = Instantiate(chessPiecesPrefabs[idx],getTileCenter(x,y),Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        ChessPieces[x, y] = go.GetComponent<ChessPiece>();
        ChessPieces[x, y].SetPosition(x, y);
        activeChessPieces.Add(go);
    }

    private void SpawnAllChessPieces()
    {
        activeChessPieces = new List<GameObject>();
        ChessPieces = new ChessPiece[8, 8];
        //White Team First
        //king
        SpawnChessPieces(0, 3, 0);
        //Queen
        SpawnChessPieces(1, 4, 0);
        //Rooks
        SpawnChessPieces(2, 0, 0);
        SpawnChessPieces(2, 7, 0);
        //knights
        SpawnChessPieces(4, 1, 0);
        SpawnChessPieces(4, 6, 0);
        //Bishops
        SpawnChessPieces(3, 2, 0);
        SpawnChessPieces(3, 5, 0);
        //for loop for pawns
        for(int i = 0; i < 8; i++)
        {
            SpawnChessPieces(5, i, 1);
        }

        //Now the Black Team
        //king
        SpawnChessPieces(6, 3, 7);
        //Queen
        SpawnChessPieces(7, 4, 7);
        //Rooks
        SpawnChessPieces(8, 0, 7);
        SpawnChessPieces(8, 7, 7);
        //bishops
        SpawnChessPieces(10,1, 7);
        SpawnChessPieces(10,6, 7);
        //knights
        SpawnChessPieces(9, 2, 7);
        SpawnChessPieces(9, 5, 7);
        //for loop for pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessPieces(11, i, 6);
        }
    }

    private void updateSelection()
    {
        if (!Camera.main) return;

        // Same RayCast technique for the unit game by the professor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("chessplane"))
            {
                //Debug.Log(hit.point);
                selectionX = (int)hit.point.x;
                selectionY = (int)hit.point.z;
            }
            else
            {
                selectionX = -1;
                selectionY = -1;
            }
        }
    }

    private void DrawBoard()
    {
        /*
         * Creates the board representation so that we can see it
         */
        Vector3 widthOfLine = Vector3.right * 8;        //Creates the width for the board so that 8 tiles will fit
        Vector3 depthLine = Vector3.forward * 8;

        for(int i = 0; i<=8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthOfLine,Color.black, Time.deltaTime) ;
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + depthLine, Color.black, Time.deltaTime);
            }
        }

        //draw selected tiles
        if(selectionX>=0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

        }

    }
    private void endGame()
    {
        if (WhiteTurn)
        {
            Debug.Log("White Won");
            StartCoroutine(endGameText(0));

        }
        else
        {
            Debug.Log("Black Won");
            StartCoroutine(endGameText(1));
        }
        foreach(GameObject go in activeChessPieces)
        {
            Destroy(go);
        }
        WhiteTurn = true;
        BoardColoring.Instance.HideColor();
        SpawnAllChessPieces();
    }
    IEnumerator endGameText(int x)
    {
        if (x == 0)
        {
            text.text = "WHITE WON";
        }
        else
        {
            text.text = "BLACK WON";
        }
        yield return new WaitForSeconds(10);
    }

}
