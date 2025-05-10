using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private float floatSpeed = 1;
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
    }
    public void SetValue(int value)
    {
        damageText.text = value.ToString();
    }
}
