using System;

public interface IExpService
{
  event Action OnLevelUp;
  event Action OnExpGained;
  void GainExp(int amount);
  int GetCurrentExp();
  int GetRequiredExp();
  int GetLevel();
  int GetMaxLevel();
}