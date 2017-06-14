﻿using UnityEngine;
using System.Collections;

namespace EA4S.Core {

    public class Singleton<T> : MonoBehaviour where T : Component
    {
        // Static singleton property
        public static T Instance { get; private set; }
        public string TypeName { get; private set; }

        protected bool IsDuplicatedInstance = false;

        protected virtual void Awake() {
            TypeName = typeof(T).FullName;

            // checks if there is already another instance of this type.
            if (Instance != null) {
                if (Instance != this) {
                    // destroys immediately to break the chain of events associated to this object.
                    //DestroyImmediate(gameObject);
                    IsDuplicatedInstance = true;
                    Destroy(gameObject);
                }
                return;             
            }

            // Here we save our singleton instance
            Instance = this as T;

            Initialise();
        }

        void OnDestroy() {
            if (Instance == this)
                Finalise();
        }

        protected virtual void Initialise() {
        }

        protected virtual void Finalise() {
        }
    }
}
