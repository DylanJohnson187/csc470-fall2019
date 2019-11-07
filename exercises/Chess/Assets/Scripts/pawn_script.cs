using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn_script : ChessPiece
{
    //Okay so we need to remember that x is going across and z is going left right
    public override bool[,] possibleMove()
    {
        bool[,] r = new bool[8, 8];
        //Pawn Move logic here
        ChessPiece c, c2;
        //first deal with white moves
        
        if (isWhite)
        {
            Debug.Log("Current X is: " + CurrentX + "Current Y is: " + CurrentY);
            //Check for enemies for attack postion
            if(CurrentX!= 0 && CurrentY != 7)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX - 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX - 1, CurrentY + 1] = true;
            }
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX + 1, CurrentY + 1] = true;
            }
            //standard forward
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY + 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY + 1] = true;
                    Debug.Log("GOT HERE!!!");
                }
            }
            //double jump
            if (CurrentY == 1)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.ChessPieces[CurrentX, CurrentY + 2];
                if(c ==null && c2 == null)
                {
                    r[CurrentX, CurrentY + 2] = true;
                }
            }
        }
        else                            //Black Pawn
        {
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    r[CurrentX - 1, CurrentY - 1] = true;
            }
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    r[CurrentX + 1, CurrentY - 1] = true;
            }
            //standard forward
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY - 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY - 1] = true;
                }
            }
            //double jump
            if (CurrentY == 6)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY - 1];
                c2 = BoardManager.Instance.ChessPieces[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                {
                    r[CurrentX, CurrentY - 2] = true;
                }
            }
        }
        return r;
    }
}
