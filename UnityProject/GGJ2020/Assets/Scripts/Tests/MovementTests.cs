using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTests : MonoBehaviour
{
	public float rotateSpeed = 90.0f;
	public float speed = 5.0f;

	private void Update()
	{
		var rot = Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed;
		transform.Rotate(Vector3.up * rot);

		var movement = Input.GetAxis("Vertical") * Time.deltaTime * speed;
		transform.Translate(0, 0, movement);

		var transAmount = speed * Time.deltaTime;
		var rotateAmount = rotateSpeed * Time.deltaTime;

		if (Input.GetKey("up"))
		{
			transform.Translate(0, 0, transAmount);
		}
		if (Input.GetKey("down"))
		{
			transform.Translate(0, 0, -transAmount);
		}
		if (Input.GetKey("left"))
		{
			transform.Rotate(0, -rotateAmount, 0);
		}
		if (Input.GetKey("right"))
		{
			transform.Rotate(0, rotateAmount, 0);
		}
	}
}
