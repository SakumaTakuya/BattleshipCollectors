using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace Sakkun.DOTS
{
    public struct Hittable : IComponentData
    {
        public int Hp;
    }

}
