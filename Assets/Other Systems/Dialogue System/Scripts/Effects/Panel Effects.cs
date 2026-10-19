using System;
using DG.Tweening;
using UnityEngine;

namespace DialogueSystem
{
    public class PanelEffects : MonoBehaviour
    {
        [SerializeField] private DialogueData dialogueData;
        [SerializeField] private RectTransform panel;

        private void Awake()
        {
            panel.gameObject.SetActive(false);
        }

        public void OpenPanel(Action onFinish)
        {
            panel.localScale = Vector3.one * dialogueData.initialSizeMultiplier;
            panel.gameObject.SetActive(true);

            panel.DOScale(Vector3.one, dialogueData.activationDuration)
                .SetEase(dialogueData.activationEase)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    onFinish?.Invoke();
                });
        }

        public void ClosePanel(Action onFinish)
        {
            panel.DOScale(Vector3.one * 0f, dialogueData.deactivationDuration)
                .SetEase(dialogueData.deactivationEase)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    panel.gameObject.SetActive(false);
                    onFinish?.Invoke();
                });
        }
    }
}