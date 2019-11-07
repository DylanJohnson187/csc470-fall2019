using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop_script : ChessPiece
{
   public override bool[,] possibleMove()
    {
        bool[,] r = new bool[8, 8];
        ChessPiece c;
        int i, j;

        //diagnol left forward
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if(i<0 || j >= 8) { break; }

            c = BoardManager.Instance.ChessPieces[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if(isWhite != c.isWhite)
                {
                    r[i, j] = true;
                    
                }
                break;
            }
        }
        //diagnolly right forward
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >=8 || j >= 8) { break; }

            c = BoardManager.Instance.ChessPieces[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite)
                {
                    r[i, j] = true;
                    
                }
                break;
            }
        }
        //diagnolly left backwards
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i <0 || j < 0) { break; }

            c = BoardManager.Instance.ChessPieces[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite)
                {
                    r[i, j] = true;
                    
                }
                break;
            }
        }
        //diagnolly left backwards
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >=8 || j < 0) { break; }

            c = BoardManager.Instance.ChessPieces[i, j];
            if (c == null)
            {
                r[i, j] = true;
            }
            else
            {
                if (isWhite != c.isWhite)
                {
                    r[i, j] = true;
                }
                break;
            }
        }

        return r;
    }
}
