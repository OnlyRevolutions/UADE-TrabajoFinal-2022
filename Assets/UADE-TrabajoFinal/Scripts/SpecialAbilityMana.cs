using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
public class SpecialAbilityMana : MonoBehaviour
{
    float _currentMana;
    [SerializeField] float maxMana = 100;
    MMProgressBar _bar;
    void Start()
    {
        _currentMana = 0;
        _bar = GameObject.Find("SpecialBar").GetComponent<MMProgressBar>();

        _bar.UpdateBar(_currentMana, 0, maxMana);
    }

    void Update()
    {
        Debug.Log(_currentMana);
    }

    public void AddMana(float mana)
    {
        _currentMana += mana;

        if (_currentMana > maxMana) _currentMana = maxMana;

        _bar.UpdateBar(_currentMana, 0, maxMana);
    }

    public void UseMana(float mana)
    {
        _currentMana -= mana;

        if (_currentMana < 0) _currentMana = 0;

        _bar.UpdateBar(_currentMana, 0, maxMana);
    }

    public bool HasEnoughMana(float manaToUse)
    {
        return _currentMana >= manaToUse;
    }
}
