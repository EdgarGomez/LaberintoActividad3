using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad;

    private Vector3 objetivo;
    // Start is called before the first frame update
    void Start()
    {
        objetivo = puntoB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo) < 0.1f )
        {
            objetivo = (objetivo == puntoA.position) ? puntoB.position : puntoA.position;
        }
    }
}
