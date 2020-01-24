using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace Sakkun.DOTS
{
    public struct MouseControled : IComponentData
    {
        public float Sensitivity;
        public float Yaw;
        public float Pitch;
    }
}
