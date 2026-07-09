using UnityEngine;
using System.Collections.Generic;

public class BoatManager : MonoBehaviour
{
    private List<BoatPiece> boatPieces = new List<BoatPiece>();

    public void CollectBoatPiece(BoatPiece piece)
    {
        if (!piece.isCollected)
        {
            piece.isCollected = true;
            boatPieces.Add(piece);
            Debug.Log("Boat Piece Collected: " + piece.pieceName);
        }
    }

    public void AssembleBoat()
    {
        if (boatPieces.Count >= 5) // Change this number based on how many pieces are required
        {
            Debug.Log("All parts collected! Assembling the boat!");
            // Logic for assembling the boat
            // Here, instantiate the boat prefab, etc.
        }
        else
        {
            Debug.Log("Not enough pieces to assemble the boat.");
        }
    }
}
