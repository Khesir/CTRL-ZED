
using System;

public interface IBioChipService
{
  event Action OnSpendBioChip;
  int GetBioChip();
  void AddBioChip(int val);
  bool SpendBioChip(int val);
  bool SpendRemainingCharge(int val);
  int GetRemainingCharge();
}