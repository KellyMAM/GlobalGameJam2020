using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    [RequireComponent(typeof(Rigidbody))]
    public class Log : MonoBehaviour
    {
        [HideInInspector]
        public LogState CurrentState = LogState.InField;

        private Rigidbody _rigidbody;
        private BoxCollider _capsuleCollider;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponentInChildren<BoxCollider>();
        }

        public void LogPlaced()
        {
            _rigidbody.isKinematic = true;
            _capsuleCollider.enabled = false;
            RaiseToWater w = gameObject.AddComponent<RaiseToWater>();
            w.useOffset = false;
        }
    }
}