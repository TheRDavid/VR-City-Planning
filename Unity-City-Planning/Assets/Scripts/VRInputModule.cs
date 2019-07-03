using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
    public Camera m_Camera;
    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_CurrentObject = null;

    // maybe implment subclass of pointer event data to include building
    private PointerEventData m_Data = null;

    protected override void Awake()
    {
        base.Awake();

        m_Data = new PointerEventData(eventSystem);
    }

    // equivalent of update method
    public override void Process()
    {
        // reset data
        m_Data.Reset();
        m_Data.position = new Vector2(m_Camera.pixelWidth/2, m_Camera.pixelHeight/2);

        // setup raycast
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

        // clear raycast
        m_RaycastResultCache.Clear();

        // handle hover states on objects
        HandlePointerExitAndEnter(m_Data, m_CurrentObject);

        // press input
        if (m_ClickAction.GetStateDown(m_TargetSource))
        {
            processRelease(m_Data);
        }

        // release input
        if (m_ClickAction.GetStateUp(m_TargetSource))
        {
            processRelease(m_Data);
        }
    }

    public PointerEventData getData()
    {
        return m_Data;
    }

    private void processPress(PointerEventData data)
    {

    }

    private void processRelease(PointerEventData data)
    {

    }

}
