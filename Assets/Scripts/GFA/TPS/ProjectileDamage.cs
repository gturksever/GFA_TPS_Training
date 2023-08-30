using GFA.TPS.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField]
    private float _damage = 1;

    private ProjectileMovement _projectileMovement;

    private void Awake()
    {
        _projectileMovement = GetComponent<ProjectileMovement>();
    }

    private void OnEnable()
    {
        _projectileMovement.Impacted += OnImpacted;
    }

    private void OnDisable()
    {
        _projectileMovement.Impacted -= OnImpacted;
    }

    private void OnImpacted(RaycastHit hit)
    {
       if (hit.transform.TryGetComponent<IDamageable>(out  var damageable))
        {
            damageable.ApplyAamage(_damage,gameObject);
        }
    }
}
