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


        [Header("Events")]
        [SerializeField] private UnityEvent OpenGate;
        [SerializeField] private UnityEvent CloseGate;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OpenGate?.Invoke();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(!isPermamemt)
                CloseGate?.Invoke();
        }
        
    }
}
