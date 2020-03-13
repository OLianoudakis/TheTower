using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior
{
    public class BehaviorManager : MonoBehaviour
    {
        [SerializeField]
        private float m_emotionIntensityTreshold = 0.6f;

        private float[] m_finishedMotivations;

        public float[] finishedMotivations
        {
            get { return m_finishedMotivations; }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
