using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public bool IsOccupied { get; set; }
    public Piece CurrentPiece { get; private set; }
    public Vector2Int BoardPosition { get; private set; }
    private GameManager gameManager;

    void Awake()
    {
        IsOccupied = false;
        CurrentPiece = null;
        
    }
    void Start()
    {
        gameManager = GameManager.instance;
    }
    public void Init(bool isOffset,Vector2Int boardPosition)
    {
        BoardPosition = boardPosition;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        Highlight();
        if (gameManager.currentSelectedPiece != null)
        {
            // Update the currentTile of the selected piece to this tile
            gameManager.currentSelectedPiece.CurrentTile = this;
        }
        GameManager.instance.TileSelected(this);
        
    }
    
    void OnMouseDown()
    {
        if (!IsOccupied)
        {
            gameManager.ResetPlacementTimer();
            gameManager.PlaceCurrentPieceOnTile(this);
        }
    }

    void OnMouseExit()
    {
        Unhighlight();
    }
    public void Highlight(){
        _highlight.SetActive(true);
    }
    public void Unhighlight(){
        _highlight.SetActive(false);
    }
    public void PlacePiece(Piece piece)
    {
        // Check if the tile is not occupied before placing the piece
        if (!IsOccupied)
        {
            // Set the piece's position to the tile's position
            piece.transform.position = transform.position;

            // Set the piece's current tile reference
            piece.CurrentTile = this;
        }
        else
        {
            Debug.LogWarning("Tile is already occupied.");
        }
    }
    


}
