using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object Pawn used for managing the stats of each character.
/// </summary>
[CreateAssetMenu(menuName = "Pawn")]
public class SO_Pawn : ScriptableObject
{
    [System.Flags]
    public enum PlaceableSlot // Starting from 1 because 0 is for Nothing and not using 3 because 3 is for everything (bitwise operation)
    { 
        BOT = 1,
        MIDDLE = 2,
        TOP = 4
    }

    [System.Flags]
    public enum SlotType
    {
        GRASS = 1,
        WATER = 2
    }

    public Sprite cardIcon;
    public GameObject characterPrefab;
    public int hp, dmg, mana;
    public SlotType slotType;
    public PlaceableSlot placeInSlot;
}
