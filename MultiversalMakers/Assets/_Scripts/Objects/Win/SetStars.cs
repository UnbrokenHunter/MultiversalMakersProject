using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace MultiversalMakers
{
    public class SetStars : MonoBehaviour
    {
        [SerializeField] private Sprite filledStar;
        [SerializeField] private float animationTime = 0.05f;

        private void OnEnable() => StartCoroutine(SetLevelStars());

        [Button]
        public IEnumerator SetLevelStars()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            ChangeImage[] _starImages = GetComponentsInChildren<ChangeImage>();

            for (int i = 0; i < GameManager.Instance.StarCount; i++)
            {
                if (_starImages.Length <= i) yield break;

                // Switching the first and the middle star
                ChangeImage star;
                if (i == 0) star = _starImages[i + 1];
                else if (i == 1) star = _starImages[i - 1];
				else star = _starImages[i];


				star.ChangeSprite(filledStar);
                StartCoroutine(FadeInStar(star));


                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
        private IEnumerator FadeInStar(ChangeImage star)
        {
			star.Image.color = new Color(
				star.Image.color.r,
				star.Image.color.g,
				star.Image.color.b,
	            0);


			while (star.Image.color.a < 250)
            {
                Color _color = star.Image.color;

			    _color = new Color(
                    _color.r,
                    _color.g, 
                    _color.b, 
                    Mathf.Lerp(_color.a, 1, 0.1f));

				star.Image.color = _color;

                yield return new WaitForSecondsRealtime(animationTime);

            }
		}
	}
}
