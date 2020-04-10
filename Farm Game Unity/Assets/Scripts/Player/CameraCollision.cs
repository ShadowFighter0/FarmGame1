using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour 
{
	public float minCamDistance = 1.0f;
	public float maxCamDistance = 4.0f;
	public float smooth = 10.0f;
	private Vector3 dollyDir;
	private float camDistance;

	public float dis_ray;
	private RaycastHit hit;
	public Transform player;

	void Awake()
	{
		dollyDir = transform.localPosition.normalized;
		camDistance = transform.localPosition.magnitude;
	}
	void Update()
	{
		Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxCamDistance);
		if (InputManager.state == InputManager.States.Working)
		{
			camDistance = minCamDistance;
		}
		else
		{
			if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
			{
				camDistance = Mathf.Clamp((hit.distance * dis_ray), minCamDistance, maxCamDistance);
			}
			else
			{
				camDistance = maxCamDistance;
			}
		}
		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * camDistance, Time.deltaTime * smooth);
	}
}
