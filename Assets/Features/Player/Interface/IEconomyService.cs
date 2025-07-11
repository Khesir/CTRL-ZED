
using System;

public interface IEconomyService
{
  event Action OnCoinsChange;
  float GetCoinsPerExp();
  float GetHealthPerCoin();
  int GetRequiredCoinsToLevelup();
  int GetCoins();
  void AddCoins(int val);
  bool SpendCoins(int val);
}