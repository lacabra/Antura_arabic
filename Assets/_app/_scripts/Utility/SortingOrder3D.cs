﻿using UnityEngine;

// refactor: move this to Editor utilities
namespace EA4S.Utilities
{
    /// <summary>
    /// Allows to set the sortingOrder of 3D objects in the world, simply setting the order (no layerName/Id)
    /// </summary>
    [ExecuteInEditMode]
    public class SortingOrder3D : MonoBehaviour
    {
        public int SortingOrder;

        void Awake()
        {
            this.GetComponent<Renderer>().sortingOrder = SortingOrder;
        }

#if UNITY_EDITOR
        Renderer rend;
        void Update()
        {
            if (Application.isPlaying) return;

            if (rend == null) rend = this.GetComponent<Renderer>();
            if (rend.sortingOrder != SortingOrder) rend.sortingOrder = SortingOrder;
        }
#endif
    }
}