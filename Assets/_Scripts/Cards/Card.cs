using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    internal SO_Pawn so_character;
    internal PlayerHandler playerReference;

    private Image _spriteIcon;
    private TMP_Text _hp, _dmg, _mana;

    public void SetUp()
    {
        SetUpIcon();
        SetUpStats();
    }

    private void SetUpIcon()
    {
        try
        {
            _spriteIcon = transform.GetChild(0).GetComponentInChildren<Image>();
            _spriteIcon.sprite = so_character.cardIcon;
        }
        catch(UnityException ex)
        {
            Debug.LogError(ex);
        }
    }

    private void SetUpStats()
    {
        try
        {
            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
            _hp = texts[0];
            _dmg = texts[1];
            _mana = texts[2];

            _hp.text = so_character.hp.ToString();
            _dmg.text = so_character.dmg.ToString();
            _mana.text = so_character.mana.ToString();
        }
        catch (UnityException ex)
        {
            Debug.LogError(ex);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == 0)
            playerReference.StartDragging(this);
    }
}
