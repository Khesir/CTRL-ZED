using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType
{
    UI_Activate,
    UI_Button,
    UI_Error,
    UI_OnPause,
    UI_OnClose,
    UI_OnOpen,
    UI_OnSelect,
    UI_OnTransition,
    UI_Warning,
    Coins_massive,
    Coins_spend,
    Coins_Receive,
    Team_Deploy,
    Team_UnDeploy,
    Team_OnDrop,
    BGM_MainMenu,
    BGM_Start,
    BGM_Gameplay1,
    Gameplay_Shoot,
    Gameplay_Collect,
    Gameplay_Damage,
    Gameplay_Explosion,
    Gameplay_Immune,
}
public enum SoundCategory { UI, Coins, Team, BGM, Gameplay }
[System.Serializable]
public struct CategorizedSound
{
    public SoundCategory category;
    public SoundType soundType;
    public AudioClip clip;
}
