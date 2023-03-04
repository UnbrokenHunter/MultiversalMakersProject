using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MultiversalMakers
{
    public class ResetTimescaleOnStart : MonoBehaviour
    {
        [SerializeField] private float defaultTimescale = 1.0f;

        [Button]
        private void Start()
        {
            MMTimeManager.Instance.SetTimeScaleTo(defaultTimescale);
        }

        private void Update()
        {
            print($" Normal: {Time.timeScale} Current: {MMTimeManager.Instance.CurrentTimeScale} Target: { MMTimeManager.Instance.TargetTimeScale } MM Normal: { MMTimeManager.Instance.NormalTimeScale } ");
        }
    }
}
