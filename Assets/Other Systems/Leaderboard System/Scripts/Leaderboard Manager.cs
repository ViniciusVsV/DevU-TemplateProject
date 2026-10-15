using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

namespace LeaderboardSystem
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private GeneralData generalData;

        private LeaderboardUI leaderboardUI;

        private bool hasSubmitted;
        
        private async void Start()
        {
            leaderboardUI = FindAnyObjectByType<LeaderboardUI>();
            if (leaderboardUI == null)
            {
                Debug.LogError("[Leaderboard Manager] No Leaderboard UI found!");
                return;
            }

            try
            {
                if (UnityServices.State == ServicesInitializationState.Uninitialized)
                    await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                GetScores();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnEnable()
        {
            LeaderboardUI.OnScoreSubmitted += SubmitScore;
        }
        private void OnDisable()
        {
            LeaderboardUI.OnScoreSubmitted -= SubmitScore;
        }

        public async void SubmitScore()
        {
            //Cria uma conta anônima para postar o score obtido
            //Primeiro, verifica se já está enviando a requisição. Evita duplicatas
            if (hasSubmitted)
                return;

            //Segundo, verifica se o nome é válido
            string username = leaderboardUI.GetName();
            if (string.IsNullOrEmpty(username))
                return;

            //Terceiro, pega o score obtido
            int score = PlayerPrefs.GetInt("Score", 0);

            //Quarto, tenta fazer as operações de troca de contas
            hasSubmitted = true;
            
            try
            {
                AuthenticationService.Instance.SignOut(true);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                await AuthenticationService.Instance.UpdatePlayerNameAsync(username);

                await LeaderboardsService.Instance.AddPlayerScoreAsync(generalData.leaderboardId, score);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                GetScores();
            }
        }

        public async void GetScores()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                return;

            try
            {
                var response = await LeaderboardsService.Instance.GetScoresAsync
                (
                    generalData.leaderboardId,
                    new GetScoresOptions { Offset = 0, Limit = 50 }
                );

                leaderboardUI.UpdateValues(response.Results);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}