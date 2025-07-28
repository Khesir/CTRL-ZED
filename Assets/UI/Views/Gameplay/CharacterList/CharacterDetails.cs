using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class CharacterDetails : MonoBehaviour
{
    public Image image;
    public TMP_Text characterName;
    public TMP_Text level;
    public Slider healthSlider;
    public TMP_Text HealthLevel;
    public CharacterBattleState instance;
    public void Initialize(CharacterBattleState data)
    {
        instance = data;
        image.sprite = data.data.GetInstance().ship;
        characterName.text = data.data.GetName();
        level.text = "LVL " + data.data.GetLevel().ToString();
        UpdateHealth();
        // Register CharacterBattleState event -- for damage updates, etc.
        data.data.onDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthSlider.maxValue = instance.data.GetMaxHealth();
        healthSlider.value = instance.currentHealth;
        HealthLevel.text = instance.currentHealth + " / " + instance.data.GetMaxHealth();
    }

}
