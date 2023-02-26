using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class LoopBorders : MonoBehaviour
    {
        public enum Direction { Horizontal, Vertical };
        public Direction dir = Direction.Vertical;

        private Transform Border1, Border2;
       
        private float coord1, coord2;

        private void Awake()
        {
            Border1 = transform.GetChild(0);
            Border2 = transform.GetChild(1);
            if (dir == Direction.Vertical)
            {
                coord1 = Border1.position.y;
                coord2 = Border2.position.y;
            }
            else
            {
                coord1 = Border1.position.x;
                coord2 = Border2.position.x;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CollisionExit(Collider2D collider, Transform border)
        {
            if (dir == Direction.Vertical)
            {
                if (border == Border1)
                {
                    float coordDif = coord1 - collider.transform.position.y;
                    if (coordDif < 0) return;
                    Vector3 newPos = new Vector3(collider.transform.position.x, coord2 + coordDif, collider.transform.position.z);
                    teleport(collider.gameObject, newPos);
                }
                else if (border == Border2)
                {
                    float coordDif = coord2 - collider.transform.position.y;
                    if (coordDif > 0) return;
                    Vector3 newPos = new Vector3(collider.transform.position.x, coord1 + coordDif, collider.transform.position.z);
                    teleport(collider.gameObject, newPos);
                }
            }
            else
            {
                if (border == Border1)
                {
                    float coordDif = coord1 - collider.transform.position.x;
                    if (coordDif < 0) return;
                    Vector3 newPos = new Vector3(coord2 + coordDif, collider.transform.position.y, collider.transform.position.z);
                    teleport(collider.gameObject, newPos);
                }
                else if (border == Border2)
                {
                    float coordDif = coord2 - collider.transform.position.x;
                    if (coordDif > 0) return;
                    Vector3 newPos = new Vector3(coord1 + coordDif, collider.transform.position.y, collider.transform.position.z);
                    teleport(collider.gameObject, newPos);
                }
            }

            
        }

        private void teleport(GameObject obj, Vector3 newPos)
        {
            List<ParticleSystem> parts = new List<ParticleSystem>();
            foreach (ParticleSystem ps in obj.GetComponentsInChildren<ParticleSystem>(false))
            {
                if (ps.isPlaying == true) 
                {
                    ps.Stop();
                    parts.Add(ps);
                    Debug.Log(ps.gameObject.name + " " + ps.isPlaying);
                }
            }
            obj.transform.position = newPos;
            foreach (TrailRenderer tr in obj.GetComponentsInChildren<TrailRenderer>(false)) tr.Clear();
            foreach (ParticleSystem ps in parts) ps.Play();
        }

    }
}
