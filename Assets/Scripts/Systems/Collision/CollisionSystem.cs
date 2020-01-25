using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using static Unity.Mathematics.math;

namespace Sakkun.DOTS
{
    [UpdateInGroup(typeof(CollisionSystemGroup))]
    public class CollisionSystem : JobComponentSystem
    {
        public NativeArray<Entity> CollidableArray { get; private set; }
        public NativeMultiHashMap<int, int> CollisionMap{ get; private set; }

        private EntityQuery _query;

        // [BurstCompile]
        // private struct CollisionJob : IJobParallelFor
        // {
        //     [DeallocateOnJobCompletion, ReadOnly] public NativeArray<Translation> Translations;
        //     [DeallocateOnJobCompletion, ReadOnly] public NativeArray<Collider> Colliders;
        //     [WriteOnly] public NativeMultiHashMap<int, int>.ParallelWriter CollisionMap;

        //     public void Execute(int index)
        //     {
        //         var mine = Translations[index].Value;
        //         var myRad = Colliders[index].Radius;
        //         var len = Translations.Length;
        //         for(int i = 0; i < len; i++)
        //         {
        //             var target = Translations[i].Value;
        //             var tarRad = Colliders[i].Radius;

        //             if ( distance(mine, target) < myRad + tarRad)
        //             {
        //                 CollisionMap.Add(index, i);
        //             }
        //         }
        //     }
        // }


        [BurstCompile]
        private struct CollisionJob : IJobForEachWithEntity<Translation, Collider>
        {
            [DeallocateOnJobCompletion, ReadOnly] public NativeArray<Translation> Translations;
            [DeallocateOnJobCompletion, ReadOnly] public NativeArray<Collider> Colliders;
            [WriteOnly] public NativeMultiHashMap<int, int>.ParallelWriter CollisionMap;
            public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref Collider collider)
            {
                var mine = translation.Value;
                var myRad = collider.Radius;
                var len = Translations.Length;
                for(int i = 0; i < len; i++)
                {
                    var target = Translations[i].Value;
                    var tarRad = Colliders[i].Radius;

                    if ( distance(mine, target) < myRad + tarRad)
                    {
                        CollisionMap.Add(index, i);
                    }
                }
            }
        }
        protected override void OnCreate()
        {
            _query = GetEntityQuery(typeof(Collider), typeof(Translation));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            InitializeArrays();

            var translations = _query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var colliders = _query.ToComponentDataArray<Collider>(Allocator.TempJob);

            CollidableArray = _query.ToEntityArray(Allocator.TempJob);
            CollisionMap = new NativeMultiHashMap<int, int>(translations.Length, Allocator.TempJob);

            return new CollisionJob
            {
                Translations = translations,
                Colliders = colliders,
                CollisionMap = CollisionMap.AsParallelWriter()
            }.Schedule(this, inputDeps);
        }

        protected override void OnDestroy()
        {
            InitializeArrays();
        }
 
        private void InitializeArrays()
        {
            if (CollidableArray.IsCreated)
            {
                CollidableArray.Dispose();
            }

            if (CollisionMap.IsCreated)
            {
                CollisionMap.Dispose();
            }
        }
    }

#if DEBUG
    [UpdateInGroup(typeof(CollisionSystemGroup))]
    [UpdateAfter(typeof(CollisionSystem))]
    public class TestCollision : JobComponentSystem
    {
        private struct TestJob : IJobForEach<Translation>
        {
            [ReadOnly] public NativeArray<Entity> ArrayLen;
            [ReadOnly] public NativeMultiHashMap<int, int> MapLen;

            public void Execute(ref Translation c0)
            {
                UnityEngine.Debug.Log("map: " + MapLen.Length + " array: " + ArrayLen.Length);
            }
        }

        private CollisionSystem _collisionSystem;

        protected override void OnCreate()
        {
            _collisionSystem = World.Active.GetOrCreateSystem(typeof(CollisionSystem)) as CollisionSystem;
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new TestJob
            {
                ArrayLen = _collisionSystem.CollidableArray, 
                MapLen = _collisionSystem.CollisionMap
            }.Schedule(this, inputDeps);
        }
    }
#endif
}