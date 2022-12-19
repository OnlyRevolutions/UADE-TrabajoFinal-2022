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

    [SerializeField] private MMFeedbacks _berserkFeedback;
    [SerializeField] private MMFeedbacks _stopFeedback;

    [SerializeField] private float _abilityTime;
    private float _originalAbilityTime;

    [SerializeField] private float _abilityCooldown;
    private float _originalAbilityCooldown;

    private GameObject _clone;

    private float _originalSpeed;
    private float _originalHealth;

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
            if (Input.GetKeyDown(_abilityInput))
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
        Debug.Log("Casting Ability.");
        _berserkFeedback?.PlayFeedbacks();

        _usingAbility = true;
        _originalHealth = _character.CharacterHealth.CurrentHealth;

        // Resets Ability Timer.
        _abilityTime = _originalAbilityTime;
        _clone = Instantiate(_clonePrefab, transform.position, Quaternion.identity);
        _characterRun.RunSpeed = _characterRun.RunSpeed * 2;
        _character.CharacterHealth.SetHealth(1);

    }

    private void BerserkEnd()
    {
        Debug.Log("Finished Ability.");
        _berserkFeedback?.StopFeedbacks();
        _stopFeedback?.PlayFeedbacks();

        _usingAbility = false;
        transform.position = _clone.transform.position;
        _character.CharacterHealth.SetHealth(_originalHealth);

        // Resets Cooldown Timer.
        _abilityCooldown = _originalAbilityCooldown;
        _characterRun.RunSpeed = _originalSpeed;

        Destroy(_clone.gameObject);
        //_stopFeedback?.StopFeedbacks();
    }
}
