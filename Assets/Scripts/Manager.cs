using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{
    public static Manager Instanse {get; private set;}
    public static GameObject selected;
    public List<Box> boxes = new List<Box>();
    
    void Awake()
    {

        if (Instanse == null)
        {
            Instanse = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void BoxSelected(Box box){
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].GetComponent<MeshRenderer>().material.color = Color.grey;
        }

        box.GetComponent<MeshRenderer>().material.color = Color.red;
        
    }


}
