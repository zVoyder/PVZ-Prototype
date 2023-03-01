using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// This script manage the behaviour of the AI playing with its hand of characters.
/// </summary>
public class EnemyAI : Contender // Extending the class contender to manage its statistics.
{
    [Header("Deck")]
    [SerializeField] private SO_Pawn[] _characters;
    [SerializeField] private byte _cardsPerHand = 5;

    [Header("Slots")]
    public float spawnPawnEveryNseconds = 0.5f;
    [SerializeField] private Columns<Slot> _mySlots;
    

    private List<SO_Pawn> _currentHand = new List<SO_Pawn>(); // its hand

    private void Start()
    {
        FillAll();
        FillHand();
    }

    public new IEnumerator BeginTurn()
    {
        base.BeginTurn();
        yield return SpawnCharacters();
    }

    public override void EndTurn()
    {
        base.EndTurn();
    }

    #region GAMEMANGER CALLS

    public override void Death()
    {
        base.Death();
        GameManager.instance.PlantsWon(); // Singleton Call to decleare the plants win.
    }

    #endregion

    #region Hand

    /// <summary>
    /// Fill the Hand of the AI
    /// </summary>
    public void FillHand()
    {
        for (int i = 0; i < _cardsPerHand; i++)
            AddCardToHand();
    }

    /// <summary>
    /// Add one card to the hand of the AI
    /// </summary>
    public void AddCardToHand()
    {
        _currentHand.Add(_characters[Random.Range(0, _characters.Length)]);
    }

    /// <summary>
    /// Remove / Use a character from its hand
    /// </summary>
    /// <param name="character"></param>
    private void RemoveFromHand(SO_Pawn character)
    {
        _currentHand.Remove(character);
    }

    #endregion

    #region Spawning

    /// <summary>
    /// Spawn characters of its hand in the field in the correct positions
    /// </summary>
    /// <returns>IEnumerator Time for each spawn.</returns>
    private IEnumerator SpawnCharacters()
    {
        for (int i = 0; i < _currentHand.Count; i++ )
        {
            SO_Pawn character = _currentHand[i];

            if (HaveManaToSpawnPawn(character))
            {
                if (FindEmptySlot(out Slot slot, character))
                {
                    SpawnInSlot(slot, character);
                }
            }

            yield return new WaitForSeconds(spawnPawnEveryNseconds);
        }

        EndTurn();
    }

    /// <summary>
    /// Spawn a character in a slot.
    /// </summary>
    /// <param name="slot">Slot you want the SO_Pawn character to spawn.</param>
    /// <param name="pawn">The SO_Pawn character you want to spawn.</param>
    private void SpawnInSlot(Slot slot, SO_Pawn pawn)
    {
        GameObject goCharacter = Instantiate(pawn.characterPrefab, slot.transform.position, Quaternion.identity);

        if (goCharacter.TryGetComponent(out Entity ent))
        {
            ent.Init(pawn.hp, pawn.dmg, slot);
        }

        ConsumeMana(pawn.mana);
        RemoveFromHand(pawn);
    }

    #endregion

    #region Finders

    /// <summary>
    /// Found a correct and empy slot for a character.
    /// </summary>
    /// <param name="foundSlot">out of the found slot.</param>
    /// <param name="character">character you want to spawn.</param>
    /// <returns>True if was a slot was found, False otherwise.</returns>
    private bool FindEmptySlot(out Slot foundSlot, SO_Pawn character)
    {  
        for (int i = 0; i < _mySlots.Lenght; i++)
        {
            Column<Slot> slots = _mySlots.GetColumnByIndex(i);

            for(int k = 0; k < slots.Lenght; k++)
            {
                Slot slot = slots.GetByIndex(k);
                if (!slot.IsOccupied && slot.CheckSlotType(character) && slot.CheckSlotRow(character))
                {
                    foundSlot = slot;
                    return true;
                }
            }
        }

        foundSlot = null;
        return false;
    }

    #endregion
}
