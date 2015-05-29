using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]
public class FPSController : MonoBehaviour
{
	// System Members
	public Camera Viewer;

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

	private void Start()
	{
		// Collect rigidbody and collider
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Collider = GetComponent<CapsuleCollider>();

		// If no camera is explicitely assigned, attempt to find one
		if(!Viewer)
			Viewer = GetComponentInChildren<Camera>();
	}
}
