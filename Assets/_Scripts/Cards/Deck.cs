using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for manageing the cards of the player
/// </summary>
public class Deck : MonoBehaviour
{
    [Header("Cards")]
    public GameObject cardFramePrefab;

    [Header("References")]
    public PlayerHandler playerReference;

    [SerializeField] private SO_Pawn[] _characters; // All the characters the player can have
    [SerializeField] private byte _cardsPerHand = 5; // byte to save memory

    void Start()
    {
        FillHand();
    }

    /// <summary>
    /// Fill the UI Hand of the Player
    /// </summary>
    public void FillHand()
    {
        byte childCount = (byte)transform.childCount;
        for(byte i = childCount; i < _cardsPerHand; i++)
        {
            AddCardToHand();
        }
    }


    /// <summary>
    /// Add one card to the Hand of the Player
    /// </summary>
    public void AddCardToHand()
    {
        SO_Pawn randomPawn = GetRandomPawn();
        GameObject goCard = Instantiate(cardFramePrefab, transform);

        if(goCard.TryGetComponent<Card>(out Card card))
        {
            card.playerReference = playerReference;
            card.so_character = randomPawn;
            card.SetUp();
        }
    }

    /// <summary>
    /// Get A Random Pawn from the SO_Pawn List
    /// </summary>
    /// <returns>SO_Pawn character</returns>
    private SO_Pawn GetRandomPawn()
    {
        return _characters[Random.Range(0, _characters.Length)];
    }
}
