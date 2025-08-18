using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrivesMenuComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text content;
    private PlayerService service;
    public void Setup()
    {
        service = GameManager.Instance.PlayerManager.playerService;
        UpdateText();
        service.OnSpendDrives += UpdateText;
    }
    public void OnDestroy()
    {
        service.OnSpendDrives -= UpdateText;

    }
    public void UpdateText()
    {
        content.text = service.GetDrives().ToString() + " / " + service.GetChargedDrives().ToString();
    }
}
