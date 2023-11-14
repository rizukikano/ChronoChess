using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public Knight()
    {
        PieceType = ChessPieceType.Knight;
    }
    public override List<Tile> GetAttackArea(Board board)
    {
        List<Tile> attackArea = new List<Tile>();

        AddLShapedMoves(attackArea, board, 2, 1);
        AddLShapedMoves(attackArea, board, 2, -1);
        AddLShapedMoves(attackArea, board, -2, 1);
        AddLShapedMoves(attackArea, board, -2, -1);
        AddLShapedMoves(attackArea, board, 1, 2);
        AddLShapedMoves(attackArea, board, 1, -2);
        AddLShapedMoves(attackArea, board, -1, 2);
        AddLShapedMoves(attackArea, board, -1, -2);

        return attackArea;
    }

    private void AddLShapedMoves(List<Tile> tiles, Board board, int xOffset, int yOffset)
    {
        AddTileIfValid(tiles, board, CurrentTile.BoardPosition.x + xOffset, CurrentTile.BoardPosition.y + yOffset);
        AddTileIfValid(tiles, board, CurrentTile.BoardPosition.x - xOffset, CurrentTile.BoardPosition.y + yOffset);
    }

    private void AddTileIfValid(List<Tile> tiles, Board board, int x, int y)
    {
        Tile tile = board.GetTileAtPosition(new Vector2(x, y));
        if (tile != null)
        {
            tiles.Add(tile);
        }
    }
}
