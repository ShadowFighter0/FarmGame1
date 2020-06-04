using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager Instance;

    private List<CarController> carTypes;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;    
        for(int i = 0; i < transform.childCount; i++)
        {
            carTypes.Add(transform.GetChild(i).GetComponent<CarController>());
        }
    }

    public void DeployCar (bool pathDirection, bool gonnaBuy)
    {
        if(pathDirection == true)
        {
            
        }

        //uno q va a comprar
    }

    // Update is called once per frame
    void Update()
    {
        //trafico normal 
    }
}
