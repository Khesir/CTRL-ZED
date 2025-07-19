
using System;

public interface IDrivesService
{
  event Action OnSpendDrives;
  int GetDrives();
  void AddDrives(int val);
  bool SpendDrives(int val);
  bool SpendRemainingCharge(int val);
  int GetChargedDrives();
  DrivesChargePerResource GetResourceChargePerDrives();
}