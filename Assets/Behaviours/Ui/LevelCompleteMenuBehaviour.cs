﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Behaviours.Ui
{
    class LevelCompleteMenuBehaviour : MonoBehaviour
    {
        [SerializeField]
        private LevelControllerBehaviour _levelController;
        [SerializeField]
        private Button _restartLevelButton;
        [SerializeField]
        private Button _nextLevelButton;

        private void Start()
        {
            _restartLevelButton.onClick.AddListener(() => _levelController.RestartLevel());
            _nextLevelButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
}