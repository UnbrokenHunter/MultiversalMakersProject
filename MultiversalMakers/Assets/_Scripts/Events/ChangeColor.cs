using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiversalMakers
{
    public class ChangeColor : MonoBehaviour
    {

        [SerializeField] private Color[] colors;

        public void SetColor(int color) => GetComponent<SpriteRenderer>().color = colors[Mathf.Clamp(color, 0, colors.Length - 1)];

    }
}
