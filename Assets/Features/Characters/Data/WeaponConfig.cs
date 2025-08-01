using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Configs/Weapon")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public GameObject bulletPrefab;


    [Header("Stats")]
    public float fireRate = 5f;
    public float fireForce = 20f;
    public int baseDamage = 1;
}
