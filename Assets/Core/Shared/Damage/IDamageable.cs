using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, GameObject source = null);
    bool isDead { get; }
}
