// ================= License ====================
//
// BulletFireSystem.cs
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
    /// <summary>
    /// 弾を発射するシステム
    /// </summary>
    [AlwaysUpdateSystem]
    public class BulletFireSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

        private uint _interval;

        protected override void OnCreate()
        {
            // EntityCommandBufferの取得
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            // コマンドバッファを取得
            var commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

            // 60フレームのうちn回弾を発射する
            if (_interval > GameManager.Instance.ShotRate)
            {
                Entities
                    .WithAll<FirePointTag>()
                    .WithoutBurst()
                    .ForEach((in FirePointTag pointTag, in LocalToWorld localToWorld) =>
                    {
                        if (pointTag.ShotMode == FirePointTag.FireType.Normal) // 通常弾
                        {
                            // PrefabとなるEntityから弾を複製する
                            var instantiateEntity = commandBuffer.Instantiate(pointTag.PrefabEntity);

                            // 位置の初期化
                            commandBuffer.SetComponent(instantiateEntity, new Translation
                            {
                                Value = localToWorld.Position
                            });

                            // 回転の初期化
                            commandBuffer.SetComponent(instantiateEntity, new Rotation
                            {
                                Value = localToWorld.Rotation
                            });
                        }
                        else if (pointTag.ShotMode == FirePointTag.FireType.Random) // ばらまき弾
                        {
                            // PrefabとなるEntityから弾を複製する
                            var instantiateEntity = commandBuffer.Instantiate(pointTag.PrefabEntity);

                            var pi = math.PI;
                            var lp = localToWorld.Position;

                            var maxAngle = pi / 3.5f; // 最大角度
                            var radius = UnityEngine.Random.Range(0, 1.5f); // 半径
                            var angle = UnityEngine.Random.Range(-maxAngle - pi / 2.0f, maxAngle - pi / 2.0f); // 角度

                            var position = new float3(lp.x + radius * math.cos(angle), lp.y + radius * math.sin(angle), lp.z); // ランダム座標
                            var diff = math.normalizesafe(position - localToWorld.Position); // 発射源から見たランダム座標の向き

                            // 位置の初期化
                            commandBuffer.SetComponent(instantiateEntity, new Translation
                            {
                                Value = position
                            });

                            // 回転の初期化
                            commandBuffer.SetComponent(instantiateEntity, new Rotation
                            {
                                Value = quaternion.LookRotationSafe(diff, math.forward())
                            });
                        }
                        else if (pointTag.ShotMode == FirePointTag.FireType.Aim) // 自機狙い弾
                        {
                            // プレイヤーのローカル座標を取得
                            var playerEntityQuery = EntityManager.CreateEntityQuery(typeof(PlayerTag));
                            var playerEntity = playerEntityQuery.GetSingletonEntity();
                            var playerLocalToWorld = GetComponent<LocalToWorld>(playerEntity);

                            // 発射台から見たプレイヤーの向き
                            var direction = math.normalizesafe(playerLocalToWorld.Position - localToWorld.Position);

                            // PrefabとなるEntityから弾を複製する
                            var instantiateEntity = commandBuffer.Instantiate(pointTag.PrefabEntity);

                            // 位置の初期化
                            commandBuffer.SetComponent(instantiateEntity, new Translation
                            {
                                Value = localToWorld.Position
                            });

                            // 回転の初期化
                            commandBuffer.SetComponent(instantiateEntity, new Rotation
                            {
                                Value = quaternion.LookRotationSafe(direction, math.forward())
                            });

                            // 向き情報の初期化
                            commandBuffer.SetComponent(instantiateEntity, new AimBulletTag
                            {
                                MoveDirection = direction
                            });
                        }

                    }).Run(); // メインスレッド

                // JobをCommandBufferで流し込む
                _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

                _interval = 0;
            }

            _interval++;
        }
    }
}