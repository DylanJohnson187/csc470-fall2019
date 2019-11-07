using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight_script : ChessPiece
{
    

    public override bool[,] possibleMove()
    {
        bool[,] r = new bool[8, 8];
        //Eight night moves
        int x = CurrentX;
        int y = CurrentY;
        //up left
        KnightMoves(x - 1, y + 2, ref r);
        //up right
        KnightMoves(x + 1, y + 2, ref r);
        //left up
        KnightMoves(x - 2, y + 1, ref r);
        //right up
        KnightMoves(x + 2, y + 1, ref r);
        //down left
        KnightMoves(x - 1, y - 2, ref r);
        //down right
        KnightMoves(x + 1, y - 2, ref r);
        //left down
        KnightMoves(x - 2, y - 1, ref r);
        //right down
        KnightMoves(x + 2, y - 1, ref r);
        return r;
    }
    public void KnightMoves(int x, int y, ref bool[,] r)
    {
        ChessPiece c;
        if (x >=0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.ChessPieces[x, y];
            if (c == null)
            {
                r[x, y] = true;
            }
            else if(isWhite != c.isWhite)
            {
                r[x, y] = true;
            }
        }
    }
}
