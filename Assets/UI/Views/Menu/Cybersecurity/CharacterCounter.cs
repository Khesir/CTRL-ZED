using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counter;

    public void Setup(int Count)
    {
        counter.text = "Cyber Securities " + Count;
    }

    public void UpdateCounter() => Setup(ServiceLocator.Get<ICharacterManager>().GetCharacters().Count);
}
