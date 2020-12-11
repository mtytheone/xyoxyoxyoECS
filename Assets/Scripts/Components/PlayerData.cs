// ================= License ====================
//
// PlayerData.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// プレイヤーの諸情報を持つコンポーネント
    /// </summary>
    [GenerateAuthoringComponent]
    public struct PlayerData : IComponentData
    {
        [HideInInspector] public float3 MoveDirection; // 向く方向
        [HideInInspector] public float MoveSpeedMultiplier; // 移動速度係数
        public float MoveSpeed; // 移動速度
    }
}