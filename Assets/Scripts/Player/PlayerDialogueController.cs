using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementController m_playerMovementController;

    private POIBehavior m_currentPOI;

    public void GetPOIBehavior(POIBehavior poi)
    {
        m_currentPOI = poi;
        ActivatePOI();
    }

    public void SwitchToMovement()
    {
        m_playerMovementController.enabled = true;
        this.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextMessage();
        }
    }

    private void ActivatePOI()
    {
        m_currentPOI.ActivatePOIBehavior(this);
    }

    private void ShowNextMessage()
    {
        m_currentPOI.ShowNextMessage();
    }
}
