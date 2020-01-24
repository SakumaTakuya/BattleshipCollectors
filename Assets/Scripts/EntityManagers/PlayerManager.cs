using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

using UnityEngine;

namespace Sakkun.DOTS
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private float _mouseSensitivity = 1f;
        [SerializeField] private float _moveSpeed = 1f;

        [SerializeField] private Mesh _mesh = default;
        [SerializeField] private Material _material = default;

        private void Awake()
        {
            var manager = World.Active.EntityManager;
            var entity = manager.CreateEntity(
                typeof(Player),
                typeof(MouseControled),
                typeof(StraightMover),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Rotation));

            manager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
            manager.SetComponentData(entity, new MouseControled{ Sensitivity = _mouseSensitivity });
            manager.SetComponentData(entity, new StraightMover { Speed = _moveSpeed });
        }

    }
}
