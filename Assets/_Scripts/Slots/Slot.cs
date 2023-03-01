using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class used for managing the slot of the contenders.
/// </summary>
public class Slot : MonoBehaviour
{
    public int columnIndex = 0;

    private bool _isOccupied = false;
    public bool IsOccupied { get => _isOccupied; }

    public SO_Pawn.SlotType slotType;
    public SO_Pawn.PlaceableSlot slotRow;

    /// <summary>
    /// Set the slot to occupied or not occupied
    /// </summary>
    /// <param name="b">True if it is occupied, False if not.</param>
    public virtual void SetOccupied(bool b)
    {
        _isOccupied = b;
    }

    /// <summary>
    /// Check the slot type
    /// </summary>
    /// <param name="pawn">SO_Pawn character you want to check with</param>
    /// <returns>True if the character is a character has the ability to stay in this slot, False if not</returns>
    public bool CheckSlotType(SO_Pawn pawn)
    {
        return (pawn.slotType & slotType) != 0;
    }

    /// <summary>
    /// Check the Slot Row of this slot compared to a SO_Pawn character one
    /// </summary>
    /// <param name="pawn">SO_Pawn character you want to check with</param>
    /// <returns>True if the character has the ability to stay on this slot row, False if not</returns>
    public bool CheckSlotRow(SO_Pawn pawn)
    {
        return (pawn.placeInSlot & slotRow) != 0;
    }
}
