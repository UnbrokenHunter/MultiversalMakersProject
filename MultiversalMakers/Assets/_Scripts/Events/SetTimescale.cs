using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

namespace MultiversalMakers
{
    public class SetTimescale : MonoBehaviour
    {
        public void SetScale(float timeScaleTarget) => Time.timeScale = timeScaleTarget;

        public void SetScaleOnManager(float timeScaleTarget)
        {
            MMTimeManager.Instance.SetTimeScaleTo(timeScaleTarget);
        }
    }
}
