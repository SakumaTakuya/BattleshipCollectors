using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;


namespace Sakkun.DOTS
{
    // [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(FixedUpdate))]
    public class MoveSystemGroup : ComponentSystemGroup { }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class CollisionSystemGroup : ComponentSystemGroup { }
}