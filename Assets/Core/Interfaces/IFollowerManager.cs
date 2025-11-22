using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowerManager
{
    event Action OnSwitch;
    List<GameObject> Initialize(List<CharacterBattleState> data);
    List<GameObject> HandleSwitchTeam();
    void SwitchTo(int index);
    void AddFollower(Follower follower);
    void ResetTarget();
    int GetAvailableFollower();

    Transform GetCurrentTarget();
    CharacterBattleState GetCurrentTargetBattleState();
}
