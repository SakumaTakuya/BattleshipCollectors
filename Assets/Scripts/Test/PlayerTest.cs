using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

using UnityEngine;

namespace Sakkun.DOTS.Test
{
    public class PlayerTest : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh = default;
        [SerializeField] private Material _material = default;
        private EntityManager _entityManager;

        private void Start()
        {
            _entityManager = World.Active.EntityManager;
            var entity = _entityManager.CreateEntity(
                typeof(Player),
                typeof(MouseControled),
                typeof(StraightMover),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Rotation));

            _entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });

            _entityManager.SetComponentData(entity, new MouseControled{ Sensitivity = 1f });
            _entityManager.SetComponentData(entity, new StraightMover { Speed = 1f });

            var prefab = _entityManager.CreateEntity(
                typeof(MouseControled),
                typeof(StraightMover),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Rotation));

            _entityManager.SetSharedComponentData(prefab, new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
            _entityManager.SetComponentData(prefab, new MouseControled{ Sensitivity = 1f });
            _entityManager.SetComponentData(prefab, new StraightMover { Speed = 1f });

            const int Side = 1000;
            var rand = new Unity.Mathematics.Random(1);
            using(var entities = new NativeArray<Entity>(Side, Allocator.Temp))
            {
                _entityManager.Instantiate(prefab, entities);
                    for (int index = 0; index < Side; index++)
                    {
                        _entityManager.SetComponentData(entities[index], new Translation
                        {
                            Value = rand.NextFloat3(-10f, 10f)
                        });                  

                        _entityManager.SetComponentData(entities[index], new MouseControled{ Sensitivity = rand.NextFloat(-2f, 2f) });
                        _entityManager.SetComponentData(entities[index], new StraightMover { Speed = rand.NextFloat(-2f, 2f) });
                    }
            }
            
        }
    }
}