using Cinemachine;
using GFA.TPS.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFA.TPS
{
    public class ExplosiveBarrel : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private float _health = 5;

        [SerializeField]
        private float _explosionRadius = 5;

        [SerializeField]
        private float _explosionDamage = 5;

        [SerializeField]
        private float _explosionForce = 50;

        [SerializeField]
        private float _delayBeforeExplodion = 1;

        [SerializeField]
        private AnimationCurve _explosionFalloff;

        private CinemachineImpulseSource _impulseSource;

        [SerializeField]
        private float _cameraShakePower = 0.1f;

        private bool _isDead;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void ApplyAamage(float damage, GameObject casuer = null)
        {
            if(_isDead) return;
            _health -= damage;
            if(_health < 0)
            {
                StartCoroutine(ExplodeDelayed());
                Explode();
                _isDead = true;
            }

        }

        private IEnumerator ExplodeDelayed()
        {
            yield return new WaitForSeconds(_delayBeforeExplodion);
                Explode();
        }

        private void Explode()
        {
            var hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (var hit in hits)
            {
                if(hit.transform == transform) continue;
                var distance = Vector3.Distance(transform.position, hit.transform.position);
                var rate = distance / _explosionRadius;
                var falloff = _explosionFalloff.Evaluate(rate);



                if(hit.transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyAamage(_explosionDamage * falloff);
                }

                if(hit.transform.TryGetComponent<CharacterMovement>(out var movement))
                {
                    movement.ExternalForces += (hit.transform.position - transform.position).normalized * _explosionForce * falloff * .5f;
                }

                if (hit.attachedRigidbody)
                {
                    hit.attachedRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1, ForceMode.Impulse);
                }

                _impulseSource.GenerateImpulseAt(transform.position, new Vector3(0,1,1) * _cameraShakePower);
               
            }


            Destroy(gameObject);
        }
    }

}

