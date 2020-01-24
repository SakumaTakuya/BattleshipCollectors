using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace Sakkun.DOTS
{
    public struct LocalPosition : IComponentData
    {
        public float3 Value;
    }
}