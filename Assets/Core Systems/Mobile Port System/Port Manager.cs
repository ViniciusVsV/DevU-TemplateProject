using UnityEngine;

namespace MobilePortSystem
{
    public class PortManager : MonoBehaviour
    {
        [SerializeField] private PortUI portUI;

        private void Start()
        {
            RuntimePlatform platform = Application.platform;

            if
            (
                platform == RuntimePlatform.Android ||
                platform == RuntimePlatform.IPhonePlayer ||
                (platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
            )
            {
                PlayerPrefs.SetInt("IsOnMobile", 1);
                PlayerPrefs.Save();
            }
            else
                PlayerPrefs.DeleteKey("IsOnMobile");
        }

        //Para ativar o sistema
        //Deve ser chamado externamente por algum script ou sistema de transição de cenas
        public void EnableSystem()
        {
            if (PlayerPrefs.GetInt("IsOnMobile", 0) == 0)
                return;

            portUI.ToggleUI(true);
        }

        //Para desativar o sistema, como em cutscenes ou alguns menus
        //Deve ser chamado externamento pelos sistemas que lidam com essas ocasiões
        public void DisableSystem()
        {
            portUI.ToggleUI(false);
        }

        //Para desativar todos os botões menos o de pause, quando o jogo for pausado
        public void PauseControls(bool isPausing)
        {
            if (PlayerPrefs.GetInt("IsOnMobile", 0) == 0)
                return;

            portUI.DisableElementsOnPause(isPausing);
        }
    }
}