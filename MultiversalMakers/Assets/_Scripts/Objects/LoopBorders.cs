using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class LoopBorders : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private float triggerCooldown = 0.05f;
        [SerializeField] private Vector2 colliderOffset;
        [SerializeField] private Vector2 teleportationOffset;

        private bool isOnCooldown = false;
        private Transform playerTransform;
        private Vector2[] cameraCorners;

        private void Awake()
        {
            // Set Borders
            #region Borders
            // Doing this for readability

            // Get Camera corners
            var dist = (transform.position - Camera.main.transform.position).z;

            var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x - colliderOffset.x;
            var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x + colliderOffset.x;

            var downBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y - colliderOffset.y;
            var upBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y + colliderOffset.y;
            #endregion

            // Save Camera Corners
            cameraCorners = new[] {
                new Vector2 (rightBorder, downBorder), //   1, -1
                new Vector2 (leftBorder, downBorder),  //  -1, -1
                new Vector2 (leftBorder, upBorder),    //  -1,  1
                new Vector2 (rightBorder, upBorder)    //   1,  1
            };

            GetComponent<PolygonCollider2D>().points = cameraCorners;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            if (isOnCooldown) return;
            StartCoroutine(TriggerCooldown());

            // Globally cache for convenience 
            playerTransform = collision.transform;
            Teleport(GetNewPosition());  
        }

        private Vector3 GetNewPosition()
        { 
            #region Variables
            // Doing this for readibility
            float leftBorder = cameraCorners[1].x,
                rightBorder = cameraCorners[0].x,
                upBorder = cameraCorners[1].y,
                downBorder = cameraCorners[2].y;

            bool _changeX = playerTransform.position.x < leftBorder || playerTransform.position.x > rightBorder,
                _changeY = playerTransform.position.y < downBorder || playerTransform.position.y > upBorder;


            float _newPlayerX = playerTransform.position.x,
                _newPlayerY = playerTransform.position.y;
            #endregion

            

            if (_changeX)
                _newPlayerX = (playerTransform.position.x > rightBorder ? leftBorder + teleportationOffset.x : rightBorder - teleportationOffset.x);


            else if (_changeY)
                 _newPlayerY = (playerTransform.position.y > upBorder ? upBorder + teleportationOffset.y : downBorder - teleportationOffset.y);

            print("GetPos");

            // Return new position
            return new Vector3 (_newPlayerX, _newPlayerY, 0);
        }

        private void Teleport(Vector3 _newPos)
        {
            // Foreach particle in the player gameobject, clear all particles
            ParticleSystem[] particleSystems = playerTransform.gameObject.GetComponentsInChildren<ParticleSystem>();
            List<ParticleSystem> pList = new List<ParticleSystem>();
            foreach(ParticleSystem ps in particleSystems)
            {
                if (ps.isPlaying)
                {
                    pList.Add(ps);
                    ps.Pause();
                }
            }

            // Teleport player
            playerTransform.position = _newPos;

            // Clear the trail
            playerTransform.GetComponentInChildren<TrailRenderer>().Clear();

            // Play them all again
            foreach(ParticleSystem ps in pList) {
                ps.Play();
            }
        }

        private IEnumerator TriggerCooldown()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(triggerCooldown);
            isOnCooldown = false;
        }

    }
}
