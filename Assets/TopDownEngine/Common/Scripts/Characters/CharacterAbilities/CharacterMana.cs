using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class CharacterMana : MonoBehaviour
{
    float _currentMana;
    [SerializeField] float maxMana = 100;
    [SerializeField] MMProgressBar _bar;
    void Start()
    {
        _currentMana = 0;
        _bar = GameObject.Find("SpecialBar").GetComponent<MMProgressBar>();
        _bar.UpdateBar(_currentMana, 0, maxMana);
    }

    private void Update()
    {
        if (_bar == null)
        {
            _bar = GameObject.Find("SpecialBar").GetComponent<MMProgressBar>();
        }
    }

    public void AddMana(float mana)
    {
        _currentMana += mana;

        if (_currentMana > maxMana) _currentMana = maxMana;

        _bar?.UpdateBar(_currentMana, 0, maxMana);
    }

    public void UseMana(float mana)
    {
        _currentMana -= mana;

        if (_currentMana < 0) _currentMana = 0;

        _bar?.UpdateBar(_currentMana, 0, maxMana);
    }

    public bool HasEnoughMana(float manaToUse)
    {
        return _currentMana >= manaToUse;
    }
}
