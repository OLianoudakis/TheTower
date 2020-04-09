using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextBehavior : MonoBehaviour
{
    [SerializeField]
    private float m_removeTextAfter = 3.0f;

    private float m_timer = 0.0f;
    private Camera m_cameraToLookAt;
    private TextMesh m_textMesh;

    public string text
    {
        get { return m_textMesh.text; }
    }

    public void ChangeText(string text)
    {
        m_timer = 0.0f;
        m_textMesh.text = text;
    }

    // Use this for initialization 
    private void Awake()
    {
        m_cameraToLookAt = Camera.main;
        m_textMesh = GetComponent(typeof(TextMesh)) as TextMesh;
    }

    // Update is called once per frame 
    private void LateUpdate()
    {
        transform.LookAt(m_cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(m_cameraToLookAt.transform.forward);
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_removeTextAfter)
        {
            m_textMesh.text = "";
        }
    }
}
