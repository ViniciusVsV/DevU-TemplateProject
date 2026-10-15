using UnityEngine;

namespace MobilePortSystem
{
    public class PortUI : MonoBehaviour
    {
        [SerializeField] private GameObject rootObject;

        [Header("Pause Behaviour")]
        [SerializeField] private GameObject[] disableableOnPause;

        public void ToggleUI(bool isActivating)
        {
            rootObject.SetActive(isActivating);
        }

        public void DisableElementsOnPause(bool isPausing)
        {
            foreach (var uiElement in disableableOnPause)
                uiElement.SetActive(!isPausing);
        }
    }
}