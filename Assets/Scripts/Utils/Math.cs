using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace Sakkun.Utils
{
    public static class Math
    {
        public const float MAX_ANGLE = 360f;

        public static float ClampAngle(float angle, float min, float max)
            => clamp(abs(angle) > MAX_ANGLE ? angle - sign(angle) * MAX_ANGLE : angle, min, max); 
    }
}
