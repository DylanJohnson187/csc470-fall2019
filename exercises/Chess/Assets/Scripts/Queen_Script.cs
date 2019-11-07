using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen_Script : ChessPiece
{
  public override bool[,] possibleMove()
    {
        //table setting 
        bool[,] r = new bool[8, 8];
        ChessPiece c;
        int i,j;
        //first rook stuff just copy paste from rook
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }
            c = BoardManager.Instance.ChessPieces[i, CurrentY];
            if (c == null)
            {
                r[i, CurrentY] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[i, CurrentY] = true;
                }
                break;
            }
        }
        //left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
            {
                break;
            }
            c = BoardManager.Instance.ChessPieces[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;
                break;
            }
        }
        //forward
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }
            c = BoardManager.Instance.ChessPieces[CurrentX, i];
            if (c == null)
            {
                r[CurrentX, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[CurrentX, i] = true;
                }
                break;
            }
        }
        //backwards
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
            {
                break;
            }
            c = BoardManager.Instance.ChessPieces[CurrentX, i];
            if (c == null)
            {
                r[CurrentX, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    r[CurrentX, i] = true;
                }
                break;
            }
        }
        //Then do bishop stuff
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8) { break; }

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
        //diagnolly right forward
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8) { break; }

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
            if (i < 0 || j < 0) { break; }

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
            if (i >= 8 || j < 0) { break; }

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
