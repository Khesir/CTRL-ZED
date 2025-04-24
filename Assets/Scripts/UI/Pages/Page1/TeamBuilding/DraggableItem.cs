using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public CharacterData instance;
    public bool isExternal;

    public TMP_Text characterName;
    public TMP_Text className;
    public TMP_Text level;
    public void Setup(CharacterData data, bool external = false)
    {
        instance = data;
        isExternal = external;
        image.sprite = data.baseData.icon;
        if (!external)
            setDetails(data);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
    private void setDetails(CharacterData data)
    {
        characterName.text = data.name;
        className.text = data.baseData.className;
        level.text = $"Lvl. {data.level}";
    }
}
