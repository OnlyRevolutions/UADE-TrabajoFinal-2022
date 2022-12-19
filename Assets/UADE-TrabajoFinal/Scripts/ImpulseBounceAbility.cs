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
    Collider2D _col;

    float _originalDrag;
    float _originalAngularDrag;
    float _originalMass;

    public MMFeedbacks HitDamageableFeedback;
    public MMFeedbacks ImpulseFeedback;

    float _impulseTimer;
    public float impulseDuration = 3f;
    public float impulseSpeed = 50f;

    public int impulseDamage = 10;
    [SerializeField] PhysicsMaterial2D bouncyMat;
    


    Vector3 _impulseDir;
    Vector3 _startingDir;

    void Start()
    {
        _character = GetComponent<Character>();
        _controller = GetComponent<TopDownController>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _characterMovement = _character?.FindAbility<CharacterMovement>();
        _characterDash = _character?.FindAbility<CharacterDamageDash2D>();
        _orientation = _character?.FindAbility<CharacterOrientation2D>();
        _characterWeapon = _character?.FindAbility<CharacterHandleWeapon>();
        _health = _character.CharacterHealth;

        _originalAngularDrag = _rb.angularDrag;
        _originalDrag = _rb.drag;
        _originalMass = _rb.mass;


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) StartImpulse();
        HandleAbility();
    }

    private void FixedUpdate()
    {
        if (_usingAbility)
        {
            if (_rb.velocity.magnitude > 40f || _rb.velocity.magnitude < 35f) _rb.velocity = _rb.velocity.normalized * 40f;
        }   
    }

    public void StartImpulse()
    {
        if (_usingAbility || _characterDash.Dashing) return;
        _usingAbility = true;
        _health.DamageDisabled();
        _health.ImmuneToKnockback = true;
        _rb.mass = 0.1f;
        EnableAbilities(false);

        _controller.FreeMovement = false;

        //_rb.useFullKinematicContacts = true;
        // _controller.SetKinematic(true);
        _col.sharedMaterial = bouncyMat;

        ImpulseFeedback?.PlayFeedbacks();

        //_startingDir = (transform.position + _characterWeapon.WeaponAimComponent.CurrentAim).normalized;
        _impulseDir =  ((transform.position + _characterWeapon.WeaponAimComponent.CurrentAim) - transform.position).normalized;
        if (!_orientation.IsFacingRight) _impulseDir *= -1f;

        _rb.drag = 0;
        _rb.angularDrag = 0;
        _rb.AddForce(_impulseDir * impulseSpeed, ForceMode2D.Impulse);


        
        
    }

    public void StopImpulse()
    {
        _rb.velocity = Vector3.zero;
        _impulseTimer = 0;
        _usingAbility = false;
        _health.DamageEnabled();
        _health.ImmuneToKnockback = false;
        _rb.mass = _originalMass;
        EnableAbilities(true);

        _controller.FreeMovement = true;

        _col.sharedMaterial = null;

        ImpulseFeedback?.StopFeedbacks();

        _impulseDir = Vector3.zero;
        //_controller.SetKinematic(false);

        _rb.drag = _originalDrag;
        _rb.angularDrag = _originalAngularDrag;

    }

    public void EnableAbilities(bool enabled)
    {
        _characterMovement.InputAuthorized = enabled;
        _characterMovement.AbilityPermitted = enabled;
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
                Debug.Log(_rb.velocity.magnitude);
                //if (_rb.velocity.magnitude > 40f) _rb.velocity = _rb.velocity.normalized * 40f;
                //_controller.MovePosition((transform.position + _impulseDir));
                
            }
            else StopImpulse();
            
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _characterWeapon.WeaponAimComponent.CurrentAim.normalized);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + _impulseDir * 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_usingAbility) return;
        Health colHealth = collision.gameObject.GetComponent<Health>();
        if (colHealth)
        {
            if(!colHealth.ImmuneToDamage)
            {
                colHealth.Damage(impulseDamage, gameObject, 0.1f, 0.5f, Vector3.zero);
                if (colHealth.gameObject.layer == 13)
                {
                    HitDamageableFeedback?.PlayFeedbacks();
                }
            }
        }
        /*if(collision.gameObject.layer == 8) // Si colisiona con una pared u obstaculo
        {
            _impulseDir = Vector3.Reflect(_impulseDir, collision.GetContact(0).normal);
        }*/
    }
}
