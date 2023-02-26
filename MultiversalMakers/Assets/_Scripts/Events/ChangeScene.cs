using ProjectBeelzebub;
using UnityEngine;

namespace MultiversalMakers
{
    public class ChangeScene : MonoBehaviour
    {
        public void LoadScene(string sceneName) => LevelManager.Instance.LoadScene(sceneName);

        public void LoadSceneAsync(string sceneName) => LevelManager.Instance.LoadSceneAsync(sceneName);

        public void LoadNextScene() => LevelManager.Instance.LoadNextScene();

    }
}
