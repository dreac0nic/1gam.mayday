using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]
public class FPSController : MonoBehaviour
{
	// System Members
	public Camera Viewer;
	public bool ComponentMultiplication = false;

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
