using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class CharacterDetailsIcon : MonoBehaviour
{
    public Image image;
    public TMP_Text indexText;
    public CharacterBattleState instance;
    public void Initialize(CharacterBattleState data, int index)
    {
        instance = data;
        indexText.text = (index + 1).ToString();
        image.sprite = data.data.GetInstance().ship;

        data.data.onDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        if (instance.isDead)
        {
            image.color = Color.red;
        }
    }

}
