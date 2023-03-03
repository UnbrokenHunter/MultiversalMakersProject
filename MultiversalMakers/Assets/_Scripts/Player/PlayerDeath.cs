using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class PlayerDeath : MonoBehaviour
    {
        static List<PlayerDeath> players = new List<PlayerDeath>(); 
        private Vector3 initialPos;
        private Quaternion initialRot;
        public LayerMask deathMask;
        
        // Start is called before the first frame update
        void Start()
        {
            initialPos = transform.position;
            initialRot = transform.rotation;
            players.Add(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.gameObject.layer == 8)
            {
                killAll();
            }
        }

        public static void killAll()
        {
            //if we want a blackscreen flash on death, turn it on here
            int removeCount = players.Count;
            foreach (PlayerDeath p in players)
            {
                p.Die();
            }
            players.RemoveRange(0, removeCount);
            // and off here
        }
        public void Die()
        {
            Instantiate(this.gameObject, initialPos, initialRot);
            //could spawn player death particles & sfx here
            Destroy(this.gameObject);
        }
    }
}
