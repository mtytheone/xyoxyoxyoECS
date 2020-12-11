// ================= License ====================
//
// ReflectionBulletTag.cs
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
    /// 反射弾と識別するタグ
    /// </summary>
    [GenerateAuthoringComponent]
    public struct ReflectionBulletTag : IComponentData
    {
        /// <summary>
        /// 残り反射回数
        /// </summary>
        public uint ReflectCount;
    }
}