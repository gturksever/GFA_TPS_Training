using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFA.TPS.Test
{
    public class ShooterTest : MonoBehaviour
    {

        [SerializeField]
        private GameObject _projectilePrefab;

        private void Start()
        {
            StartCoroutine(Shooter());

           
        }


        private IEnumerator Shooter()
        {
            while (true)
            {
                yield return new WaitForSeconds(.2f);
                Instantiate(_projectilePrefab, transform.position, transform.rotation);
            }
        }

    }

}

