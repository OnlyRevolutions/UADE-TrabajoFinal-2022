using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class ChargeCharacterAbility : MonoBehaviour
{
    GameObject owner;
    Projectile _projectileComponent;
    DamageOnTouch _damageOnTouch;

    [SerializeField] float manaToCharge = 10f;

    private void Awake()
    {
        _projectileComponent = GetComponent<Projectile>();
        _damageOnTouch = GetComponent<DamageOnTouch>();
    }
    private void OnEnable()
    {
        owner = _projectileComponent.GetOwner();
        _damageOnTouch.HitDamageableEvent.AddListener(OnHitDamagable);
    }
    void Start()
    {
        //owner = _projectileComponent.GetOwner();
    }

    public void OnHitDamagable(Health health)
    {
        if(health.gameObject.layer == 13) // Si es un enemigo
        {
            owner.GetComponent<SpecialAbilityMana>()?.AddMana(manaToCharge);
        }
    }

    private void OnDisable()
    {
        _damageOnTouch.HitDamageableEvent.RemoveListener(OnHitDamagable);
    }
}
