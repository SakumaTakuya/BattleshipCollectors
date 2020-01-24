using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


namespace Sakkun.DOTS
{
    [UpdateInGroup(typeof(MoveSystemGroup))]
    public class ControledRotationSystem : ComponentSystem
    {
        private EntityQuery _query;

        protected override void OnCreate()
            => _query = GetEntityQuery(
                            typeof(MouseControled),
                            typeof(Rotation));

        protected override void OnUpdate()
            =>  Entities.With(_query)
                        .ForEach<MouseControled, Rotation>(MoveByInput);

        private void MoveByInput(ref MouseControled controled, ref Rotation rotation)
        {
            controled.Yaw += Input.GetAxis("Mouse X") * controled.Sensitivity;
            controled.Pitch -= Input.GetAxis("Mouse Y") * controled.Sensitivity;

            rotation.Value = quaternion.Euler(controled.Pitch, controled.Yaw, 0f);
        }
    }
}
