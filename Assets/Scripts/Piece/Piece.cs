using UnityEngine;
using System.Collections.Generic;

public enum ChessPieceType
{
    Rook,
    Knight,
    Bishop
}
public abstract class Piece : MonoBehaviour
{
    public virtual ChessPieceType PieceType { get; protected set; }
    public Tile CurrentTile { get; set; }
    public abstract List<Tile> GetAttackArea(Board board);
    // Check if a target tile is in the attack area
    public bool IsInAttackArea(Tile targetTile, Board board)
    {
        List<Tile> attackArea = GetAttackArea(board);
        return attackArea.Contains(targetTile);
    }

}
