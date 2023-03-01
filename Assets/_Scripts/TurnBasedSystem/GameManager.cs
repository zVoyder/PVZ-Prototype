using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// State Machine Singleton class used for managing the turns and the status of the battlefield
/// between the Player Contender and the Enemy Contender
/// </summary>
public class GameManager : MonoBehaviour
{
    // Turn states with the Z rotation of the button associated.
    public enum TurnState { ZOMBIE = 45, PLANTS = 0, FIGHT = -45} 
    // LoopList used for looping the turns.
    public static LoopList<TurnState> turns = new LoopList<TurnState>( new List<TurnState>() { TurnState.ZOMBIE , TurnState.PLANTS , TurnState.FIGHT } );
    public static GameManager instance;

    [Header("Handlers")]
    public PlayerHandler playerHandler;
    public Deck playerDeck;
    public EnemyAI enemyAI;
    //public AI zombie;

    [Header("UI")]
    public RectTransform turnButton;
    public GameObject plantsWonPanel;
    public GameObject zombiesWonPanel;

    [Header("Field")]
    public int numberOfColumns = 4;
    public float timeForEachColumnPhase = 2.0f; // How long does it take a column of entities to act.

    public Columns<Entity> EntitiesOnField { get => _entitiesOnField; }

    private Columns<Entity> _entitiesOnField;

    void Awake()
    {
        turns.Reset(); // Reset the turns, so when it load again the scene it wont start from an other turn.

        if(instance == null) // Singleton Pattern (this if is not necessary, just used to avoid bugs in future, if i will modify this script). 
        {
            instance = this;
            _entitiesOnField = new Columns<Entity>(numberOfColumns);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GoNextTurn();
    }

    /// <summary>
    /// Go to the Next Turn
    /// </summary>
    public void GoNextTurn()
    {
        TurnState currentTurn = turns.Next(); // Get the current turn and move the index of the turn one forward.

        SetButtonTurnRotation((float)currentTurn); // Set the button rotation based on the turn.
#if DEBUG
        Debug.Log($"Current Turn {currentTurn}");
#endif
        switch (currentTurn)
        {
            case TurnState.ZOMBIE:
                StartCoroutine(ZombiesTurn());
                break;

            case TurnState.PLANTS:
                PlantsTurn();
                break;

            case TurnState.FIGHT:
                StartCoroutine(FightRoutine());
                break;
        }
    }


    #region TurnsSetup

    /// <summary>
    /// Begin Plants Turn
    /// </summary>
    private void PlantsTurn()
    {
        playerDeck.AddCardToHand();
        playerHandler.IncreaseMaxMana();
        playerHandler.FillMana();
        playerHandler.BeginTurn(); // The EndTurn will be called at the end of BeginTurn
    }

    /// <summary>
    /// Begin Zombies Turn Routine
    /// </summary>
    /// <returns>IEnumerator EnemyAI.BeginTurn()</returns>
    private IEnumerator ZombiesTurn()
    {
        enemyAI.AddCardToHand();
        enemyAI.IncreaseMaxMana();
        enemyAI.FillMana();
        yield return enemyAI.BeginTurn();
        GoNextTurn();
    }

    /// <summary>
    /// Fight Routine
    /// </summary>
    /// <returns>IEnumerator Time For Each Column Phase</returns>
    private IEnumerator FightRoutine()
    {
        for(int k = 0; k < _entitiesOnField.Lenght; k++)
        {
            ColumnBeginPhase(k);
            yield return new WaitForSeconds(timeForEachColumnPhase);
        }


        GoNextTurn();
    }

    /// <summary>
    /// Trigger all the Begin Phase methods of a column of entities
    /// </summary>
    /// <param name="index">index of the column</param>
    private void ColumnBeginPhase(int index)
    {
        Column<Entity> column = _entitiesOnField.GetColumnByIndex(index);

        for (int i = 0; i < column.Lenght; i++)
        {
            column.GetByIndex(i).BeginPhase();
        }
    }

    #endregion

    #region EntitiesManagement
    /// <summary>
    /// Add an entity to a column
    /// </summary>
    /// <param name="ent">Entity you want to add</param>
    /// <param name="index">index of the column you want to add the Entity in</param>
    public void AddEntity(Entity ent, int index)
    {
        _entitiesOnField.AddToColumn(ent, index);
    }

    /// <summary>
    /// Remove an entity to a column
    /// </summary>
    /// <param name="ent">Entity you want to remove</param>
    /// <param name="index">index of the column you want to remove the Entity from</param>s
    public void RemoveEntity(Entity ent, int index)
    {
        _entitiesOnField.RemoveFromColumn(ent, index);
    }
    #endregion

    #region GAMEOVER

    /// <summary>
    /// Trigger the Game Over
    /// </summary>
    private void GameOver()
    {
        StopAllCoroutines();
        playerHandler.EndTurn();
        enemyAI.EndTurn();
    }

    /// <summary>
    /// Game Over Plants Won (Player Won)
    /// </summary>
    public void PlantsWon()
    {
#if DEBUG
        Debug.Log($"Player WON.");
#endif
        GameOver();
        plantsWonPanel.SetActive(true);
    }

    /// <summary>
    /// Game Over Zombies Won (Enemy Won)
    /// </summary>
    public void ZombiesWon()
    {
#if DEBUG
        Debug.Log($"Zombies WON.");
#endif
        GameOver();
        zombiesWonPanel.SetActive(true);
    }

    #endregion

    #region UI

    /// <summary>
    /// Set the button of the turns to a rotation
    /// </summary>
    /// <param name="zRotation">Rotation Z you want to rotate the button</param>
    private void SetButtonTurnRotation(float zRotation)
    {
        turnButton.rotation = Quaternion.Euler(new Vector3(turnButton.rotation.x, turnButton.rotation.y, zRotation));
    }

    #endregion
}
