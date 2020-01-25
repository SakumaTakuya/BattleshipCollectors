using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


namespace Sakkun.DOTS
{
    public struct TpsCamera : IComponentData
    {
        public float Speed;
        public float3 Offset;
    }

}