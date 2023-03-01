using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script used for managing the player actions and status.
/// </summary>
public class PlayerHandler : Contender // Extending the class contender to manage its statistics.
{
    [Header("Hand")]
    [Range(0, 100f)] public float dragSpeed = 15f;
    [SerializeField] private Transform _deck;
    [SerializeField] private GridLayoutGroup _grid;

    [Header("Placeholder Slots")]
    [SerializeField] private GameObject _botSlotsLine;
    [SerializeField] private GameObject _middleSlotsLine;
    [SerializeField] private GameObject _topSlotsLine;
    [SerializeField] private GameObject _botWaterSlotsLine;
    [SerializeField] private GameObject _middleWaterSlotsLine;
    [SerializeField] private GameObject _topWaterSlotsLine;

    private bool _canDrag = false;
    private bool _isDragging;
    private bool _isOnSlot;

    private Card _draggedCard;
    private RectTransform _draggedCardTransform;
    private int _cardCurrentIndex;

    private Slot _slot;

    public bool IsOnSlot { get => _isOnSlot; set => _isOnSlot = value; }
    public bool IsDragging { get => _isDragging; }

    private void Start()
    {
        FillAll();
    }

    private void Update()
    {
        if (IsDragging) // Check if is he dragging a card.
        {
            // if so the card will move to the pointer position with Lerp.
            _draggedCardTransform.anchoredPosition = Vector2.Lerp(_draggedCardTransform.anchoredPosition, Input.mousePosition, dragSpeed * Time.deltaTime);

            if (Input.GetButtonUp("Fire1")) // If the player release the card.
            {
                // Check if he has release the card above a slot.
                if (_isOnSlot && !_slot.IsOccupied && HaveManaToSpawnPawn(_draggedCard.so_character))
                {
                    SpawnCharacter(); // if so, spawn a character.
                }
                else
                {
                    ReturnCardToHand(); // otherwise return the card to the player's hand.
                }
            }
        }
    }

    public override void BeginTurn()
    {
        base.BeginTurn();
        _canDrag = true;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        StopDragging();
        _canDrag = false;
    }

    #region GAMEMANGER CALLS

    /// <summary>
    /// End the player turn and call the NextTurn method of the GameManager Singleton.
    /// </summary>
    public void EndTurnAndGoNext()
    {
        if (IsMyTurn) // Do it only if it is the player's turn.
        {
            EndTurn();
            GameManager.instance.GoNextTurn();
        }
    }

    public override void Death()
    {
        base.Death();
        GameManager.instance.ZombiesWon(); // Decleare the Zombies win.
    }

    #endregion

    #region Spawning

    private void SpawnCharacter()
    {
        ///_slot.SetOccupied(true); The Entity will do that

        GameObject character = Instantiate(_draggedCard.so_character.characterPrefab, _slot.transform.position, Quaternion.identity);
        character.transform.SetParent(_slot.transform.root.GetChild(0).transform);

        if (character.TryGetComponent(out Entity ent))
        {
            ent.Init(_draggedCard.so_character.hp, _draggedCard.so_character.dmg, _slot);
        }

        ConsumeMana(_draggedCard.so_character.mana);
        StopDragging();
        Destroy(_draggedCard.gameObject);
    }

    public void SetSlotSpawnPoint(Slot slot)
    {
        _slot = slot;
    }

    public void EnablePlaceableSlots(SO_Pawn pawn)
    {
        bool isBot = (pawn.placeInSlot & SO_Pawn.PlaceableSlot.BOT) != 0;
        bool isMiddle = (pawn.placeInSlot & SO_Pawn.PlaceableSlot.MIDDLE) != 0;
        bool isTop = (pawn.placeInSlot & SO_Pawn.PlaceableSlot.TOP) != 0;

        bool isWater = (pawn.slotType & SO_Pawn.SlotType.WATER) != 0;
        bool isGrass = (pawn.slotType & SO_Pawn.SlotType.GRASS) != 0;

        if (isGrass)
        {
            _botSlotsLine.SetActive(isBot);
            _middleSlotsLine.SetActive(isMiddle);
            _topSlotsLine.SetActive(isTop);
        }

        if (isWater)
        {
            _botWaterSlotsLine.SetActive(isBot);
            _middleWaterSlotsLine.SetActive(isMiddle);
            _topWaterSlotsLine.SetActive(isTop);
        }
    }

    public void DisablePlaceableSlots()
    {
        _botSlotsLine.SetActive(false);
        _middleSlotsLine.SetActive(false);
        _topSlotsLine.SetActive(false);
        _botWaterSlotsLine.SetActive(false);
        _middleWaterSlotsLine.SetActive(false);
        _topWaterSlotsLine.SetActive(false);
    }

    #endregion

    #region UI

    /// <summary>
    /// Start dragging a card from the player's hand.
    /// </summary>
    /// <param name="toDrag">Card i want to drag.</param>
    public void StartDragging(Card toDrag)
    {
        if (_canDrag) // Do it only if i can drag (if it is the player's turn).
        {
            _grid.enabled = false;
            _draggedCard = toDrag;
            EnablePlaceableSlots(_draggedCard.so_character);
            _draggedCardTransform = (RectTransform)toDrag.transform;
            _cardCurrentIndex = _draggedCardTransform.GetSiblingIndex();
            _draggedCardTransform.SetParent(_draggedCardTransform.root, false);
            _draggedCardTransform.anchoredPosition = Input.mousePosition;
            _draggedCardTransform.anchorMin = Vector2.zero;
            _draggedCardTransform.anchorMax = Vector2.zero;
            _isDragging = true;
        }
    }

    /// <summary>
    /// Return the dragged card to the hand of the player.
    /// </summary>
    private void ReturnCardToHand()
    {
        StopDragging();
        _draggedCardTransform.SetParent(_deck, false);
        _draggedCardTransform.SetSiblingIndex(_cardCurrentIndex);
    }

    /// <summary>
    /// Stop the dragging of the card.
    /// </summary>
    private void StopDragging()
    {
        DisablePlaceableSlots();
        _isDragging = false;
        _grid.enabled = true;
    }
    #endregion
}
