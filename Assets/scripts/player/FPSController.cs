using System.Collections;
﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]
public class FPSController : MonoBehaviour
{
	// System Members
	// TODO: Add layerMask to control what layers identify ground or "walkable" collisions.
	public Camera Viewer;

	// DEBUG
	public Text PlayerInformation;
	public bool ComponentMultiplication = false;

	// Player adjustments.
	public float MouseSensitivity = 1.0f;

	// Movement Members
	// TODO: Test and create defaults appropriate for normal circumstances.
	public float MovementSpeed = 6.0f;
	public float RunMultiplier = 1.5f;
	public float SneakMultiplier = 0.7f;
	public float CrouchMultiplier = 0.35f;
	public float JumpStrength = 20f;

	private Rigidbody m_Rigidbody;
	private CapsuleCollider m_Collider;
	private bool m_IsGrounded;
	private Quaternion m_PlayerRotation;
	private Quaternion m_CameraRotation;

	private void Start()
	{
		// Collect rigidbody and collider
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Collider = GetComponent<CapsuleCollider>();

		// If no camera is explicitely assigned, attempt to find one
		if(!Viewer)
			Viewer = GetComponentInChildren<Camera>();

		// Get rotations for player and camera.
		m_PlayerRotation = this.transform.localRotation;
		m_CameraRotation = Viewer.transform.localRotation;
	}

	private void Update()
	{
		string debugText = "PLAYER INFORMATION\n";

		debugText += "velocity: " + m_Rigidbody.velocity.ToString() + "\n";
		debugText += "ground: " + m_IsGrounded + "\n";

		PlayerInformation.text = debugText;

		// Rotate player with mouse.
		float oldRotation = transform.eulerAngles.y;
		Vector2 mouseRotations = new Vector2(Input.GetAxis("Mouse X")*MouseSensitivity, Input.GetAxis("Mouse Y")*MouseSensitivity);

		// TODO: Add camera X clamping to keep player from looking too high or low.
		m_PlayerRotation *= Quaternion.Euler(0f, mouseRotations.x, 0f);
		m_CameraRotation *= Quaternion.Euler(-mouseRotations.y, 0f, 0f);

		// TODO: Add smoothing and acceleration if they REALLY want them.
		this.transform.localRotation = m_PlayerRotation;
		Viewer.transform.localRotation = m_CameraRotation;
	}

	private void FixedUpdate()
	{
		// TODO: Ground Check
		m_IsGrounded = true;

		// Get direction vector based on input.
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"),
		                            Input.GetAxis("Vertical"));

		// Calculate new movement direction/speed.
		if((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_IsGrounded) {
			Vector3 movement = Vector3.ProjectOnPlane(Viewer.transform.forward*input.y + Viewer.transform.right*input.x, new Vector3(0f, 1f, 0f)).normalized;

			// Add speed to movement direction.
			if(ComponentMultiplication) {
				movement.x *= MovementSpeed;
				movement.y *= MovementSpeed;
				movement.z *= MovementSpeed;
			} else {
				movement *= MovementSpeed;
			}

			// FIXME: Keep player from pushing over slopes too steep (animation slow? :DDDD)
			if(m_Rigidbody.velocity.sqrMagnitude < (MovementSpeed*MovementSpeed))
				m_Rigidbody.AddForce(movement, ForceMode.Impulse);
		}

		// Check if grounded.
		// TODO: Add air/jump code.
		if(m_IsGrounded) {
			m_Rigidbody.drag = 5f; // FIXME: Change to use Rigidbody's original drag, allowing editor customization.
		} else {
			m_Rigidbody.drag = 0f;
		}
	}
}
