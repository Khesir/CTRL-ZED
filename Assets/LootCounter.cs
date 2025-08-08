using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootCounter : MonoBehaviour
{
    public int currentAmount = 0;
    public TMP_Text counter;

    public void UpdateCounter(LootDropData dropData = null)
    {
        if (dropData == null) return;
        currentAmount += dropData.amount;
        counter.text = currentAmount.ToString();
    }
}
