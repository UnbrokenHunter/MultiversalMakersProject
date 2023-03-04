using UnityEngine;

namespace MultiversalMakers
{
    public class ResetTimescaleOnStart : MonoBehaviour
    {
        [SerializeField] private float defaultTimescale = 1.0f;

        private void Start() => Time.timeScale = defaultTimescale;

        private void Update()
        {
            print(Time.timeScale);
        }
    }
}
