using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


namespace Sakkun.DOTS
{
    public struct CameraSpeed : IComponentData
    {
        public float Value;
    }

}