using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Hole"))
        {
            float rand = Random.Range(1f, 5f);
            other.GetComponent<HoleBehaviour>().AddWater(20);
        }
    }
}
