using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private IWeapon currentWeapon;
    public IWeapon CurrentWeapon => currentWeapon;

    /// <summary>
    /// Equips a new weapon using the given WeaponConfig.
    /// Instantiates the prefab, parents it to this transform, and initializes it.
    /// </summary>
    /// <param name="config">WeaponConfig containing prefab and stats</param>
    public void EquipWeapon(WeaponConfig config)
    {
        if (config == null || config.weaponPrefab == null)
        {
            Debug.LogWarning("[WeaponHolder] WeaponConfig or prefab is null");
            return;
        }
        // Remove old weapon if any
        if (currentWeapon != null)
        {
            Destroy(((MonoBehaviour)currentWeapon).gameObject);
            currentWeapon = null;
        }
        // Instantiate and attach weapon
        GameObject weaponGO = Instantiate(config.weaponPrefab, transform);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;

        currentWeapon = weaponGO.GetComponent<IWeapon>();
        if (currentWeapon == null)
        {
            Debug.LogError($"[WeaponHolder] Weapon prefab '{config.weaponPrefab.name}' does not implement IWeapon!");
            Destroy(weaponGO);
            return;
        }
        currentWeapon.Initialize(config);
    }
    public void Fire()
    {
        currentWeapon?.Fire();
    }
    public void Unequip()
    {
        if (currentWeapon != null)
        {
            Destroy(((MonoBehaviour)currentWeapon).gameObject);
            currentWeapon = null;
        }
    }
}
