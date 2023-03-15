using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectBeelzebub
{

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [SerializeField] private GameObject _loaderCanvas;
        [SerializeField] private Image _progressBar;

        private AsyncOperation scene;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ReloadScene()
        {
            GetComponent<MMF_Player>().PlayFeedbacks(); // Vignette
            StartCoroutine(LoadWait());
        }

        private IEnumerator LoadWait()
        {
            yield return new WaitForSeconds(0.05f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
        public void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);

        public void AllowSceneComplete()
        {
            if(scene != null)
                scene.allowSceneActivation = true;
        }

		public void LoadSceneAsync(string sceneName)
		{
			scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
		}

        public void LoadNextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}