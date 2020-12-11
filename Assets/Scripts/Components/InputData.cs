// ================= License ====================
//
// InputData.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using UnityEngine;
using Unity.Entities;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// 上下左右キーを決めるコンポーネント
    /// </summary>
    [GenerateAuthoringComponent]
    public struct InputData : IComponentData
    {
        public KeyCode UpKey;
        public KeyCode DownKey;
        public KeyCode RightKey;
        public KeyCode LeftKey;
    }
}