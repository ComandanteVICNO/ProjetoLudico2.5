using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy1 == null && enemy2 == null)
        {
            Debug.Log("empty");
        }
        else
        {
            Debug.Log("They got smth");
        }
    }
}
