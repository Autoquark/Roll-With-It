﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Behaviours
{
    class LevelControllerBehaviour : MonoBehaviour
    {
        public bool MainMenuMode = false;

        public bool Enabled = true;

        public int BallsInGoal { get; set; } = 0;

        public bool IsSwitched { get; private set; } = false;

        public float MinimumLevelGeometryY { get; private set; }

        public bool LevelStarted => LevelStartTime != -1;

        public float LevelStartTime { get; private set; } = -1;

        [SerializeField]
        private GameObject LevelStartAudio;

        public GameObject LevelCompleteAudio;

        bool _started = false;

        public void RestartLevel()
        {
            var camera = FindObjectOfType<CameraControlBehaviour>();
            camera.StorePosition();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Start()
        {
            MinimumLevelGeometryY = GameObject.Find("LevelGeometry").GetComponentsInChildren<Transform>()
                .Min(x => x.position.y - 10);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsSwitched = !IsSwitched;
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }

            if(Enabled && !_started && (Input.GetKeyDown(KeyCode.F) || MainMenuMode))
            {
                LevelStartTime = Time.time;
                if (!MainMenuMode) Instantiate(LevelStartAudio);
                _started = true;
            }
        }
    }
}
