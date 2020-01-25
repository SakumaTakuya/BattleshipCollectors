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
    [UpdateAfter(typeof(GoStraightSystem))]
    public class TpsCameraSystem : ComponentSystem
    {
        private EntityQuery _cameraQuery;
        private EntityQuery _playerQuery;

        protected override void OnCreate()
        {
            _cameraQuery = GetEntityQuery(
                typeof(Translation),
                typeof(Rotation),
                typeof(TpsCamera));
                // typeof(CameraTransform),
                // typeof(LocalPosition),
                // typeof(CameraSpeed));
            _playerQuery = GetEntityQuery(
                typeof(Player), 
                typeof(Translation),
                typeof(Rotation));
        }

        protected override void OnUpdate()
        {
            // var 
            Entities.With(_cameraQuery)
                    .ForEach<Translation, Rotation, TpsCamera>(FollowPlayer);
            
        }


        private void FollowPlayer(ref Translation trans, ref Rotation rotation, [ReadOnly] ref TpsCamera tpsCamera)
        {
                Debug.Log(trans.Value + "+" + rotation.Value);

            using(var translations = _playerQuery.ToComponentDataArray<Translation>(Allocator.TempJob))
            using(var rotations = _playerQuery.ToComponentDataArray<Rotation>(Allocator.TempJob))
            {
                if (translations.Length == 0 || rotations.Length == 0) return;

                var offset = tpsCamera.Offset;
                var speed = tpsCamera.Speed;
                var pos = translations[0].Value;
                var rot = rotations[0].Value;

                trans.Value = lerp(trans.Value, pos + mul(rot, tpsCamera.Offset), Time.deltaTime*speed);
                rotation.Value = slerp(rotation.Value, rot, Time.deltaTime*speed);

            }
        }
    }
}
