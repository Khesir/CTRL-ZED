using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInformationModal : MonoBehaviour
{
    public Animator animator;
    public Image image;
    public TMP_Text title;
    public TMP_Text objective;
    public TMP_Text description;
    public TMP_Text recommendation;
    public Button button;
    [HideInInspector] public LevelData data;
    public void Trigger()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Close");
        Debug.Log("Truggered");

    }
    public void CloseTrigger()
    {
        animator.SetTrigger("Close");

        StartCoroutine(WaitUntilReadyThenDisable());
    }
    private IEnumerator WaitUntilReadyThenDisable()
    {
        while (animator.IsInTransition(0))
            yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Ready"))
        {
            yield return null; // Wait 1 frame
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        Debug.Log(data.levelName);
        image.sprite = data.levelIcon;
        title.text = data.levelName;
        objective.text = data.objective;
        recommendation.text = data.recommendation;
        description.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartGame);
    }
    void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    void StartGame()
    {
        Debug.Log("Starting Game!");
    }
}
