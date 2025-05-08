using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
  public ResourceUI resourceUI;

  public async UniTask Initialize()
  {
    InitializeResourceUI();
    await UniTask.CompletedTask;
  }
  private void InitializeResourceUI()
  {
    resourceUI.Setup();
  }
}
