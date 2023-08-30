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
        private float _explosionForce = 5;

        [SerializeField]
        private AnimationCurve _explosionFalloff;

        public void ApplyAamage(float damage, GameObject casuer = null)
        {
            _health -= damage;
            if(_health < 0)
            {
                Explode();
            }

        }

        private void Explode()
        {
            var hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (var hit in hits)
            {
                var distance = Vector3.Distance(transform.position, hit.transform.position);
                var rate = distance / _explosionRadius;
                var falloff = _explosionFalloff.Evaluate(rate);



                if(hit.transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyAamage(_explosionDamage * falloff);
                }

                if (hit.attachedRigidbody)
                {
                    hit.attachedRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1, ForceMode.Impulse);
                }
               
            }


            Destroy(gameObject);
        }
    }

}

