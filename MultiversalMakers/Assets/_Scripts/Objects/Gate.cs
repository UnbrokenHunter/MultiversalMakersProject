using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MultiversalMakers
{
    public class Gate : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool isPermamemt = false;

        [SerializeField] private float stayOpenBonus = .2f;

        [SerializeField] private float openCooldown = 1f;
        private bool isOnCooldown = false;
        private bool hasBeenOpened = false;

        [Header("Events")]
        [SerializeField] private UnityEvent OpenGate;
        [SerializeField] private UnityEvent CloseGate;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isOnCooldown) return;
            if (isPermamemt && hasBeenOpened) return;

            OpenGate?.Invoke();
            StartCoroutine(StartCooldown());
            hasBeenOpened = true;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(!isPermamemt)
                StartCoroutine(StartStayOpenBonus());
        }

        private IEnumerator StartStayOpenBonus()
        {
            yield return new WaitForSeconds(stayOpenBonus);
            CloseGate?.Invoke();
        }

        private IEnumerator StartCooldown()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(openCooldown);
            isOnCooldown = false;
        }
        
    }
}
