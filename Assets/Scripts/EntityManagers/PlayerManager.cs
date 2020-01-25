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
        [SerializeField] private int _initialCrew = 5;

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
                typeof(Rotation),
                typeof(Collider));

            manager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
            manager.SetComponentData(entity, new MouseControled{ Sensitivity = _mouseSensitivity });
            manager.SetComponentData(entity, new StraightMover { Speed = _moveSpeed });
        
            var prefab = manager.CreateEntity(
                typeof(Crew),
                typeof(Gunner),
                typeof(Hittable),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Rotation),
                typeof(Parent),
                typeof(LocalToParent),
                typeof(Collider));

            manager.SetSharedComponentData(prefab, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });

            manager.SetComponentData(prefab, new Parent
            {
                Value = entity
            });

            var rand = new Unity.Mathematics.Random(1);
            using(var entities = new NativeArray<Entity>(_initialCrew, Allocator.Temp))
            {
                manager.Instantiate(prefab, entities);
                for (int index = 0; index < _initialCrew; index++)
                {
                    manager.SetComponentData(entities[index], new Translation
                    {
                        Value = rand.NextFloat3(-5f, 5f)
                    });                  
                }
            }
        }

    }
}
