// ================= License ====================
//
// BulletDestroySystem.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// 画面外に出たら弾を消すシステム
    /// </summary>
    [UpdateAfter(typeof(BulletMovementSystem))]
    public class BulletDestroySystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

        private Camera _mainCamera;
        private Vector2 _lowerLeft;
        private Vector2 _upperRight;

        private readonly Vector2 _minPoint = new Vector2(-0.1f, -0.1f);
        private readonly Vector2 _maxPoint = new Vector2(1.1f, 1.1f);

        protected override void OnCreate()
        {
            // EntityCommandBufferの取得
            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnStartRunning()
        {
            // スクリーン座標 → ワールド座標
            _mainCamera = Camera.main;
            _lowerLeft = _mainCamera.ViewportToWorldPoint(_minPoint);
            _upperRight = _mainCamera.ViewportToWorldPoint(_maxPoint);
        }

        protected override void OnUpdate()
        {
            // コマンドバッファを取得
            var commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

            Entities
                .WithAll<BulletTag>()
                .WithoutBurst()
                .ForEach((Entity entity, in Translation translation) =>
                {
                    // 画面外に出たら消す
                    if (translation.Value.x > _upperRight.x || translation.Value.x < _lowerLeft.x || translation.Value.y > _upperRight.y || translation.Value.y < _lowerLeft.y)
                    {
                        commandBuffer.DestroyEntity(entity);
                    }

                }).Run(); // メインスレッド

            // JobをCommandBufferで流し込む
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}