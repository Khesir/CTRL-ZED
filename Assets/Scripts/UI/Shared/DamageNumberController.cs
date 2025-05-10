using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public DamageNumber prefab;
    public void CreateNumber(float value, Vector3 location)
    {
        DamageNumber damageNumber = Instantiate(prefab, location, transform.rotation, transform);
        damageNumber.SetValue(Mathf.RoundToInt(value));
    }
}
