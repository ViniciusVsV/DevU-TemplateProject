using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class TextEffects : MonoBehaviour
    {
        [SerializeField] private DialogueData dialogueData;
        [SerializeField] private TextMeshProUGUI text;

        private Coroutine typeRoutine;

        private void Awake()
        {
            text.maxVisibleCharacters = 0;
        }

        public void TypeText(string newText, Action onFinish)
        {
            text.maxVisibleCharacters = 0;
            text.text = newText;

            typeRoutine = StartCoroutine(TypingRoutine(onFinish));
        }
        private IEnumerator TypingRoutine(Action onFinish)
        {
            text.ForceMeshUpdate();

            int totalCharacters = text.textInfo.characterCount;

            for (int i = 0; i <= totalCharacters; i++)
            {
                text.maxVisibleCharacters = i;

                yield return new WaitForSeconds(dialogueData.typingDelay);
            }

            onFinish?.Invoke();

            typeRoutine = null;
        }

        public void FinishText()
        {
            if (typeRoutine != null)
            {
                StopCoroutine(typeRoutine);
                typeRoutine = null;
            }

            text.maxVisibleCharacters = int.MaxValue;
        }

        public void CleanText()
        {
            text.text = "";
        }
    }
}