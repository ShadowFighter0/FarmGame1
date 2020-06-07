using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager Instance;

    private Queue<CarController> carTypes = new Queue<CarController>();
    private Transform[] path;
    private Transform[] inversePath;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;   
        
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            carTypes.Enqueue(transform.GetChild(0).GetChild(i).GetComponent<CarController>());
        }

        path = new Transform [transform.GetChild(1).transform.childCount];
        inversePath = new Transform[transform.GetChild(1).transform.childCount];

        for (int i = 0, j = path.Length - 1; i < path.Length; i++, j--)
        {
            path[i] = transform.GetChild(1).GetChild(i); 
            inversePath[j] = transform.GetChild(1).GetChild(i);
        }

        StartCoroutine(NewCar(0));

    }

    public void EndRoute (CarController obj)
    {
        StartCoroutine(NewCar(Random.Range(10, 60)));
    }

    IEnumerator NewCar(int time)
    {
        Debug.Log(time);
        yield return new WaitForSeconds(time);
        int direction = Random.Range(0,1);
        int gonnaBuy = Random.Range(1,1);

        CarController script =  carTypes.Dequeue();
        Debug.Log(script.name);
        if (direction == 0)
        {
            if (gonnaBuy == 0)
            {
                script.TurnOn(path, false);
            }
            else
            {
                script.TurnOn(path, true);
            }
            
        }
        else
        {
            if (gonnaBuy == 0)
            {
                script.TurnOn(inversePath, false);
            }
            else
            {
                script.TurnOn(inversePath, true);
            }
        }
        
        carTypes.Enqueue(script);
    }
}
