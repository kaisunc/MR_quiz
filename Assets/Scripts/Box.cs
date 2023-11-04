using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerClickHandler,IDragHandler,  IBeginDragHandler, IEndDragHandler
{
    public event Action<Box> EventSelected;
    void Awake(){
        EventSelected += Manager.Instanse.BoxSelected;
    }

    public void Select(){
        // Material m = GetComponent<MeshRenderer>().material;
        // m.color = Color.red;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Manager.selected = eventData.selectedObject;
        EventSelected(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("drag handler");
        Vector2 t = eventData.delta;
        transform.position += new Vector3(t.x, 0f, t.y);
        
    }
}
