using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class CharacterDetailsIcon : MonoBehaviour
{
    public Image image;
    public TMP_Text indexText;
    public CharacterService instance;
    public void Initialize(CharacterService data, int index)
    {
        instance = data;
        indexText.text = (index + 1).ToString();
        image.sprite = data.GetInstance().baseData.ship;

        data.onDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        if (instance.IsDead())
        {
            image.color = Color.red;
        }
    }

}
