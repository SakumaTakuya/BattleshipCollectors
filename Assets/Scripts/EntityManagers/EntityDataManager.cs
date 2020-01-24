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
    static public class EntityUtil
    {
        private static EntityManager _entityManager;
        private static EntityManager EntityManager 
        {
            get 
            {
                if (_entityManager == null)
                {
                    _entityManager = World.Active.EntityManager;
                }

                return _entityManager;
            }
        }

        public static void SetData<T>(this Entity entity, T data) where T : struct, IComponentData
        {
            EntityManager.SetComponentData(entity, data);
        }

        public static void SetSharedData<U>(this Entity entity, U data) where U : struct, ISharedComponentData
        {
            EntityManager.SetSharedComponentData(entity, data);
        }
    }
}