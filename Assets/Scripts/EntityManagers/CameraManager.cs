using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

using UnityEngine;

namespace Sakkun.DOTS
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Camera _camera = default;
        private EntityManager _entityManager;

        private void Start()
        {
            _entityManager = World.Active.EntityManager;
            var entity = _entityManager.CreateEntity(
                typeof(CameraTransform),
                typeof(LocalPosition),
                typeof(CameraSpeed));

            _entityManager.SetSharedComponentData(entity, new CameraTransform
            {
                Value = _camera.transform
            });
            _entityManager.SetComponentData(entity, new LocalPosition
            {
                Value = _camera.transform.localPosition
            });
            _entityManager.SetComponentData(entity, new CameraSpeed
            {
                Value = _speed
            });

        }
    }
}