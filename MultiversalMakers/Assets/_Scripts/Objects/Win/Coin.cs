using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MultiversalMakers
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private UnityEvent coinEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            coinEvent?.Invoke();
        }

    }
}
