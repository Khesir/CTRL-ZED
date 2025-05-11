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
    public CharacterService instance;
    public void Initialize(CharacterService data)
    {
        instance = data;
        image.sprite = data.GetInstance().baseData.ship;
        characterName.text = data.GetName();
        level.text = "LVL " + data.GetLevel().ToString();
        UpdateHealth();
        // Register characterService event -- for damage updates, etc.
        data.onDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthSlider.maxValue = instance.GetMaxHealth();
        healthSlider.value = instance.GetCurrentHealth();
        HealthLevel.text = instance.GetCurrentHealth() + " / " + instance.GetMaxHealth();
    }

}
