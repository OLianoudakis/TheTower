﻿using System.Collections;
using System.Collections.Generic;

namespace AI.Personality
{
    public class ConstantMappings
    {
        public static readonly float[,] MotivationToPersonalityTraits
            = new float[16, 5]
            {   //  O     C     E     A     N
                {  0.0f, 0.0f, 0.0f, 0.0f, 0.5f },
                {  0.46f, 0.0f, 0.0f, 0.0f, 0.0f },
                {  0.0f, 0.0f, 0.0f, 0.0f, 0.25f },
                {  0.0f, 0.21f, 0.0f, 0.22f, 0.0f },
                {  0.0f, 0.31f, 0.0f, 0.18f, 0.0f },
                {  0.17f, 0.24f, 0.0f, 0.3f, 0.0f },
                {  0.0f, 0.0f, 0.0f, -0.29f, 0.5f },
                {  -0.19f, 0.0f, 0.0f, 0.0f, 0.33f },
                {  0.17f, 0.24f, 0.0f, 0.3f, 0.0f },
                {  0.0f, 0.0f, 0.39f, -0.18f, 0.0f },
                {  0.0f, 0.0f, 0.0f, -0.23f, 0.0f },
                {  0.0f, 0.0f, 0.0f, -0.16f, 0.28f },
                {  0.2f, 0.0f, 0.58f, 0.0f, 0.0f },
                {  0.0f, 0.0f, 0.19f, -0.28f, 0.24f },
                {  0.0f, 0.0f, 0.0f, 0.0f, 0.46f },
                {  0.0f, 0.28f, 0.0f, -0.61f, 0.31f }
            };

        public static readonly float[,] MoodToPersonalityTraits
            = new float[3, 5]
            {
                //  O     C     E     A     N
                {  0.0f, 0.0f, 0.21f, 0.59f, 0.19f },  // P
                {  0.15f, 0.0f, 0.0f, 0.3f, 0.57f },   // A
                {  0.25f, 0.17f, 0.6f, 0.32f, 0.0f }   // D
            };

        public static readonly float[,] MoodToEmotion
            = new float[3, 21]
            {
                {  0.4f, -0.4f, -0.2f, -0.4f, 0.2f, -0.64f, 0.3f, 0.2f, -0.3f, 0.4f, 0.5f,
                    -0.3f, -0.3f, 0.4f, -0.4f, 0.4f, -0.51f, 0.6f, -0.3f, 0.3f, -0.6f },  // P
                {  0.2f, -0.2f, -0.3f, -0.2f, 0.2f, 0.60f, -0.2f, -0.3f, 0.1f, 0.3f, 0.3f,
                    0.1f, -0.1f, 0.16f, 0.2f, 0.2f, 0.59f, 0.5f, 0.1f, 0.1f, 0.6f },  // A
                {  0.1f, -0.5f, -0.2f, -0.5f, -0.1f, -0.43f, 0.4f, 0.4f, -0.4f, 0.3f, -0.2f,
                    -0.6f, 0.4f, -0.24f, 0.1f, -0.3f, 0.25f, 0.4f, -0.6f, 0.2f, 0.3f }  // D
            };

        public static readonly List<MotivationDesires>[] EmotionToMotivation
            = new List<MotivationDesires>[21]
            {
                new List<MotivationDesires>(){MotivationDesires.Curiosity,  MotivationDesires.SocialContact, MotivationDesires.Eating},
                new List<MotivationDesires>(){MotivationDesires.SocialContact, MotivationDesires.Family, MotivationDesires.Idealism},
                new List<MotivationDesires>(){MotivationDesires.Saving, MotivationDesires.SocialStatus},
                new List<MotivationDesires>(){MotivationDesires.Idealism},
                new List<MotivationDesires>(){MotivationDesires.Idealism, MotivationDesires.Curiosity},
                new List<MotivationDesires>(){MotivationDesires.Tranquility, MotivationDesires.Order},
                new List<MotivationDesires>(){MotivationDesires.Saving},
                new List<MotivationDesires>(){MotivationDesires.Tranquility, MotivationDesires.Acceptance},
                new List<MotivationDesires>(){MotivationDesires.SocialContact, MotivationDesires.Family, MotivationDesires.Idealism},
                new List<MotivationDesires>(){MotivationDesires.SocialStatus, MotivationDesires.Honor},
                new List<MotivationDesires>(){MotivationDesires.SocialContact, MotivationDesires.Honor, MotivationDesires.Idealism},
                new List<MotivationDesires>(){MotivationDesires.Acceptance, MotivationDesires.SocialContact},
                new List<MotivationDesires>(){MotivationDesires.Vengeance},
                new List<MotivationDesires>(){MotivationDesires.Family, MotivationDesires.SocialContact, MotivationDesires.Romance},
                new List<MotivationDesires>(){MotivationDesires.Vengeance, MotivationDesires.Power},
                new List<MotivationDesires>(){MotivationDesires.SocialContact, MotivationDesires.Idealism},
                new List<MotivationDesires>(){MotivationDesires.Vengeance, MotivationDesires.Power, MotivationDesires.Idealism},
                new List<MotivationDesires>(){MotivationDesires.Acceptance, MotivationDesires.SocialStatus, MotivationDesires.Honor},
                new List<MotivationDesires>(){MotivationDesires.Acceptance, MotivationDesires.Idealism, MotivationDesires.Tranquility},
                new List<MotivationDesires>(){MotivationDesires.Family, MotivationDesires.SocialContact, MotivationDesires.Romance},
                new List<MotivationDesires>(){MotivationDesires .Vengeance, MotivationDesires.Power}
            };

        public static readonly int[,] EmotionToPersonalityTraits
            = new int[21, 5]
            {
                // O  C  E  A  N
                {  1, 0, 1, 0, 0 },
                {  1, 0, 0, 0, 1 },
                {  1, 1, 0, -1, 1 },
                {  1, 0, 0, 1, 1},
                {  0, 0, 1, 0, 0 },
                {  -1, 1, 0, 0, 1 },
                {  1, 0, 1, 0, 0 },
                {  0, 0, 1, 0, 0 },
                {  -1, 0, 0, 0, 1 },
                {  1, 0, 1, 0, 0 },
                {  1, 0, 1, 1, 0 },
                {  -1, 0, 0, 0, 1 },
                {  0, 0, 0, -1, 1 },
                {  1, 0, 1, 1, 0 },
                {  0, 0, 0, -1, 1 },
                {  1, 0, 1, 0, 0 },
                {  0, 0, 0, -1, 1 },
                {  1, 0, 1, 0, 0 },
                {  1, 0, 0, 0, 1 },
                {  1, 0, 1, 0, 0 },
                {  1, 0, 0, -1, 1 }
            };

        public static readonly int[,] MoodOctants
            = new int[8, 3]
        {
            { 1, 1, 1},
            { 1, 1, -1},
            { 1, -1, 1},
            { 1, -1, -1},
            { -1, -1, -1},
            { -1, -1, 1},
            { -1, 1, -1},
            { -1, 1, 1}
        };
    }
}