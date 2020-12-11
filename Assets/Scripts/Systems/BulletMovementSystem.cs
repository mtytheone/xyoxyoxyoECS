// ================= License ====================
//
// BulletMovementSystem.cs
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this script.
//
// ==============================================

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace hatuxes.xyoxyoxyo
{
    /// <summary>
    /// 弾を動かすシステム
    /// </summary>
    public class BulletMovementSystem : SystemBase
    {
        private Camera _mainCamera;
        private Vector2 _lowerLeft;
        private Vector2 _upperRight;

        private readonly Vector2 _minPoint = Vector2.zero;
        private readonly Vector2 _maxPoint = Vector2.one;

        protected override void OnStartRunning()
        {
            // スクリーン座標 → ワールド座標
            _mainCamera = Camera.main;
            _lowerLeft = _mainCamera.ViewportToWorldPoint(_minPoint);
            _upperRight = _mainCamera.ViewportToWorldPoint(_maxPoint);
        }

        protected override void OnUpdate()
        {
            // グローバル変数のキャッシュ
            var deltaTime = Time.DeltaTime;
            var lowerLeft = _lowerLeft;
            var upperRight = _upperRight;

            // 通常弾の動き
            Entities
                .WithAll<BulletTag>()
                .WithNone<AimBulletTag, ReflectionBulletTag>()
                .ForEach((ref Translation translation, in LocalToWorld localToWorld, in BulletTag bulletTag) =>
                {
                    translation.Value += localToWorld.Forward * bulletTag.BulletSpeed * deltaTime;

                }).ScheduleParallel();

            // 自機狙い弾の動き
            Entities
                .WithAll<BulletTag, AimBulletTag>()
                .ForEach((ref Translation translation, in AimBulletTag aimBulletTag, in BulletTag bulletTag) =>
                {
                    translation.Value += aimBulletTag.MoveDirection * bulletTag.BulletSpeed * deltaTime;

                }).ScheduleParallel();

            // 跳ね返る弾の動き
            Entities
                .WithAll<BulletTag, ReflectionBulletTag>()
                .ForEach((ref Translation translation, ref Rotation rotation, ref ReflectionBulletTag reflectionBulletTag,
                    in LocalToWorld localToWorld, in BulletTag bulletTag) =>
                {
                    translation.Value += localToWorld.Forward * bulletTag.BulletSpeed * deltaTime;

                    // 画面外で残り反射回数が0より大きかったら跳ね返す
                    if (translation.Value.y < lowerLeft.y || translation.Value.y > upperRight.y ||
                        translation.Value.x < lowerLeft.x || translation.Value.x > upperRight.x)
                    {
                        if (reflectionBulletTag.ReflectCount > 0)
                        {
                            // 回転（精度が完璧ではない）
                            quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                            quaternion angleToRotate = quaternion.AxisAngle(math.up(), 180 * deltaTime);
                            rotation.Value = math.mul(normalizedRotation, angleToRotate);

                            // 残り反射回数を減らす
                            reflectionBulletTag.ReflectCount--;
                        }
                    }

                }).ScheduleParallel(); // 分散並列スレッド

        }
    }
}