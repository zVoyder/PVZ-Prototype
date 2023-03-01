using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class used for the slots of the player
/// </summary>
public class PlayerSlot : Slot // Extending the class Slot
{
    public PlayerHandler playerReference;


    public override void SetOccupied(bool b)
    {
        base.SetOccupied(b);
        gameObject.SetActive(!b); // Disable the slot gameobject so it wont be visible as a possible spawn position from the player.
    }


    private void OnMouseEnter()
    {
        if (playerReference.IsDragging) // On Mouse enter check if the player is dragging a card.
        {
            playerReference.SetSlotSpawnPoint(this); // if so set the slot where the character will spawn to this slot.
            playerReference.IsOnSlot = true; // Tell the player is on a slot.
        }
    }

    private void OnMouseExit()
    {
        playerReference.SetSlotSpawnPoint(null); // On Mouse exit the slot wont be this anymore.
        playerReference.IsOnSlot = false; // Tell the player is not on a slot.
    }
}
