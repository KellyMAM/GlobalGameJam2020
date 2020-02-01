using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementTests : MonoBehaviour
{
	public float Speed = 150f;
	public float RotationSpeed = 200f;

	private float _vertical;
	private float _horizontal;
	private Rigidbody _rigidbody;

	[SerializeField]
	private BoxCollider _beavetMouth;

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

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Logs" && Input.GetKey(KeyCode.Space))
		{
			Debug.Log("Hold obj");
		}
	}
}