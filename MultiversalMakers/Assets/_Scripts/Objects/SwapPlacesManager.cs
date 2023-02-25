using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace MultiversalMakers
{
    public class SwapPlacesManager : MonoBehaviour
    {

        [Title("Settings")]
        [SerializeField] private bool limitSwaps = false;

        [SerializeField,
            ShowIf("@limitSwaps"),
            Tooltip("How many times can the players swap back and forth")]
        private int maxSwaps = 1;

        [SerializeField,
            Tooltip("Minimum Delay between 2 swaps.")] 
        private float swapCooldown = 1f;


        [Space]



        [SerializeField, 
            Tooltip("From when swapping is triggered, how long should you wait before swapping positions.")] 
        private float swapDelay = 0.5f;


        [Space]


        [Title("Events")]
        [SerializeField] private UnityEvent swapPlacesEvent;




        private SwapPlaces[] swapPlaces;
        private void Awake() => swapPlaces = GetComponentsInChildren<SwapPlaces>();

        [Tooltip("How many times you have swapped")] private int swappedCount;
        [Tooltip("Backend bool to allow orr disallow movement due to the cooldown")] private bool isOnCooldown = false;
        
        public void CheckSwaps()
        {
            if (isOnCooldown) return;
            if (swapPlaces.Length < 1) return;
            if (limitSwaps && swappedCount >= maxSwaps) return;

            if (AreAllTriggered())
                SwapPlayerPlaces();
        }

        private bool AreAllTriggered()
        {
            bool _isAllTriggered = true;
            Array.ForEach(swapPlaces, swapPlace => _isAllTriggered = swapPlace.IsSwapTriggered && _isAllTriggered);
            return _isAllTriggered;
        }

        private async void SwapPlayerPlaces()
        {
            await Task.Delay((int)(swapDelay * 1000));
         
            if (!AreAllTriggered()) return;
            StartSwapCooldown();

            swapPlacesEvent?.Invoke();

            Vector3 temp = swapPlaces[0].TriggerCollider.transform.position;
            for(int i = 0; i < swapPlaces.Length; i++) 
            {

                Vector3 _nextTransform;
                Transform _currentTransform = swapPlaces[i].TriggerCollider.transform;

                if (i < swapPlaces.Length - 1)
                    _nextTransform = swapPlaces[i + 1].TriggerCollider.transform.position;
                else
                    _nextTransform = temp;

                _currentTransform.position = _nextTransform;

            }

            swappedCount++;
        }

        private async void StartSwapCooldown()
        {
            isOnCooldown = true;
            await Task.Delay((int)(swapCooldown * 1000));
            isOnCooldown = false;
        }

    }
}
