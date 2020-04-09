using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour 
{
	public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;
	private Vector3 dollyDir;
	private float distance;

	public float dis_ray;
	private RaycastHit hit;
	public Transform player;
	void Awake()
	{
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}

	void Update()
	{
		Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
		if(InputManager.instance.editing)
		{
			distance = minDistance;
		}
		else
		{
			if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
			{
				distance = Mathf.Clamp((hit.distance * dis_ray), minDistance, maxDistance);
			}
			else
			{
				distance = maxDistance;
			}
		}
		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
	}
}
