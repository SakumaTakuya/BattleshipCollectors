using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


namespace Sakkun.DOTS
{
    public struct CameraTransform : ISharedComponentData, IEquatable<CameraTransform>
    {
        public Transform Value;

        public bool Equals(CameraTransform other) => other.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
    }

}