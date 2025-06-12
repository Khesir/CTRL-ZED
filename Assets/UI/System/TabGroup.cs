using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class TabGroup : MonoBehaviour
{
    public List<TabButtons> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButtons selectedTab;
    public TabButtons defaultButton;
    public List<GameObject> objectsToSwap;
    public void OnEnable()
    {
        if (defaultButton.isDefault)
        {
            OnTabSelected(defaultButton);
        }
    }
    public void OnDisable()
    {
        ResetTabs();
    }
    public void Subscribe(TabButtons button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtons>();
        }
        tabButtons.Add(button);
        // Incase its not manually assigned
        if (button.isDefault && defaultButton == null)
        {
            defaultButton = button;
            OnTabSelected(button);
        }
    }
    public void OnTabEnter(TabButtons button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
            button.background.sprite = tabHover;
    }
    public void OnTabExit(TabButtons button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButtons button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtons button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
