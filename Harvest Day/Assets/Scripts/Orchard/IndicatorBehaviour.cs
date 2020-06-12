using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBehaviour : MonoBehaviour
{
    private Grid grid;
    [SerializeField] int offset = 2;
    private Vector3Int worldPos;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3Int v = grid.LocalToCell(RayCastController.instance.HitPos());
        Vector3Int playerVector = grid.LocalToCell(player.position);

        if (v.x > offset + playerVector.x)
            v.x = offset + playerVector.x;
        if (v.z > offset + playerVector.z)
            v.z = offset + playerVector.z;
        if (v.x < -(offset) + playerVector.x)
            v.x = -(offset) + playerVector.x;
        if (v.z < -(offset) + playerVector.z)
            v.z = -(offset) + playerVector.z;

        worldPos = v;
        transform.localPosition = grid.GetCellCenterLocal(worldPos);
    }
}
