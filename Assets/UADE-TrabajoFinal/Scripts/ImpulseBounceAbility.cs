using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;

public class ImpulseBounceAbility : MonoBehaviour
{
    bool _usingAbility = false;
    Character _character;
    CharacterMovement _characterMovement;
    CharacterDamageDash2D _characterDash;
    CharacterHandleWeapon _characterWeapon;
    CharacterOrientation2D _orientation;
    TopDownController _controller;
    Health _health;
    Rigidbody2D _rb;

    public MMFeedbacks HitDamageableFeedback;

    float _impulseTimer;
    public float impulseDuration = 3f;
    public float impulseSpeed = 2f;

    public int impulseDamage = 10;
    


    Vector3 _impulseDir;
    Vector3 _startingDir;

    void Start()
    {
        _character = GetComponent<Character>();
        _controller = GetComponent<TopDownController>();
        _rb = GetComponent<Rigidbody2D>();
        _characterMovement = _character?.FindAbility<CharacterMovement>();
        _characterDash = _character?.FindAbility<CharacterDamageDash2D>();
        _orientation = _character?.FindAbility<CharacterOrientation2D>();
        _characterWeapon = _character?.FindAbility<CharacterHandleWeapon>();
        _health = _character.CharacterHealth;


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) StartImpulse();
        HandleAbility();
    }

    public void StartImpulse()
    {
        if (_usingAbility || _characterDash.Dashing) return;
        _usingAbility = true;
        _health.DamageDisabled();
        EnableAbilities(false);

        _controller.FreeMovement = false;

        _rb.useFullKinematicContacts = true;
        _controller.SetKinematic(true);

        //_startingDir = (transform.position + _characterWeapon.WeaponAimComponent.CurrentAim).normalized;
        _impulseDir =  ((transform.position + _characterWeapon.WeaponAimComponent.CurrentAim) - transform.position).normalized;
        if (!_orientation.IsFacingRight) _impulseDir *= -1f; 
        
    }

    public void StopImpulse()
    {
        _impulseTimer = 0;
        _usingAbility = false;
        _health.DamageDisabled();
        EnableAbilities(true);

        _controller.FreeMovement = true;

        _impulseDir = Vector3.zero;
        _controller.SetKinematic(false);
    }

    public void EnableAbilities(bool enabled)
    {
        _characterMovement.InputAuthorized = enabled;
        _characterDash.AbilityPermitted = enabled;
        _characterWeapon.AbilityPermitted = enabled;
    }

    public void HandleAbility()
    {
        if (_usingAbility)
        {
            if (_impulseTimer < impulseDuration)
            {
                _impulseTimer += Time.deltaTime;
                _controller.MovePosition((transform.position + _impulseDir));
                
            }
            else StopImpulse();
            
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _characterWeapon.WeaponAimComponent.CurrentAim.normalized);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + _impulseDir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_usingAbility) return;
        Health colHealth = collision.gameObject.GetComponent<Health>();
        if (colHealth)
        {
            if(!colHealth.ImmuneToDamage)
            {
                HitDamageableFeedback?.PlayFeedbacks();
                colHealth.Damage(impulseDamage, gameObject, 0.1f, 0.5f, Vector3.zero);
            }
        }
        if(collision.gameObject.layer == 8) // Si colisiona con una pared u obstaculo
        {
            _impulseDir = Vector3.Reflect(_impulseDir, collision.GetContact(0).normal);
        }
    }
}
