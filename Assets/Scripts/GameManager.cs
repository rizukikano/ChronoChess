using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board board;
    public static GameManager instance;
    private int playerHealth = 3;
    private int playerScore = 0;
    public float placementTimerDuration = 6f;
    public float placementTimer = 0f;
    private bool isPlacementTimerRunning = false;
    public Queue<Piece> pieceQueue = new Queue<Piece>();
    public List<Piece> allPieces = new List<Piece>();
    private List<Piece> placedPieces = new List<Piece>();
    private Tile selectedTile; // Variable to store the selected tile
    // Currently selected piece
    public Piece currentSelectedPiece;

    private List<Tile> potentialAttackTiles = new List<Tile>(); // List to store potential Attack tiles

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        isPlacementTimerRunning = true;
        UIManager.instance.UpdateScore(playerScore);
        InitializePieceQueue();
        SetCurrentSelectedPiece(pieceQueue.Dequeue());
    }
    void InitializePieceQueue()
    {
        while (pieceQueue.Count < 4)
        {
            AddRandomPieceToQueue();
        }
        UIManager.instance.SetQueueUI(pieceQueue);
    }
    void CheckPieceQueue(){
        if (pieceQueue.Count < 4)
        AddRandomPieceToQueue();
        UIManager.instance.SetQueueUI(pieceQueue);
    }
    void AddRandomPieceToQueue()
    {
        Piece randomPiece = GetRandomPiece();
        pieceQueue.Enqueue(randomPiece);
    }
    Piece GetRandomPiece()
    {
        // Get a random index within the range of the list
        int randomIndex = Random.Range(0, allPieces.Count);

        // Return the piece at the random index
        return allPieces[randomIndex];
    }
    public void SetCurrentSelectedPiece(Piece piece)
    {
        currentSelectedPiece = piece;
    }
    
    private Piece DequeuePiece()
    {
        if (pieceQueue.Count > 0)
        {
            Piece dequeuedPiece = pieceQueue.Dequeue();
            return dequeuedPiece;
        }

        return null; // Return null if the queue is empty
    }
    public void PlaceCurrentPieceOnTile(Tile clickedTile)
    {
        if (currentSelectedPiece != null && isPlacementTimerRunning)
        {
            PlacePiece(clickedTile);
            currentSelectedPiece = DequeuePiece(); // Set the new currentSelectedPiece from the queue
        }
    }

    public void ResetPlacementTimer()
    {
        placementTimer = 0f;
        isPlacementTimerRunning = true;
        if (playerHealth <= 0)
        {
            // Stop the timer when the game is over
            isPlacementTimerRunning = false;
        }
    }
    private void OnPlacementTimerComplete()
    {
        HandleReduction();
        currentSelectedPiece = DequeuePiece();
    }

    void  Update()
    {
        if (isPlacementTimerRunning)
        {
            placementTimer += Time.deltaTime;
            UIManager.instance.UpdateRadialTimer(placementTimer,placementTimerDuration);

            if (placementTimer >= placementTimerDuration)
            {
                OnPlacementTimerComplete();

                // Reset the timer
                ResetPlacementTimer();

            }
        }
    }
    public void TileSelected(Tile tile)
    {

        if (selectedTile != null)
        {
            ClearPotentialAttackTiles();
        }
        selectedTile = tile;

        if (selectedTile != null && !selectedTile.IsOccupied && currentSelectedPiece !=null)
        {
            potentialAttackTiles = currentSelectedPiece.GetAttackArea(board);
            HighlightPotentialAttackTiles();
        }
    }
    void HighlightPotentialAttackTiles()
    {
        foreach (var tile in potentialAttackTiles)
        {
            if (tile != null)
            {
                tile.Highlight();
            }
        }
    }
    void ClearPotentialAttackTiles()
    {
        foreach (var tile in potentialAttackTiles)
        {
            if (tile != null)
            {
                tile.Unhighlight(); 
            }
        }

        potentialAttackTiles.Clear();
    }

    public void PlacePiece(Tile clickedTile)
    {
        if (IsValidPlacement(clickedTile) && IsMoveValid())
        {
            // Place the piece on the board
            Piece newPiece = InstantiatePiece(clickedTile);
            clickedTile.PlacePiece(newPiece);
            // Set the tile as occupied
            newPiece.CurrentTile.IsOccupied = true;
            // Add the piece to the list of placed pieces
            placedPieces.Add(newPiece);
            CheckSpecialConditions(newPiece);
            // Check if the queue has fewer than 4 pieces
            CheckPieceQueue();
        }
        else
        {
            HandleReduction();
        }
    }
    public void HandleReduction()
    {
        playerHealth--;
        UIManager.instance.HandleInvalidUI();
        UIManager.instance.UpdateHealthUI(playerHealth);
        CheckPieceQueue();
        CheckPlayerHealth();
    }

    private void CheckPlayerHealth()
    {
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        SetHighScore();
        UIManager.instance.ShowGameOver(playerScore);
    }
    private bool IsMoveValid()
    {
        // Check if any potential attack tiles are occupied by another piece
        foreach (var tile in potentialAttackTiles)
        {
            if (tile.IsOccupied)
            {
                // Invalid move if the tile is occupied
                return false;
            }
        }

        // Move is valid if no potential attack tiles are occupied
        return true;
    }


    bool IsValidPlacement(Tile clickedTile)
    {
        if (clickedTile == null || clickedTile.IsOccupied)
        {
            return false;
        }

        // Check if the tile is in the attack area of another piece
        foreach (var placedPiece in placedPieces)
        {
            if (placedPiece != null && placedPiece.IsInAttackArea(clickedTile,board))
            {
                return false;
            }
        }

        return true; // Valid placement
    }
    public bool IsInAttackArea(Tile targetTile)
    {
        foreach (var placedPiece in placedPieces)
        {
            if (placedPiece.IsInAttackArea(targetTile, board))
            {
                return true; // Target tile is in the attack area of at least one piece
            }
        }

        return false; // Target tile is not in the attack area of any piece
    }

    void CheckSpecialConditions(Piece piece)
    {
        // Check if the player has placed 4 pieces of the same type
        int sameTypeCount = 0;
        foreach (var placedPiece in placedPieces)
        {
            if (placedPiece.PieceType == piece.PieceType)
            {
                sameTypeCount++;
            }
        }

        if (sameTypeCount >= 4)
        {
            // Remove the 4 pieces of the same type
            RemoveSameTypePieces(piece.PieceType);

            // Increase the player's score based on the type of pieces lost
            IncreaseScore(piece.PieceType);
            
        }
    }

    void RemoveSameTypePieces(ChessPieceType pieceType)
    {
        // Remove 4 pieces of the same type from the board and the list of placed pieces
        List<Piece> toRemove = new List<Piece>();
        foreach (var placedPiece in placedPieces)
        {
            if (placedPiece.PieceType == pieceType && toRemove.Count < 4)
            {
                toRemove.Add(placedPiece);
                placedPiece.CurrentTile.IsOccupied = false;
            }
        }

        foreach (var pieceToRemove in toRemove)
        {
            placedPieces.Remove(pieceToRemove);
            Destroy(pieceToRemove.gameObject);
        }
    }

    void IncreaseScore(ChessPieceType pieceType)
    {
        switch (pieceType)
        {
            case ChessPieceType.Rook:
                playerScore += 5;
                break;
            case ChessPieceType.Knight:
                playerScore += 10;
                break;
            case ChessPieceType.Bishop:
                playerScore += 12;
                break;
        }
        UIManager.instance.UpdateScore(playerScore);

    }

    private Piece InstantiatePiece(Tile currentTile)
    {
        if (currentSelectedPiece != null)
        {
            Piece piece = Instantiate(currentSelectedPiece, currentTile.transform.position, Quaternion.identity);
            // Set the current tile of the instantiated piece
            piece.CurrentTile = currentTile;
            return piece;
        }

        Debug.LogError("CurrentSelectedPiece is null. Make sure to assign a valid piece.");
        return null;
    }

    public void SetHighScore()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if(playerScore > currentHighScore){
            PlayerPrefs.SetInt("HighScore",playerScore);
            PlayerPrefs.Save();
        }
    }
    

}
