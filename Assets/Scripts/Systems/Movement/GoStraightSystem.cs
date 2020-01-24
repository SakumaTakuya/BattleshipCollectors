using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

using static Unity.Mathematics.math;

namespace Sakkun.DOTS
{
    [UpdateInGroup(typeof(MoveSystemGroup))]
    public class GoStraightSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct MoveJob : IJobForEach<Translation, Rotation, StraightMover>
        {
            public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation, [ReadOnly] ref StraightMover mover)
                => translation.Value += forward(rotation.Value) * mover.Speed * 0.02f;
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
            => new MoveJob().Schedule(this, inputDeps);
    }
}