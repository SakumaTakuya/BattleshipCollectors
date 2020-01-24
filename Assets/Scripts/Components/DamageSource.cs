using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace Sakkun.DOTS
{
    public struct DamageSource : IComponentData
    {
        public int Power;
    }
}
