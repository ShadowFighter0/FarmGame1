using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    private Vector3 rotation = Vector3.up * 0.3f;
    void Update()
    {
        transform.Rotate(Time.deltaTime * rotation);
    }
}
