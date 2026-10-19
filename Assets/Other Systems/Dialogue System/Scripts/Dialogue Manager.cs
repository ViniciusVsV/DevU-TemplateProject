using System;
using System.Collections;
using Ink.Runtime;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private PanelEffects panelEffects;
        [SerializeField] private TextEffects textEffects;

        [Header("Booleans")]
        private bool isActive;
        private bool isTyping;
        private bool isBusy;

        public Story currentStory;

        public static event Action<bool> OnDialogueStarted;
        public static event Action OnDialogueEnded;

        private void OnEnable()
        {
            DialogueObject.OnDialogueInteracted += ManageDialogue;
            DialogueObject.OnDialogueKilled += EndDialogue;
        }
        private void OnDisable()
        {
            DialogueObject.OnDialogueInteracted -= ManageDialogue;
            DialogueObject.OnDialogueKilled -= EndDialogue;
        }

        private void ManageDialogue(Story newStory, bool blockPlayerActions)
        {
            Debug.Log("GERENCIANDO O DIÁLOGO!");

            //Decide o que fazer de acordo com o estado atual do diálogo:
            //Se estiver inativo, ativa
            if (!isActive)
            {
                isActive = true;
                isBusy = true;

                StartDialogue(newStory, blockPlayerActions);
            }

            //Se estiver ativo e não ocupado, prossegue com o diálogo
            else if (isActive && !isBusy)
                ContinueDialogue();
        }

        //Responsável por abrir o painel de diálogo e invocar OnDialogueStarted
        private void StartDialogue(Story newStory, bool blockPlayerActions)
        {
            StartCoroutine(StartRoutine(newStory, blockPlayerActions));
        }
        private IEnumerator StartRoutine(Story newStory, bool blockPlayerActions)
        {
            //Invoca o evento
            OnDialogueStarted?.Invoke(blockPlayerActions);

            //Inicializa a história
            currentStory = newStory;

            //Espera o painel abrir
            bool finished = false;
            panelEffects.OpenPanel(() => { finished = true; });

            yield return new WaitUntil(() => finished);

            //Deixa de estar ocupado
            isBusy = false;

            //Começa a digitar o próximo texto da história (nesse caso, o próximo texto é o primeiro)
            ContinueDialogue();
        }

        //Responsável por digitar o texto a seguir da história
        public void ContinueDialogue()
        {
            if (!isActive)
                return;

            StartCoroutine(ContinueRoutine());
        }
        public IEnumerator ContinueRoutine()
        {
            //Se já estiver digitando, finaliza de mostrar o texto
            if (isTyping)
            {
                textEffects.FinishText();

                isTyping = false;

                yield break;
            }

            isTyping = true;

            //Se há mais texto na história
            if (currentStory.canContinue)
            {
                //Esepra o texto ser digitado
                bool finished = false;
                textEffects.TypeText(currentStory.Continue(), () => { finished = true; });

                yield return new WaitUntil(() => finished);
            }
            //Se não há mais texto na história
            else
                EndDialogue();

            //Sinaliza que terminou de digitar
            isTyping = false;
        }

        //Responsável por fechar o painel de diálogo e incoar OnDialogueEnded
        private void EndDialogue()
        {
            StartCoroutine(EndRoutine());
        }
        private IEnumerator EndRoutine()
        {
            //Sinaliza que está opcupado
            isBusy = true;

            //Espera o painel fechar
            bool finished = false;
            panelEffects.ClosePanel(() => { finished = true; });

            yield return new WaitUntil(() => finished);

            //Limpa o texto
            textEffects.CleanText();

            //Reinicia a história
            currentStory.ChoosePathString("main");

            //Invoca o evento
            OnDialogueEnded?.Invoke();

            //Sinaliza que o sistema está inativo
            isActive = false;
        }
    }
}