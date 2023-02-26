using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    
    public class BorderCollider : MonoBehaviour
    {
        private LoopBorders parent;

        private void Awake()
        {
            parent = transform.parent.gameObject.GetComponent<LoopBorders>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //parent.CollisionEnter(collision, transform);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
        //    parent.CollisionExit(collision, transform);
        }
    }
}
