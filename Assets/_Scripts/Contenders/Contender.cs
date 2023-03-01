using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Contender : MonoBehaviour
{
    [Header("Statistics")]
    public int maxStartingHp = 20;
    public int maxStartingMana = 10;
    private int _currentHp;
    private int _currentMana;
    private bool _isMyTurn = false;

    [Header("Texts")]
    public TMP_Text hpText;
    public TMP_Text manaText;

    private bool _isDead = false;

    public bool IsMyTurn { get => _isMyTurn; }
    public bool IsDead { get => _isDead; }

    public virtual void BeginTurn()
    {
        _isMyTurn = true;
    }

    public virtual void EndTurn()
    {
        _isMyTurn = false;
    }


    #region StatsManagement

    public void FillAll()
    {
        FillHp();
        FillMana();
    }

    public void FillHp()
    {
        _currentHp = maxStartingHp;
        UpdateHPText();
    }

    public void FillMana()
    {
        _currentMana = maxStartingMana;
        UpdateManaText();
    }

    public void IncreaseMaxMana(int qnt = 1)
    {
        maxStartingMana += qnt;
    }

    public void IncreaseMaxHp(int qnt)
    {
        maxStartingHp += qnt;
    }

    public void TakeHeal(int qnt)
    {
        if (qnt < 0)
            return;

        if (_currentHp + qnt > maxStartingHp)
        {
            FillHp();
            return;
        }

        _currentHp += qnt;
        UpdateHPText();
    }

    public void TakeDamage(int qnt)
    {
        qnt = Mathf.Abs(qnt);

        if (_currentHp - qnt <= 0)
        {
            Death();
            return;
        }

        _currentHp -= qnt;
        UpdateHPText();
    }

    public bool HaveManaToSpawnPawn(SO_Pawn pawn)
    {
        return _currentMana >= pawn.mana;
    }

    public void ConsumeMana(int qnt)
    {
        if (_currentMana - qnt < 0)
        {
            _currentMana = 0;
            return;
        }

        _currentMana -= qnt;
        UpdateManaText();
    }

    public virtual void Death()
    {
        _isDead = true;
    }

    #endregion

    #region UI

    private void UpdateAllTexts()
    {
        UpdateManaText();
        UpdateHPText();
    }

    private void UpdateManaText()
    {
        manaText.text = _currentMana.ToString();
    }

    private void UpdateHPText()
    {
        hpText.text = _currentHp.ToString();
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Harm dmg))
        {
            if(!IsDead)
                TakeDamage(dmg.Damage);
        }
    }
}
