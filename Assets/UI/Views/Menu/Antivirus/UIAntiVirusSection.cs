using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAntiVirusSection : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private UIAntivirusAbout uIAntivirusAbout;

  [SerializeField] private UIAntivirusSelector uIAntivirusSelector;
  private void OnEnable()
  {
    List<AntiVirus> antivirus = GameManager.Instance.AntiVirusManager.allBuffs;
    uIAntivirusAbout.Setup(antivirus[0]);
    uIAntivirusSelector.Setup(uIAntivirusAbout, antivirus);
  }
  private void OnDisable()
  {
    uIAntivirusSelector.Clear();
  }
}
