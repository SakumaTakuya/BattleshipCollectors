using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using static Unity.Mathematics.math;

namespace Sakkun.DOTS
{
#if DEBUG
    [UpdateInGroup(typeof(MoveSystemGroup))]
    [UpdateAfter(typeof(GoStraightSystem))]
    public class CrewMoveSystem : JobComponentSystem
    {
        [BurstCompile]
        [RequireComponentTag(typeof(Crew))]
        private struct MoveJob : IJobForEachWithEntity<Translation, Rotation, LocalPosition, LocalRotation>
        {
            [System.Obsolete, DeallocateOnJobCompletion, ReadOnly] public NativeArray<Translation> Translations;
            [System.Obsolete, DeallocateOnJobCompletion, ReadOnly] public NativeArray<Rotation> Rotations;

            // [DeallocateOnJobCompletion, ReadOnly] public NativeArray

            public void Execute(
                Entity entity, 
                int index, 
                ref Translation translation, 
                ref Rotation rotation, 
                ref LocalPosition localPosition, 
                ref LocalRotation localRotation)
            {
                translation.Value = localPosition.Value + Translations[index].Value; 
                rotation.Value = Rotations[index].Value;// mul(localRotation.Value, );
            }
        }

        private EntityQuery _query;

        protected override void OnCreate()
        {
            _query = GetEntityQuery(typeof(Target));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var targets = _query.ToComponentDataArray<Target>(Allocator.TempJob);
            var translations = new NativeArray<Translation>(targets.Length, Allocator.TempJob);
            var rotations = new NativeArray<Rotation>(targets.Length, Allocator.TempJob);

            inputDeps = new CopyTargetComponent<Translation, Rotation>
            {
                TargetComponents0 = translations,
                TargetComponents1 = rotations,
                Targets = targets,
                DataFromEntity0 = GetComponentDataFromEntity<Translation>(true),
                DataFromEntity1 = GetComponentDataFromEntity<Rotation>(true)
            }.Schedule(targets.Length, 6, inputDeps);

            inputDeps = new ReleaseArrayJob<Target>(targets).Schedule(inputDeps);

            return new MoveJob
            {
                Translations = translations,
                Rotations = rotations
            }.Schedule(this, inputDeps);
        }

        
    }
#endif
}