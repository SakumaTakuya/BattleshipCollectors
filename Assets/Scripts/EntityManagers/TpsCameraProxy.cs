using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

using UnityEngine;

namespace Sakkun.DOTS
{
    public class TpsCameraProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float _speed = 10f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new TpsCamera
            {
                Speed = _speed,
                Offset = transform.position
            });
        }
    }
}