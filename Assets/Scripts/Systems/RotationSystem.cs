// ================= License ====================
//
// RotationSystem.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace hatuxes.xyoxyoxyo
{
    public class RotationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities
                .WithAll<RotateTag>()
                .ForEach((ref Rotation rotation, in RotateTag rotateTag) =>
                {
                    quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                    quaternion angleToRotate = quaternion.AxisAngle(math.up(), rotateTag.RotationSpeed * deltaTime);

                    rotation.Value = math.mul(normalizedRotation, angleToRotate);

                }).ScheduleParallel(); // 分散並列スレッド処理
        }
    }
}