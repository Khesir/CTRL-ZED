using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementController : MonoBehaviour
{
    public GameObject target = null;

    public SoundType activate = SoundType.UI_OnOpen;
    public SoundType deactivate = SoundType.UI_OnClose;

    [Header("Flags")]
    public bool isAnimated = false;
    public string TriggerName;
    /// <summary>
    /// Activates the UI element and plays the activate sound.
    /// </summary>
    public void Activate()
    {
        if (!target)
        {
            gameObject.SetActive(true);
        }
        else
        {
            target.SetActive(true);
        }
        if (isAnimated)
        {
            if (TriggerName == null)
            {
                Debug.Log("[UIElementController] TriggerName not set");
            }
            var x = target.GetComponent<Animator>();
            x.SetTrigger(TriggerName);
        }
        SoundManager.PlaySound(SoundCategory.UI, activate);
    }

    /// <summary>
    /// Deactivates the UI element and plays the deactivate sound.
    /// </summary>
    public void Deactivate()
    {

        if (!target)
        {
            gameObject.SetActive(false);
        }
        else
        {
            target.SetActive(false);
        }
        SoundManager.PlaySound(SoundCategory.UI, deactivate);
    }
}
