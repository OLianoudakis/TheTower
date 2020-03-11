using System.Collections;
using System.Collections.Generic;

namespace AI.Personality
{
    public class ConstantMappings
    {
        public static readonly float[,] MotivationToPersonalityTraits
            = new float[16, 5]
            {
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
        

    }
}
