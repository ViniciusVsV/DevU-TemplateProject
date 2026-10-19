using System;
using Ink.Runtime;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueObject : MonoBehaviour
    {
        [SerializeField] private TextAsset inkTextJSON;
        [SerializeField] private bool blockPlayerActions;
        private Story currentStory;

        public static event Action<Story, bool> OnDialogueInteracted;
        public static event Action OnDialogueKilled;

        private void Awake()
        {
            currentStory = new Story(inkTextJSON.text);
        }

        public void InteractDialogue()
        {
            Debug.Log("INICIANDO DIÁLOGO!");
            OnDialogueInteracted?.Invoke(currentStory, blockPlayerActions);
        }

        public void InteractDialogue(Story newStory)
        {
            if (currentStory != newStory)
                currentStory = newStory;

            OnDialogueInteracted?.Invoke(currentStory, blockPlayerActions);
        }

        public void KillDialogue()
        {
            OnDialogueKilled?.Invoke();
        }
    }
}