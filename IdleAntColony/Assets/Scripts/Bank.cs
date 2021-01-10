using UnityEngine;

[CreateAssetMenu]
public class Bank : ScriptableObject
{
    [Tooltip("The amount of gold that earned for each piece of food.")] [SerializeField]
    private int foodPrice = 5;

    [SerializeField] private int goldAccumulated;

    public void ExchangeFoodPiece(int pieceAmount)
    {
        goldAccumulated += foodPrice * pieceAmount;
    }
}