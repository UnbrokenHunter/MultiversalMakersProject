using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiversalMakers
{
    public class ChangeImage : MonoBehaviour
    {

        [SerializeField] private Image image;

        [Button]
        public void ChangeSprite(Sprite newSprite)
        {
            image.sprite = newSprite;
        }

    }
}
