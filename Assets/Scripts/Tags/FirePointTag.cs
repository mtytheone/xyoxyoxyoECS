// ================= License ====================
//
// FirePointTag.cs
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
    /// 発射台と識別するタグ
    /// </summary>
    [GenerateAuthoringComponent]
    public struct FirePointTag : IComponentData
    {
        public enum FireType
        {
            Normal = 0,
            Random = 1,
            Aim = 2
        };

        /// <summary>
        /// 元になるPrefab
        /// </summary>
        public Entity PrefabEntity;

        /// <summary>
        /// 発射台の発射モード
        /// </summary>
        public FireType ShotMode;
    }
}