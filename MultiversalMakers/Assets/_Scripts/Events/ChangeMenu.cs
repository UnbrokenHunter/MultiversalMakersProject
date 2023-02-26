using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MultiversalMakers
{

    public class ChangeMenu : MonoBehaviour
    {
        [SerializeField] private UnityEvent onMenuChangeEvent;

        public void SetCurrentSelectedGameObject(GameObject newSelectedObject) 
            => EventSystem.current.SetSelectedGameObject(newSelectedObject);

        public void TriggerMenuEvent() => onMenuChangeEvent?.Invoke();
    }
}
