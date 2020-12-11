// ================= License ====================
//
// BulletTag.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using Unity.Entities;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// 弾と識別するタグ
    /// </summary>
    [GenerateAuthoringComponent]
    public struct BulletTag : IComponentData
    {
        /// <summary>
        /// 弾速
        /// </summary>
        public float BulletSpeed;
    }
}