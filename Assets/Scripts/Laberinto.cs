using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laberinto : MonoBehaviour
{
    [SerializeField]
    private GameObject paredIzq;
    [SerializeField]
    private GameObject paredDcha;
    [SerializeField]
    private GameObject paredFrontal;
    [SerializeField]
    private GameObject paredAtras;
    [SerializeField]
    private GameObject block;

    public bool IsVisited { get; private set; }

    public void Visitar()
    {
        IsVisited = true;
        block.SetActive(false);
    }

    public void ClearParedIzq()
    {
        paredIzq.SetActive(false);
    } 
    public void ClearParedDcha()
    {
        paredDcha.SetActive(false);
    }
     public void ClearParedFront()
    {
        paredFrontal.SetActive(false);
    }
      public void ClearParedAtras()
    {
        paredAtras.SetActive(false);
    }

}
