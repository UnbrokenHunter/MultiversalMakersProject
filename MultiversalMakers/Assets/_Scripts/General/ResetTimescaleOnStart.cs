using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MultiversalMakers
{
    public class ResetTimescaleOnStart : MonoBehaviour
    {
        [SerializeField] private float defaultTimescale = 1.0f;

        [Button]
        private void Start() => MMTimeManager.Instance.SetTimeScaleTo(defaultTimescale);
    }
}
