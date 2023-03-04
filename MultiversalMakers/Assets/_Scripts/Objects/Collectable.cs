using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class Collectable : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.Instance.AddStar();

            MasterAudio.PlaySound("Collect");

            GetComponent<MMF_Player>().PlayFeedbacks();
        }

    }
}
