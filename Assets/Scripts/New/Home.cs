using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] GameObject obj2;


    public void Click()
    {
        obj.SetActive(true);
        obj2.SetActive(false);
    }
}
