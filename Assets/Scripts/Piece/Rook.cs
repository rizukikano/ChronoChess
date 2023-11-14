using UnityEngine;
using System.Collections.Generic;

public class Rook : Piece
{
    public Rook()
    {
        PieceType = ChessPieceType.Rook;
    }

    public override List<Tile> GetAttackArea(Board board)
    {
        List<Tile> attackArea = new List<Tile>();

        // Check horizontally
        for (int x = 0; x < board.boardSize.x; x++)
        {
            if (x != CurrentTile.BoardPosition.x)
            {
                attackArea.Add(board.GetTileAtPosition(new Vector2(x, CurrentTile.BoardPosition.y)));
            }
        }

        // Check vertically
        for (int y = 0; y < board.boardSize.y; y++)
        {
            if (y != CurrentTile.BoardPosition.y)
            {
                attackArea.Add(board.GetTileAtPosition(new Vector2(CurrentTile.BoardPosition.x, y)));
            }
        }

        return attackArea;
    }
}
