using ProjectBeelzebub;
using UnityEngine;

namespace MultiversalMakers
{
    public class LevelSelect : MonoBehaviour
    {

        public void SelectLevel()
        {
            print(transform.GetSiblingIndex()) ;
            LevelManager.Instance.LoadScene(transform.GetSiblingIndex() + 1);
        }
    }
}
