using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLife : MonoBehaviour
{
    private int life = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TDamage()
    {
        life--;
        if(life==0)
        {
            Destroy(gameObject);
        }
    }
}
