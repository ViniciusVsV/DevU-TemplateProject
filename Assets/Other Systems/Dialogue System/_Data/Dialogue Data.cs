using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [Header("PANEL")]
    [Header("Activation")]
    public float initialSizeMultiplier;
    public float activationDuration;
    public Ease activationEase;

    [Header("Deactivation")]
    public float deactivationDuration;
    public Ease deactivationEase;

    [Header("TEXT")]
    public float typingDelay;
}
