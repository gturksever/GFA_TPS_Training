using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
   void ApplyAamage(float damage, GameObject casuer = null);
}
