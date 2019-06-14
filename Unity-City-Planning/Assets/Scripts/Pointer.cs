using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float m_DefaultLength = 5.0f;
    public GameObject m_Dot;
    public VRInputModule m_InputModule;

    private LineRenderer m_LineRender = null;

    private void Awake()
    {
        m_LineRender = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        // use default length or distance from input module
        float targetLength = m_DefaultLength;

        // raycast call create
        RaycastHit hit = CreateRaycast(targetLength);

        // setup default end
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        // or setup end on hit
        if(hit.collider != null)
        {
            endPosition = hit.point;
        }

        // set position of the dot
        m_Dot.transform.position = endPosition;

        // set postions of line renderer
        m_LineRender.SetPosition(0, transform.position);
        m_LineRender.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);

        return hit;
    }

}
