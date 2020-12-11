// ================= License ====================
//
// Player_MovementSystem.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// プレイヤーを動かすシステム
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(Player_InputSystem))]
    public class Player_MovementSystem : SystemBase
    {
        private Camera _mainCamera;
        private Vector2 _lowerLeft;
        private Vector2 _upperRight;

        protected override void OnStartRunning()
        {
            // スクリーン座標 → ワールド座標
            _mainCamera = Camera.main;
            _lowerLeft = _mainCamera is null ? Vector3.zero : _mainCamera.ViewportToWorldPoint(Vector2.zero);
            _upperRight = _mainCamera is null ? Vector3.zero : _mainCamera.ViewportToWorldPoint(Vector2.one);
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities
                .WithoutBurst()
                .WithAll<PlayerTag>()
                .ForEach((ref Translation translation, in PlayerData playerData) =>
                {
                    // 向きを取得してその方向に動かす
                    var moveDirection = math.normalizesafe(playerData.MoveDirection);
                    translation.Value += moveDirection * playerData.MoveSpeed * playerData.MoveSpeedMultiplier * deltaTime;

                    // 範囲制限
                    translation.Value.x = math.clamp(translation.Value.x, _lowerLeft.x, _upperRight.x);
                    translation.Value.y = math.clamp(translation.Value.y, _lowerLeft.y, _upperRight.y);

                }).Run(); // メインスレッド
        }
    }
}