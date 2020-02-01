using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	[RequireComponent(typeof(Rigidbody))]
	public class CharacterMovement : MonoBehaviour
	{
		public float Speed = 200f;
		public float RotationSpeed = 200f;

		private float _vertical;
		private float _horizontal;
		private Rigidbody _rigidbody;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			_vertical = Input.GetAxis("Vertical");
			_horizontal = Input.GetAxis("Horizontal");

			Vector3 velocity = (transform.forward * _vertical) * Speed * Time.fixedDeltaTime;
			velocity.y = _rigidbody.velocity.y;
			_rigidbody.velocity = velocity;

			Vector3 angularVelocity = (transform.up * _horizontal) * RotationSpeed * Time.fixedDeltaTime;
			_rigidbody.angularVelocity = angularVelocity;
		}
	}
}