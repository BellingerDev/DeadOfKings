using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    public class RendererHiddenPropertiesAssigner : MonoBehaviour
    {
        [SerializeField]
        private string sortingLayer;

        [SerializeField]
        private int sortingOrder;


        private void Awake()
        {
            if (!string.IsNullOrEmpty(sortingLayer))
            {
                Renderer r = GetComponent<Renderer>();

                r.sortingLayerName = sortingLayer;
                r.sortingOrder = sortingOrder;
            }
        }
    }
}
