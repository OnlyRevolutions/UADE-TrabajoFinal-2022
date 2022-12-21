using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBerserk : MonoBehaviour
{
    [SerializeField] private CharacterRun _characterRun;
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private KeyCode _abilityInput = KeyCode.E;
    [SerializeField] private bool _usingAbility = false;
    [SerializeField] private Character _character;
    [SerializeField] private CharacterMana _mana;

    [SerializeField] private MMFeedbacks _berserkFeedback;
    [SerializeField] private MMFeedbacks _stopFeedback;

    [SerializeField] private float manaUse = 100;

    [SerializeField] private float _abilityTime;
    private float _originalAbilityTime;

    [SerializeField] private float _abilityCooldown;
    private float _originalAbilityCooldown;

    private GameObject _clone;

    private float _originalSpeed;
    private float _originalHealth;

    //[SerializeField] private LayerMask _obstclesLayer;
    [SerializeField] private bool CanUseAbility => _abilityCooldown <= 0;


    // Start is called before the first frame update
    void Start()
    {
        _originalAbilityCooldown = _abilityCooldown;
        _abilityCooldown = 0;

        _originalAbilityTime = _abilityTime;
        _originalSpeed = _characterRun.RunSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CooldownTimer();
        HandleAbility();

    }

    private void HandleAbility()
    {
        if (_usingAbility == false && CanUseAbility)
        {
            if (Input.GetKeyDown(_abilityInput) && _mana.HasEnoughMana(manaUse))
            {
                BerserkStart();
            }
        }

        else if (_usingAbility == true)
        {
            _abilityTime -= Time.deltaTime;

            if (_abilityTime <= 0)
            {
                BerserkEnd();
            }
        }
    }

    private void CooldownTimer()
    {
        // Countdowns if not Using Ability and Has Cooldown.
        if (_usingAbility == false && _abilityCooldown > 0)
        {
            _abilityCooldown -= Time.deltaTime;
        }
    }

    private void BerserkStart()
    {
        _usingAbility = true;
        _originalHealth = _character.CharacterHealth.CurrentHealth;

        _clone = Instantiate(_clonePrefab, transform.position, Quaternion.identity);
        _characterRun.RunSpeed = _characterRun.RunSpeed * 2;
        _character.CharacterHealth.SetHealth(1);

        // Resets Ability Timer.
        _abilityTime = _originalAbilityTime;
        _mana.UseMana(manaUse);

        //Physics.IgnoreLayerCollision(_obstclesLayer.value, this.gameObject.layer, false);

        _berserkFeedback?.PlayFeedbacks();
    }

    private void BerserkEnd()
    {
        _usingAbility = false;
        transform.position = _clone.transform.position;
        _character.CharacterHealth.SetHealth(_originalHealth);

        //Physics.IgnoreLayerCollision(_obstclesLayer.value, this.gameObject.layer, true);

        // Resets Cooldown Timer.
        _abilityCooldown = _originalAbilityCooldown;
        _characterRun.RunSpeed = _originalSpeed;

        _berserkFeedback?.StopFeedbacks();
        _stopFeedback?.PlayFeedbacks();

        Destroy(_clone.gameObject);
    }
}
