using System;

public interface IExpService
{
  event Action OnLevelUp;
  event Action OnExpGained;
  float GetCoinsPerExpRate(); // Just return a simple float from field
  void GainExp(int amount);
  int GetCurrentExp();
  int GetRequiredExp();
  int GetLevel();
  int GetMaxLevel();
}