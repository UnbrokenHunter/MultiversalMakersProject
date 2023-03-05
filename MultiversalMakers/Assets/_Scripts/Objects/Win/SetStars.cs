using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class SetStars : MonoBehaviour
    {
        [SerializeField] private Sprite filledStar;

        private void OnEnable()
        {
            SetLevelStars();
        }

        [Button]
        public void SetLevelStars()
        {
            ChangeImage[] _starImages = GetComponentsInChildren<ChangeImage>();

            for (int i = 0; i < GameManager.Instance.StarCount; i++)
            {
                if (_starImages.Length <= i) return;

                _starImages[i].ChangeSprite(filledStar);
            }
        }

    }
}
