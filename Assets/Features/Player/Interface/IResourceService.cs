
using System;

public interface IResourceService
{
  event Action OnResourceChange;
  int GetFood();
  void SpendFood(int val);
  void AddFood(int val);

  int GetTechnology();
  void SpendTechnology(int val);
  void AddTechnology(int val);

  int GetEnergy();
  void SpendEnergy(int val);
  void AddEnergy(int val);

  int GetIntelligence();
  void SpendIntelligence(int val);
  void AddIntelligence(int val);

}