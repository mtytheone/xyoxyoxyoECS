// ================= License ====================
//
// Player_InputSystem.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using System;
using UnityEngine;
using Unity.Entities;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// プレイヤーの入力を反映するシステム
    /// </summary>
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class Player_InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref PlayerData playerData, in InputData inputData) =>
                {
                    // 入力のboolを取得
                    bool isRightKeyPressed = Input.GetKey(inputData.RightKey);
                    bool isLeftKeyPressed = Input.GetKey(inputData.LeftKey);
                    bool isUpKeyPressed = Input.GetKey(inputData.UpKey);
                    bool isDownKeyPressed = Input.GetKey(inputData.DownKey);

                    // boolから移動方向を反映
                    playerData.MoveDirection.x = Convert.ToInt32(isRightKeyPressed);
                    playerData.MoveDirection.x -= Convert.ToInt32(isLeftKeyPressed);
                    playerData.MoveDirection.y = Convert.ToInt32(isUpKeyPressed);
                    playerData.MoveDirection.y -= Convert.ToInt32(isDownKeyPressed);

                    playerData.MoveSpeedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 0.4f : 1.0f;

                }).Run(); // メインスレッド処理
        }
    }
}