/*
Class used for the menu pointer.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasPointer : MonoBehaviour
{
    public float defaultLen = 3.0f;

    public EventSystem eventSystem;
    public StandaloneInputModule inputModule;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    private Vector3 CalculateEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }

    private Vector3 GetEnd()
    {
        float distance = GetCanvasDistance();
        Vector3 endPosition = CalculateEnd(defaultLen);

        if (distance != 0.0f)
        {
            endPosition = CalculateEnd(distance);
        }

        return endPosition;
    }

    private float GetCanvasDistance()
    {
        //Get data
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.inputOverride.mousePosition;

        //Raycast using data
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        //Get closest
        RaycastResult closestResult = FindFirstRaycast(results);
        float distance = closestResult.distance;

        //clamp
        distance = Mathf.Clamp(distance, 0.0f, defaultLen);
        return distance;
    }

    private RaycastResult FindFirstRaycast(List<RaycastResult> results)
    {
        foreach(RaycastResult result in results)
        {
            if (!result.gameObject)
            {
                continue;
            }

            return result;
        }

        return new RaycastResult();
    }
}
