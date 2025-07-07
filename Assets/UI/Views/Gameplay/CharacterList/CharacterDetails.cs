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
        image.sprite = data.characterService.GetInstance().ship;
        characterName.text = data.characterService.GetName();
        level.text = "LVL " + data.characterService.GetLevel().ToString();
        UpdateHealth();
        // Register CharacterBattleState event -- for damage updates, etc.
        data.characterService.onDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthSlider.maxValue = instance.characterService.GetMaxHealth();
        healthSlider.value = instance.currentHealth;
        HealthLevel.text = instance.currentHealth + " / " + instance.characterService.GetMaxHealth();
    }

}
