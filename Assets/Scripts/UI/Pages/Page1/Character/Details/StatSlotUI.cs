using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlotUI : MonoBehaviour
{

    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Value;
    public void Setup(KeyValuePair<string, int> data)
    {
        Name.text = data.Key;
        Value.text = data.Value.ToString();
    }
}
