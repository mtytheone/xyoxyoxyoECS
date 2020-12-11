// ================= License ====================
//
// AimBulletTag.cs
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
    /// 自機狙い弾と識別するタグ
    /// </summary>
    [GenerateAuthoringComponent]
    public struct AimBulletTag : IComponentData
    {
        /// <summary>
        /// 自機弾の向き
        /// </summary>
        [HideInInspector] public float3 MoveDirection;
    }
}