using UnityEngine;
using UnityEngine.SceneManagement;

namespace CameraSystem
{
    public class CanvasesSetup : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            BindCanvases(SceneManager.GetActiveScene());
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            BindCanvases(scene);
        }

        private void BindCanvases(Scene currentScene)
        {
            foreach (var root in currentScene.GetRootGameObjects())
            {
                foreach (var canvas in root.GetComponentsInChildren<Canvas>(true))
                {
                    if (!canvas.isRootCanvas)
                        continue;

                    if (canvas.renderMode == RenderMode.WorldSpace)
                        continue;

                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = mainCamera;
                }
            }
        }
    }
}