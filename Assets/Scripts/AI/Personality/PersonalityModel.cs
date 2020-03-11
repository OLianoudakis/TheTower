using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AI.Personality
{
    [Serializable]
    public struct PersonalityTraitEntry
    {
        public PersonalityTraits m_personalityTraitType;
        public float m_value;

        public PersonalityTraitEntry(PersonalityTraits personalityTraitType, float value)
        {
            m_personalityTraitType = personalityTraitType;
            m_value = value;
        }
    }

    [CreateAssetMenu(fileName = "New Personality Model", menuName = "Personality Model")]
    public class PersonalityModel : ScriptableObject
    {
        [SerializeField]
        public PersonalityTraitEntry[] m_personalityTraitsValues 
            = new PersonalityTraitEntry[5]
            {
                new PersonalityTraitEntry(PersonalityTraits.Agreeableness, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Conscientiousness, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Extraversion, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Neuroticism, 0.0f),
                new PersonalityTraitEntry(PersonalityTraits.Openness, 0.0f)
            };
    }
}
