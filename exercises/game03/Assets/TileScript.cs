using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour

{

    public int X_position;
    public int Z_position;

    public int occupied;           //coding for this was 0 is niether
    /*OKAY SO THIS IS GOING TO BE A LOT OF TEXT BUT KEEP THIS IN MIND DYLAN YOU FUCKING MORON
     * PAWN UNCHECKED - FOR 2 JUMP START MOVE
     * SAME THING FOR ROOKS FOR CASTLING BUT THIS MIGHT BE HARD TO IMPLEMENT
     0 -- unoccuppied tile
     1 -- WHITE PAWN UNMOVED
     2 -- WHITE PAWN MOVED
     3 -- WHITE ROOK
     4 -- WHITE KNIGHT
     5 -- WHITE BISHOP
     6 -- WHITE QUEEN
     7 -- WHITE KING
     8 -- BLACK PAWN UNMOVED
     9 -- BLACK PAWN MOVED
     10 --BLACK ROOK
     11 --BLACK KNIGHT
     12 --BLACK BISHOP
     13 --BLACK QUEEN
     14 --BLACK KING
     */
    //public int piece_on_tile;      //coding for this will be 0 is nothing 1 is pawn, 2 is rook, 3 is knight, 4 is bishop, 5 is queen, 6 is king

    public Renderer rndr;
    Color defaultColor;
    public Color validMove;
    public bool valid = false;

    // Start is called before the first frame update
    void Start()
    {
        X_position =  (int) transform.position.x;
        Z_position = (int)transform.position.z;
        defaultColor = rndr.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        setColorForValid();
    }

    /* 
     These triggers do what I want them to do and will update the occupied value for each tile
     ON a piece entering a space the tile will update to the pieces associated tile value
     on a piece leaving said space the tile will then update to 0.
         
    */
    public void OnTriggerStay(Collider other)
    {
        /* Here we need to go through all of the cases 
         0 -- unoccuppied tile
         1 -- WHITE PAWN UNMOVED
         2 -- WHITE PAWN MOVED
         3 -- WHITE ROOK
         4 -- WHITE KNIGHT
         5 -- WHITE BISHOP
         6 -- WHITE QUEEN
         7 -- WHITE KING
         8 -- BLACK PAWN UNMOVED
         9 -- BLACK PAWN MOVED
         10 --BLACK ROOK
         11 --BLACK KNIGHT
         12 --BLACK BISHOP
         13 --BLACK QUEEN
         14 --BLACK KING
         */
        if (other.tag == "White_pawn")
        {
            occupied = 2;
        }
        else if(other.tag == "White_rook")
        {
            occupied = 3;
        }
        else if (other.tag == "White_knight")
        {
            occupied = 4;
        }
        else if (other.tag == "White_bishop")
        {
            occupied = 5;
        }
        else if (other.tag == "White_queen")
        {
            occupied = 6;
        }
        else if (other.tag == "White_king")
        {
            occupied = 7;
        }
        else if (other.tag == "Black_pawn")
        {
            occupied = 9;
        }
        else if (other.tag == "Black_rook")
        {
            occupied = 10;
        }
        else if (other.tag == "Black_knight")
        {
            occupied = 11;
        }
        else if (other.tag == "Black_bishop")
        {
            occupied = 12;
        }
        else if (other.tag == "Black_queen")
        {
            occupied = 13;
        }
        else if (other.tag == "Black_king")
        {
            occupied = 14;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        //set occupied to 0
        occupied = 0;
    }

    public void setColorForValid()
    {
        if (valid)
        {
            rndr.material.color = validMove;
        }
        else
        {
            rndr.material.color = defaultColor;
        }
    }

    /*public void setColorforUnitifValid()        
    {
        //This method will take a list of moves from the piece script and will generate 


    }*/

}
