
using System;

public interface IHealthService
{
  event Action OnHealthChanged;
  void Heal();
  void HealAmount(float amount);
  void TakeDamage(float damage);
  float GetMaxHealth();
  float GetCurrentHealth();
  void HandleLevelUp(int level);
  bool IsDead();
}