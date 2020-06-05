using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager Instance;

    private List<CarController> carTypes = new List<CarController>();
    private Transform[] path;
    private Transform[] inversePath;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;   
        
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            carTypes.Add(transform.GetChild(0).GetChild(i).GetComponent<CarController>());
        }

        path = new Transform [transform.GetChild(1).transform.childCount];
        inversePath = new Transform[transform.GetChild(1).transform.childCount];

        for (int i = 0, j = path.Length - 1; i < path.Length; i++, j--)
        {
            path[i] = transform.GetChild(1).GetChild(i); 
            path[j] = transform.GetChild(1).GetChild(i);
        }

    }

    void Update()
    {
        //trafico normal 
    }

    public void DeployCar(bool pathDirection, bool gonnaBuy)
    {
        if (pathDirection == true)
        {

        }
        else
        {

        }
    }
}
