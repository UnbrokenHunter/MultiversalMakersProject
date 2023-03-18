using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class StarTracker : MonoBehaviour
    {
        public static StarTracker Instance;

        [SerializeField] private int[] starTracker;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        [Button]
        public void SetLevel(int level, int count)
        {
            if(level - 1 <= starTracker.Length)
            {
                if (count > starTracker[level - 1])
                    starTracker[level - 1] = count;
            }
        }

        public int GetLevelCount(int level) => starTracker[level];

    }
}
