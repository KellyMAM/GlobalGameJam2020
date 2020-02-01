using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Log : MonoBehaviour
    {
        [HideInInspector]
        public LogState CurrentState = LogState.InField;

        private Rigidbody _rigidbody;
        private BoxCollider _capsuleCollider;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<BoxCollider>();
        }

        public void LogPlaced()
        {
            _rigidbody.isKinematic = true;
            _capsuleCollider.enabled = false;
        }
    }
}