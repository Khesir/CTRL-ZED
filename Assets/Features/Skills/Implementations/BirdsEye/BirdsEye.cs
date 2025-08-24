using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BirdsEye : MonoBehaviour, ISkill, IStatProvider
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;


    [Header("Modifier")]
    [SerializeField] private List<StatModifierData> stats;

    // Internally referenced and controlled
    private Animator vfxAnimator;
    private GameObject vfx;

    [SerializeField] private CinemachineVirtualCamera vcam;

    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
    }
    public void Activate()
    {
        if (!CanActivate()) return;

        vcam.m_Lens.OrthographicSize = 10f;

        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            vfx.transform.localScale = new Vector3(2f, 2f, 1f);
            vfxAnimator = vfx.GetComponent<Animator>();
        }
        Debug.Log($"{user.name} used {config.skillName}");
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime());
    }
    private IEnumerator RemoveAfterLifetime()
    {
        yield return new WaitForSeconds(config.skillLifetime);

        vfxAnimator.SetTrigger("End");
        yield return new WaitForSeconds(0.2f);

        Destroy(vfx);
        if (vcam != null)
            vcam.m_Lens.OrthographicSize = 7f;
        Debug.Log($"{user.name} {config.skillName} expired");

    }
    public bool CanActivate()
    {
        return Time.time >= lastUsedTime + config.cooldown;
    }
    public IEnumerable<StatModifier> GetModifiers()
    {
        foreach (var mod in stats)
        {
            if (mod.value != 0)
            {
                yield return new StatModifier(mod.statId, mod.value, mod.type, mod.priority, this);
            }
        }
    }
    public SkillConfig GetSkillConfig() => config;

}
