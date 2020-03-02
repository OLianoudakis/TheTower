using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector3 m_leftMouseClickPosition = Vector3.zero;

    private bool m_initialized = false;
    private RaycastHit m_leftMouseClickHit;
    private bool m_isLeftMouseClick = false;

    public bool initialized
    {
        get { return m_initialized; }
    }

    public bool isLeftMouseClick
    {
        get { return m_isLeftMouseClick; }
    }

    public Vector3 leftMouseClickPosition
    {
        get { return m_leftMouseClickPosition; }
    }

    public RaycastHit leftMouseClickHit
    {
        get { return m_leftMouseClickHit; }
    }

    private void Update()
    {
        if (!m_initialized)
        {
            m_initialized = true;
            m_leftMouseClickPosition = transform.position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            m_isLeftMouseClick = true;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                m_leftMouseClickPosition = hit.point;
                m_leftMouseClickHit = hit;
            }
        }
        else 
        {
            m_isLeftMouseClick = false;
        }
    }
}
