using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	//[RequireComponent(typeof(Rigidbody))]
	public class CharacterMovement : MonoBehaviour
	{
		public float Speed = 200f;
		public float RotationSpeed = 200f;

		private float _vertical;
		private float _horizontal;
		public Rigidbody Rigidbody;

		private void Start()
		{
			//Rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			_vertical = Input.GetAxis("Vertical");
			_horizontal = Input.GetAxis("Horizontal");

			Vector3 velocity = (transform.forward * _vertical) * Speed * Time.fixedDeltaTime;
			velocity.y = Rigidbody.velocity.y;
			Rigidbody.velocity = velocity;

			Vector3 angularVelocity = (transform.up * _horizontal) * RotationSpeed * Time.fixedDeltaTime;
			Rigidbody.angularVelocity = angularVelocity;
		}
	}
}