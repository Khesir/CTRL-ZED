using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    public void TakeDamage()
    {
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
    }
}
