using UnityEngine;


namespace Utils
{
    public class TextMeshSortingLayerAssigner : MonoBehaviour
    {
        [SerializeField]
        private string sortingLayer;

        [SerializeField]
        private int sortingOrder;


        private void Awake()
        {
            MeshRenderer r = GetComponent<MeshRenderer>();

            r.sortingLayerName = sortingLayer;
            r.sortingOrder = sortingOrder;
        }
    }
}
