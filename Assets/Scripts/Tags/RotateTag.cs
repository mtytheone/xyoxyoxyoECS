// ================= License ====================
//
// RotateTag.cs
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
    /// 回転するオブジェクトと識別するタグ
    /// </summary>
    [GenerateAuthoringComponent]
    public struct RotateTag : IComponentData
    {
        /// <summary>
        /// 回転速度
        /// </summary>
        public float RotationSpeed;
    }
}