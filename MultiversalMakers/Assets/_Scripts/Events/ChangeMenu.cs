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

        public void TriggerMenuEventAfter(float time) => StartCoroutine(MenuEventWaiter(time));

        private IEnumerator MenuEventWaiter(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            onMenuChangeEvent?.Invoke();
        }
    }
}
