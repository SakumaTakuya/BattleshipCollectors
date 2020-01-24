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
                typeof(CameraTransform),
                typeof(LocalPosition),
                typeof(CameraSpeed));
            _playerQuery = GetEntityQuery(
                typeof(Player), 
                typeof(Translation),
                typeof(Rotation));
        }

        protected override void OnUpdate()
        {
            Entities.With(_cameraQuery)
                    .ForEach<CameraTransform, LocalPosition, CameraSpeed>(FollowPlayer);
        }


        private void FollowPlayer(CameraTransform cameraTransform, ref LocalPosition localPosition, ref CameraSpeed cameraSpeed)
        {
            var offset = localPosition.Value;
            var speed = cameraSpeed.Value;
            void Follow(ref Translation translation, ref Rotation rotation)
            {
                var tr = cameraTransform.Value;
                tr.position = lerp(tr.position, translation.Value + mul(rotation.Value, offset), Time.deltaTime*speed);
                // tr.LookAt(translation.Value);
                tr.rotation = slerp(tr.rotation, rotation.Value, Time.deltaTime*speed);
            }

            Entities.With(_playerQuery)
                    .ForEach<Translation, Rotation>(Follow);
        }
    }
}