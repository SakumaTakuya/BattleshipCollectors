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
    public class CrewTest : MonoBehaviour
    {
        [SerializeField] private Mesh _playerMesh = default;
        [SerializeField] private Mesh _crewMesh = default;
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
                mesh = _playerMesh,
                material = _material
            });

            _entityManager.SetComponentData(entity, new MouseControled{ Sensitivity = 1f });
            _entityManager.SetComponentData(entity, new StraightMover { Speed = 1f });

            var prefab = _entityManager.CreateEntity(
                typeof(Crew),
                typeof(Gunner),
                typeof(Hittable),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Rotation),
                // typeof(LocalPosition),
                // typeof(LocalRotation),
                // typeof(Target),
                typeof(Parent),
                typeof(LocalToParent));

            _entityManager.SetSharedComponentData(prefab, new RenderMesh
            {
                mesh = _crewMesh,
                material = _material
            });

            // _entityManager.SetComponentData(prefab, new Target
            // {
            //     Value = entity
            // });

            _entityManager.SetComponentData(prefab, new Parent
            {
                Value = entity
            });

            const int Side = 10;
            var rand = new Unity.Mathematics.Random(1);
            using(var entities = new NativeArray<Entity>(Side, Allocator.Temp))
            {
                _entityManager.Instantiate(prefab, entities);
                for (int index = 0; index < Side; index++)
                {
                    _entityManager.SetComponentData(entities[index], new Translation
                    {
                        Value = rand.NextFloat3(-5f, 5f)
                    });                  
                }
            }
            
        }
    }
}