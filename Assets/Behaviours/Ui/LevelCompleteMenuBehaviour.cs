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
        [SerializeField]
        private Button _mainMenuButton;

        private void Start()
        {
            _restartLevelButton.onClick.AddListener(() => _levelController.RestartLevel());
            _nextLevelButton.onClick.AddListener(NextLevel);
            _mainMenuButton.onClick.AddListener(MainMenu);
        }

        private static void MainMenu()
        {
            SceneManager.LoadScene("Level_Machine");
        }

        private static void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex == LevelData._lastLevelIndex ? 1 : SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void OnEnable()
        {
            //_nextLevelButton.gameObject.SetActive(SceneManager.GetActiveScene().buildIndex != LevelData._lastLevelIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextLevel();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                MainMenu();
            }
        }
    }
}
