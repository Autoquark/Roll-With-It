﻿using Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class CameraControlBehaviour : MonoBehaviour
    {
        private float _minY;
        private float _maxY;

        private float _mousePivotSensitivity = 500;
        private float _mousePanSensitivity = 150;
        private float _mouseRotateSensitivity = 200;

        private float _keyboardPivotSensitivity = 50;
        private float _keyboardPanSensitivity = 20;
        private float _keyboardRotateSensitivity = 150;

        private float _maxDistance = 20;
        private float _minDistance = 5;

        private float _maxXAngle = 60;
        private float _minXAngle = 0;

        private bool _invertRotation = true;

        // The point with y = 0 on the axis which the camera should pivot around
        private Vector3 _pivotPointXZ = Vector3.zero;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            var bounds = FindObjectsOfType<CameraBoundBehaviour>();
            _minY = bounds.Min(x => x.transform.position.y);
            _maxY = bounds.Max(x => x.transform.position.y);

            //transform.LookAt(new Vector3(0, (_minY + _maxY) / 2));
            var geometry = GameObject.Find("LevelGeometry").transform.Children().ToList();
            _pivotPointXZ = new Vector3((geometry.Min(t => t.position.x) + geometry.Max(t => t.position.x)) / 2, 0, (geometry.Min(t => t.position.z) + geometry.Max(t => t.position.z)) / 2);
            transform.Rotate(0, Vector3.Angle(transform.forward.WithY(0), _pivotPointXZ - transform.position.WithY(0)), 0, Space.World);
        }

        private void Update()
        {
            Debug.DrawLine(_pivotPointXZ, _pivotPointXZ.WithY(10), Color.green);

            // Left/right pivot around pivot point
            var xDelta = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * _mousePivotSensitivity : 0;
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                xDelta -= _keyboardPivotSensitivity;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                xDelta += _keyboardPivotSensitivity;
            }
            transform.RotateAround(_pivotPointXZ, Vector3.up, xDelta * Time.deltaTime);

            // Up/down pan
            var yDelta = Input.GetMouseButton(0) ? Input.GetAxis("Mouse Y") * _mousePanSensitivity : 0;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                yDelta += _keyboardPanSensitivity;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                yDelta -= _keyboardPanSensitivity;
            }
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + yDelta * Time.deltaTime, _minY, _maxY), transform.position.z);

            // 'zoom' - adjusts camera distance from pivot
            var zDelta = -Input.mouseScrollDelta.y;
            var currentDistance = Vector3.Distance(transform.position.WithY(0), _pivotPointXZ);
            var distance = Mathf.Clamp(currentDistance + zDelta, _minDistance, _maxDistance);
            var newPositionXZ = _pivotPointXZ + (transform.position.WithY(0) - _pivotPointXZ).normalized * distance;
            transform.position = new Vector3(newPositionXZ.x, transform.position.y, newPositionXZ.z);

            // Camera up/down rotation
            yDelta = Input.GetMouseButton(1) ? Input.GetAxis("Mouse Y") * _mouseRotateSensitivity : 0;
            yDelta = _invertRotation ? -yDelta : yDelta;
            transform.rotation = Quaternion.Euler(
                Mathf.Clamp(transform.rotation.eulerAngles.x + yDelta * Time.deltaTime, _minXAngle, _maxXAngle),
                transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);
        }
    }
}
