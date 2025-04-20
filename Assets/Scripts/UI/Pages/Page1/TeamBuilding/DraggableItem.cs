using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public CharacterData instance;
    public bool isExternal;
    public void Setup(CharacterData data, bool external = false)
    {
        instance = data;
        isExternal = external;
        if (data == null)
        {
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
        }
        else
        {
            image.sprite = null;
            image.color = external ? Color.white : new Color(1, 1, 1, 0.5f); // full white or semi-transparent
        }
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
}
