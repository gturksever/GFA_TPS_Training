using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFA.TPS.Movement
{
    public class ProjectileMovement : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        public float speed 
        {
            get => _speed; 
            set => _speed = value;
        }

        [SerializeField]
        private Vector3 _movementPlane = Vector3.one;

        [SerializeField]
        private bool _shouldDisableOnCollision;
        public bool shouldDisableOnCollision
        {
            get => _shouldDisableOnCollision;
            set => _shouldDisableOnCollision = value;
        }

        [SerializeField]
        private bool _shouldDestroyOnCollision;
        public bool shouldDestroyOnCollision
        {
            get => _shouldDestroyOnCollision;
            set => _shouldDestroyOnCollision = value;
        }

        [SerializeField]
        private bool _shouldBounce;
        public bool shouldBounce
        {
            get => _shouldBounce;
            set => _shouldBounce = value;
        }

        [SerializeField]
        private float _pushPower;

        public event Action<RaycastHit> Impacted;

        private void Update()
        {
            var direction = transform.forward;
            direction.x *= _movementPlane.x;
            direction.y *= _movementPlane.y;
            direction.z *= _movementPlane.z;

            direction.Normalize();

            var distance = _speed * Time.deltaTime;
            var targetPosition = transform.position + direction * distance;

            if(Physics.Raycast(transform.position,direction, out var hit, distance)) 
            {
                if (hit.rigidbody) //çarpýþtýðýmýz nesnenin rigidbodysi var ise?
                {
                    hit.rigidbody.AddForceAtPosition(-hit.normal * _speed * _pushPower, hit.point,ForceMode.Impulse);
                }
                if (_shouldDestroyOnCollision)
                {
                    Destroy(gameObject);
                }
                

                if(_shouldDisableOnCollision)
                {
                    enabled = false;
                }

                if (_shouldBounce)
                {
                    var reflectedDirection = Vector3.Reflect(direction, hit.normal);
                    transform.forward = reflectedDirection;
                }

                targetPosition = hit.point + transform.forward * 0.01f;

                Impacted?.Invoke(hit);
               

            }

            Debug.DrawLine(transform.position, targetPosition, Color.red);
            transform.position = targetPosition;
            Debug.DrawRay(transform.position, direction * distance, Color.blue);

        }
    }
}


