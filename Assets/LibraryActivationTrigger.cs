using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryActivationTrigger : MonoBehaviour
{
    public Animator m_bookcaseAnim;
    public Animator m_frontWallAnim;
    public Animator m_backWallAnim;
    public Animator m_starcaseAnim;

    public GameObject m_invisibleWall;

    private string m_animationState = "AnimationState";
    private int m_animationActivationNumber = 1;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            BookcaseHiddenDoorEvent();
        }
    }

    private void BookcaseHiddenDoorEvent()
    {
        //Activate Animation
        m_bookcaseAnim.SetInteger(m_animationState, m_animationActivationNumber);
        m_frontWallAnim.SetInteger(m_animationState, m_animationActivationNumber);
        m_backWallAnim.SetInteger(m_animationState, m_animationActivationNumber);
        m_starcaseAnim.SetInteger(m_animationState, m_animationActivationNumber);

        //DisableInvisibleWalls
        m_invisibleWall.SetActive(false);
    }
}
