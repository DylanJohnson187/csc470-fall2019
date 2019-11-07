using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King_script : ChessPiece
{
   public override bool[,] possibleMove()
   {
        bool[,] r = new bool[8, 8];
        ChessPiece c;
        int i, j;
        //checks 3 forward spaces
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for(int h = 0; h < 3; h++)
            {
                if(i>0 || i < 8)
                {
                    c = BoardManager.Instance.ChessPieces[i, j];
                    if (c == null)
                    {
                        r[i, j] = true;
                    }
                    else if (isWhite != c.isWhite)
                    {
                        r[i, j] = true;
                    }
                }
                i++;
            }
        }
        //checks 3 backward spaces
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int h = 0; h < 3; h++)
            {
                if (i > 0 || i < 8)
                {
                    c = BoardManager.Instance.ChessPieces[i, j];
                    if (c == null)
                    {
                        r[i, j] = true;
                    }
                    else if (isWhite != c.isWhite)
                    {
                        r[i, j] = true;
                    }
                }
                i++;
            }
        }
        //left
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.ChessPieces[CurrentX-1, CurrentY];
            if (c == null)
            {
                r[CurrentX-1, CurrentY] = true;
            }
            else if (isWhite != c.isWhite)
            {
                r[CurrentX-1, CurrentY] = true;
            }
        }

        //Right
        if (CurrentX != 7)
        {
            c = BoardManager.Instance.ChessPieces[CurrentX + 1, CurrentY];
            if (c == null)
            {
                r[CurrentX + 1, CurrentY] = true;
            }
            else if (isWhite != c.isWhite)
            {
                r[CurrentX + 1, CurrentY] = true;
            }
        }
        return r;
   }
}
