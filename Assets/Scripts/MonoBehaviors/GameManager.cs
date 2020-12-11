// ================= License ====================
//
// GameManager.cs
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
    /// ゲームの設定等を行うクラス
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public uint ShotRate => _shotRate;

        [SerializeField] private Canvas _pauseCanvas;
        [SerializeField] private uint _shotRate;

        private bool _isEnable = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

            _pauseCanvas.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var world = World.DefaultGameObjectInjectionWorld;

                var system1 = world.GetExistingSystem<BulletFireSystem>();  // 弾を発射するシステム
                var system2 = world.GetExistingSystem<BulletMovementSystem>();  // 弾を動かすシステム
                var system3 = world.GetExistingSystem<Player_MovementSystem>();  // プレイヤーを動かすシステム

                // boolの更新
                _isEnable = !_isEnable;

                // システムの有効化 / 無効化
                system1.Enabled = _isEnable;
                system2.Enabled = _isEnable;
                system3.Enabled = _isEnable;

                // タイムスケールの変更
                if (_isEnable)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 0;
                }

                // ポーズ画面の有効化 / 無効化
                _pauseCanvas.gameObject.SetActive(!_isEnable);
            }
        }
    }
}