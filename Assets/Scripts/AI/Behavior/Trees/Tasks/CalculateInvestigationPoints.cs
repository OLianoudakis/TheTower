using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees.Tasks
{
    public class CalculateInvestigationPoints : Task
    {
        private string m_pointOfInterestBlackboardKey = null; // BB key where the position to sample around is stored
        private string m_investigationPointsBlackboardKey = null; // BB key where the investigation points will be stored
        private Vector3 m_pointOfInterest; // position to sample around
        private float m_radius; // radius to sample around the point
        private int m_numberOfPoints; // number of generated investigation points
        private int m_numberOfTries; // max number of times to try to find a random point


        public CalculateInvestigationPoints(
            string pointOfInterestBlackboardKey, string investigationPointsBlackboardKey, int numberOfPoints, float radius = 2.0f, int numberOfTries = 2)
            : base("CalculateInvestigationPoints")
        {
            m_pointOfInterestBlackboardKey = pointOfInterestBlackboardKey;
            m_investigationPointsBlackboardKey = investigationPointsBlackboardKey;
            m_radius = radius;
            m_numberOfPoints = numberOfPoints;
            m_numberOfTries = numberOfTries;
        }

        public CalculateInvestigationPoints(
            Vector3 pointOfInterest, string investigationPointsBlackboardKey, int numberOfPoints, float radius = 2.0f, int numberOfTries = 2)
            : base("CalculateInvestigationPoints")
        {
            m_pointOfInterest = pointOfInterest;
            m_investigationPointsBlackboardKey = investigationPointsBlackboardKey;
            m_radius = radius;
            m_numberOfPoints = numberOfPoints;
            m_numberOfTries = numberOfTries;
        }

        protected override void DoStart()
        {
            if (m_pointOfInterestBlackboardKey != null)
            {
                if (!Blackboard.Isset(m_pointOfInterestBlackboardKey))
                {
                    this.Stopped(false);
                }
                m_pointOfInterest = (Vector3)Blackboard.Get(m_pointOfInterestBlackboardKey);
            }
            Vector3[] positions = new Vector3[m_numberOfPoints];
            int index = 0;
            for (int i = 0; i < m_numberOfPoints; i++)
            {
                int numberOfTries = 0;
                while (numberOfTries < m_numberOfTries)
                {
                    Vector3 unitSphere = UnityEngine.Random.insideUnitSphere;
                    Vector3 randomPoint =
                        m_pointOfInterest
                        + (new Vector3(unitSphere.x, 0.0f, unitSphere.z) * m_radius);
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        positions[index++] = hit.position;
                        break;
                    }
                    ++numberOfTries;
                }
            }
            if (index == 0)
            {
                positions = new Vector3[] { m_pointOfInterest };
            }
            else if(index != m_numberOfPoints)
            {
                Vector3[] tmpPositions = new Vector3[index];
                for (int i = 0; i < index; i++)
                {
                    tmpPositions[i] = positions[i];
                }
                positions = tmpPositions;
            }

            Blackboard.Set(m_investigationPointsBlackboardKey, positions);
            this.Stopped(true);
        }

        protected override void DoStop()
        {
            this.Stopped(false);
        }
    }
}
