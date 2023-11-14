using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public Bishop()
    {
        PieceType = ChessPieceType.Bishop;
    }

    // Get the attack area for the bishop
    public override List<Tile> GetAttackArea(Board board)
    {
        List<Tile> attackArea = new List<Tile>();

        // Attack diagonally (top-left to bottom-right)
        AddDiagonalAttackTiles(attackArea, board, 1, 1);
        // Attack diagonally (top-right to bottom-left)
        AddDiagonalAttackTiles(attackArea, board, 1, -1);
        // Attack diagonally (bottom-left to top-right)
        AddDiagonalAttackTiles(attackArea, board, -1, 1);
        // Attack diagonally (bottom-right to top-left)
        AddDiagonalAttackTiles(attackArea, board, -1, -1);

        return attackArea;
    }

    private void AddDiagonalAttackTiles(List<Tile> attackArea, Board board, int xOffset, int yOffset)
    {
        Vector2Int currentPos = CurrentTile.BoardPosition;

        for (int i = 1; i < board.boardSize.x; i++)
        {
            int x = currentPos.x + i * xOffset;
            int y = currentPos.y + i * yOffset;

            if (IsInBoardBounds(board, x, y))
            {
                Tile tile = board.GetTileAtPosition(new Vector2(x, y));
                attackArea.Add(tile);
            }
            else
            {
                break;
            }
        }
    }

    private bool IsInBoardBounds(Board board, int x, int y)
    {
        return x >= 0 && x < board.boardSize.x && y >= 0 && y < board.boardSize.y;
    }
}
