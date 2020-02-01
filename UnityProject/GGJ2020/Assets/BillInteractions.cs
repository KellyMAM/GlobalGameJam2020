using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BillInteractions : MonoBehaviour
{
	public float Speed = 500f;
	private Rigidbody _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Logs" && Input.GetKey(KeyCode.Space))
		{
			if (collision.rigidbody != null)
			{
				Vector3 velocity = collision.GetContact(0).normal * Speed * Time.fixedDeltaTime;
				velocity.y = collision.rigidbody.velocity.y;
				collision.rigidbody.velocity = _rigidbody.velocity;
				Debug.Log("Hold obj");
			}
		}
	}
}
