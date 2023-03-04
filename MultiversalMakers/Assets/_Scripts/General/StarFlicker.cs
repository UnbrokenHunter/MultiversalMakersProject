using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MultiversalMakers
{
    public class StarFlicker : MonoBehaviour
    {
        private Light2D starLight;

        // How often it flickers
        [SerializeField] private float flickerPeriod = 5f;
        [SerializeField] private float flickerTime = 0.1f;
        [SerializeField] private float flickerSpeed = 0.1f;

        private float lightBrightness;
        private bool lerpingDown;

        private void Awake()
        {
            starLight= GetComponent<Light2D>();
            lightBrightness = starLight.intensity;

            InvokeRepeating(nameof(Flicker), flickerPeriod, flickerPeriod);
        }

        private void Flicker()
        {
            lerpingDown = true;
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            starLight.intensity = (float) System.Math.Round((decimal) starLight.intensity, 2);


            if (lerpingDown)
            {
                starLight.intensity = Mathf.Lerp(starLight.intensity, 0, flickerSpeed);
                if (starLight.intensity <= flickerSpeed) lerpingDown = false;
                else yield return new WaitForSeconds(flickerTime);
            }
            else
            {
                starLight.intensity = Mathf.Lerp(starLight.intensity, lightBrightness, flickerSpeed);

                if (starLight.intensity >= lightBrightness) yield break;
                else yield return new WaitForSeconds(flickerTime);
            }

            StartCoroutine(Wait());
        }



    }
}
