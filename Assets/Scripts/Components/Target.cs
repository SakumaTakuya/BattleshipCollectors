using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Sakkun.DOTS
{
    public struct Target : IComponentData
    {
        public Entity Value;
    }

    [BurstCompile]
    public struct CopyTargetComponent<T> : IJobParallelFor 
        where T : struct, IComponentData 
    {
        [WriteOnly] public NativeArray<T> TargetComponents;
        [ReadOnly] public NativeArray<Target> Targets;
        [ReadOnly] public ComponentDataFromEntity<T> DataFromEntity;

        public void Execute(int index)
        {
            var entity = Targets[index].Value;
            if (DataFromEntity.Exists(entity))
                TargetComponents[index] = DataFromEntity[entity];
        }
    }

    [BurstCompile]
    public struct CopyTargetComponent<T0, T1> : IJobParallelFor 
        where T0 : struct, IComponentData 
        where T1 : struct, IComponentData 
    {
        [WriteOnly] public NativeArray<T0> TargetComponents0;
        [WriteOnly] public NativeArray<T1> TargetComponents1;
        [ReadOnly] public NativeArray<Target> Targets;
        [ReadOnly] public ComponentDataFromEntity<T0> DataFromEntity0;
        [ReadOnly] public ComponentDataFromEntity<T1> DataFromEntity1;

        public void Execute(int index)
        {
            var entity = Targets[index].Value;
            if (DataFromEntity0.Exists(entity))
                TargetComponents0[index] = DataFromEntity0[entity];

            if (DataFromEntity1.Exists(entity))
                TargetComponents1[index] = DataFromEntity1[entity];
        }
    }

    public struct ReleaseArrayJob<T> : IJob
        where T : struct
    {
        [DeallocateOnJobCompletion] private NativeArray<T> targetArray;
        public void Execute() { }

        public ReleaseArrayJob(NativeArray<T> targets) => targetArray = targets;
    }
}