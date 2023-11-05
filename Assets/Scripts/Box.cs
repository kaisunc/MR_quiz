using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler 
{
    public event Action<Box> EventSelected;
    public event Action<Box> EventUnSelect;
    public bool _selected;

    void Start()
    {
        EventSelected += Manager.Instanse.Select;
        EventUnSelect += Manager.Instanse.UnSelect;
        if (name.Contains("uni"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_selected)
        {
            GetComponent<MeshRenderer>().enabled = false;
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(eventData.position);
            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 2000f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Ground"))
                {
                    Vector3 n = new Vector3(hits[i].point.x, transform.position.y, hits[i].point.z);
                    transform.position = n;
                }
            }
            Manager.Instanse.DoOp();
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (name.Contains("sub"))
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            if (Manager.SelectedObject == null) // don't highlight other when dragging
            {
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public IEnumerator Fade()
    {
        float duration = 1f;
        float currentTime = 0;
        MeshRenderer mr;
        if (name.Contains("uni"))
        {
            mr = transform.GetChild(0).GetComponent<MeshRenderer>();
        }
        else
        {
            mr = GetComponent<MeshRenderer>();
        }
        mr.enabled = true;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float v = 1 - currentTime;
            mr.material.color = new Color(1f, 0f, 0f, v);
            yield return null;
        }
        mr.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (name.Contains("sub"))
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Manager.SelectedObject = eventData.selectedObject;
        EventSelected(this);
    }

}
