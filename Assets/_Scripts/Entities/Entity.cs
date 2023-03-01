using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Base class script used for the entities (characters) on the field.
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] protected int hitPoints;
    [SerializeField] protected int damagePoints;

    [Header("Texts")]
    public TMP_Text hitPointsText;
    public TMP_Text damagePointsText;

    //private bool _isRunningPhase; // For now it is useless.
    private Slot _associatedSlot;

    /// <summary>
    /// Initialize the entity with the given statistics.
    /// </summary>
    /// <param name="hitPoints">Hit Points of the entity</param>
    /// <param name="damagePoints">Damage Points of the entity</param>
    /// <param name="associatedSlot">Slot that the entity is associated with</param>
    public void Init(int hitPoints, int damagePoints, Slot associatedSlot)
    {
        this.hitPoints = hitPoints;
        this.damagePoints = damagePoints;
        _associatedSlot = associatedSlot;
        _associatedSlot.SetOccupied(true);
        AddToGameManager();

        UpdateAllTexts();
    }


    /// <summary>
    /// Beging Phase of the entity
    /// </summary>
    public virtual void BeginPhase()
    {
        //_isRunningPhase = true;
    }

    /// <summary>
    /// End Phase of the entity
    /// </summary>
    protected virtual void EndPhase()
    {
        //_isRunningPhase = false;
    }

    /// <summary>
    /// Death phase of the Entity (what it does on death)
    /// </summary>
    public virtual void DeathPhase()
    {
        _associatedSlot.SetOccupied(false);
        RemoveFromGameManger();
        Destroy(gameObject);
    }

    /// <summary>
    /// Damage the entity lowering its Hit Points
    /// </summary>
    /// <param name="dmg">Taken damage</param>
    public void GetDamage(int dmg)
    {
        if (hitPoints - dmg > 0)
        {
            hitPoints -= dmg;
        }
        else
        {
            hitPoints = 0;
            DeathPhase();
        }

        UpdateHPText();
    }


    #region UI

    /// <summary>
    /// Update all the TMP_Text texts
    /// </summary>
    private void UpdateAllTexts()
    {
        UpdateHPText();
        UpdateDMGText();
    }

    /// <summary>
    /// Update the TMP_Text of the Hit Points
    /// </summary>
    private void UpdateHPText()
    {
        hitPointsText.text = hitPoints.ToString();
    }

    /// <summary>
    /// Update the TMP_Text of the Damage Points
    /// </summary>
    private void UpdateDMGText()
    {
        damagePointsText.text = damagePoints.ToString();
    }

    #endregion


    #region GameManager

    /// <summary>
    /// Add this entity to the GameManager so it can be managed by it
    /// </summary>
    private void AddToGameManager()
    {
        GameManager.instance.AddEntity(this, _associatedSlot.columnIndex);
    }

    /// <summary>
    /// Remove this entity from the GameManger
    /// </summary>
    private void RemoveFromGameManger()
    {
        GameManager.instance.RemoveEntity(this, _associatedSlot.columnIndex);
    }

    #endregion


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Harm harm))
        {
            GetDamage(harm.Damage);
        }
    }


#if DEBUG
    /// <summary>
    /// Debugging method to trigger the entity's death
    /// </summary>
    [ContextMenu("Die")]
    void DieNow()
    {
        DeathPhase();
    }

    /// <summary>
    /// Debugging method to trigger the entity's begin phase
    /// </summary>
    [ContextMenu("BeginPhase")]
    void Begin()
    {
        BeginPhase();
    }

    /// <summary>
    /// Debugging method to trigger the entity's end phase
    /// </summary>
    [ContextMenu("EndPhase")]
    void End()
    {
        EndPhase();
    }
#endif
}
