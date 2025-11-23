using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform firepoint;
    private const float LevelScalingFactor = 0.05f; // 5% faster fire rate per level

    private GameObject bulletPrefab;
    private float fireForce;
    private float baseFireRate;
    private float fireRate;
    private float nextFireTime = 0f;

    private GameObject source;
    private SourceType sourceType;
    private ISoundService soundService;

    public void Initialize(WeaponConfig config, GameObject source, SourceType sourceType)
    {
        bulletPrefab = config.bulletPrefab;
        fireForce = config.fireForce;
        baseFireRate = config.fireRate;

        this.sourceType = sourceType;
        this.source = source;
        soundService = ServiceLocator.Get<ISoundService>();

        // Apply level-based fire rate scaling for player characters
        if (sourceType == SourceType.Player)
        {
            var battleState = source.GetComponent<PlayerGameplayService>()?.GetCharacterState();
            int characterLevel = battleState?.data?.GetLevel() ?? 1;
            float levelMultiplier = 1 + (characterLevel * LevelScalingFactor);
            fireRate = baseFireRate * levelMultiplier;
            Debug.Log($"[Weapon] Fire rate scaled: base {baseFireRate} -> {fireRate} (Lv.{characterLevel}, x{levelMultiplier:F2})");
        }
        else
        {
            fireRate = baseFireRate;
        }
    }
    public void Fire()
    {
        if (Time.time >= nextFireTime && bulletPrefab != null && firepoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            // Damage Calculation
            var mod = source.GetComponent<IStatHandler>();
            var bulletConfig = bullet.GetComponent<Bullet>();
            bulletConfig.damage = mod.GetStat("ATK");

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firepoint.up * fireForce, ForceMode2D.Impulse);
            }
            bullet.transform.up = firepoint.up;
            nextFireTime = Time.time + (1f / fireRate);
            soundService.Play(SoundCategory.Gameplay, SoundType.Gameplay_Shoot);
        }
    }
    public void UpdateFirerate(float rate)
    {
        fireRate = rate;
    }
    public float GetFirerate() => fireRate;

}
