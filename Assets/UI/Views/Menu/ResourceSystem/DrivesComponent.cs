using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrivesComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text usable;
    [SerializeField] private TMP_Text charged;
    private PlayerService service;

    public void Setup()
    {
        service = GameManager.Instance.PlayerManager.playerService;
        UpdateText();
        service.OnSpendDrives += UpdateText;
    }
    public void UpdateText()
    {
        usable.text = service.GetDrives().ToString();
        charged.text = service.GetChargedDrives().ToString();
    }
}
