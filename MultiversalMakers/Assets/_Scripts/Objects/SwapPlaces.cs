using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class SwapPlaces : MonoBehaviour
    {
        private SwapPlacesManager swapPlacesManager;
        private void Awake() => swapPlacesManager = GetComponentInParent<SwapPlacesManager>();


        private Collider2D triggerCollider;
        public Collider2D TriggerCollider { get => triggerCollider; }


        private bool isSwapTriggered = false;
        public bool IsSwapTriggered { get => isSwapTriggered; }

        private void OnTriggerEnter2D(Collider2D _collision)
        {
            triggerCollider = _collision;

            isSwapTriggered = true;
            swapPlacesManager.CheckSwaps();
        }

        private void OnTriggerExit2D(Collider2D _collision)
        {
            triggerCollider = null;

            isSwapTriggered = false;
            swapPlacesManager.CheckSwaps();
        }
    }
}
