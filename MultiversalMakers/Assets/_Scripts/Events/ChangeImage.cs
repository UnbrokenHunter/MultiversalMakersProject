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

		public Image Image { get => image; }

		[Button]
        public void ChangeSprite(Sprite newSprite)
        {
            image.sprite = newSprite;
        }



    }
}
