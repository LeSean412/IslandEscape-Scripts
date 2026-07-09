using UnityEngine;

public class BoatPiece : MonoBehaviour
{
    public string pieceName = "Sail";
    public int pieceID;
    public bool isCollected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            Debug.Log("Collected Boat Piece: " + pieceName);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectBoatPiece(pieceName);
            }

            gameObject.SetActive(false);
        }
    }
}
