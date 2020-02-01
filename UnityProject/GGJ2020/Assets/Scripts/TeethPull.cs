using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	[RequireComponent(typeof(SphereCollider))]
	public class TeethPull : MonoBehaviour
	{
		public float Speed = 200f;
		public Rigidbody HeadRigidbody;

		private void Start()
		{
			HeadRigidbody = GetComponent<Rigidbody>();
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag == "Logs" && Input.GetKey(KeyCode.Space))
			{
				if (other.attachedRigidbody != null)
				{
					RaycastHit hit;
					if (Physics.Raycast(transform.position, transform.forward, out hit))
					{
						Vector3 normal = hit.normal;
						Vector3 velocity = normal * Speed * Time.fixedDeltaTime;
						velocity.y = other.attachedRigidbody.velocity.y;
						other.attachedRigidbody.velocity = velocity;
						Debug.Log("Hold obj");
					}
				}
			}
		}
	}
}