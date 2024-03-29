﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours.Moving
{
    enum TranslationAxis
    {
        X,
        Y,
        Z
    }

    class ReciprocatingBehaviour : MonoBehaviour
    {
        [SerializeField]
        TranslationAxis Axis = TranslationAxis.X;
        [SerializeField]
        Quaternion Rotation;
        [SerializeField]
        private float SecondsForCompleteCycle = 3.0f;
        [SerializeField]
        private float Dwell = 0.1f;
        [SerializeField]
        private float DistanceTravelled = 3.0f;
        enum Phase
        {
            Rising,
            Up,
            Lowering,
            Down
        }
        [SerializeField]
        Phase StartingPosition = Phase.Rising;
        [SerializeField]
        private bool SceneryMode = false;

        private float Offset = 0.0f;
        private Vector3 BasePosition;
        private Quaternion BaseRotation;
        private float DwellTimer = 0.0f;

        private void Awake()
        {
            if (!SceneryMode)
            {
                BasePosition = transform.position;
                BaseRotation = transform.rotation;
            } else
            {
                BasePosition = transform.localPosition;
                BaseRotation = transform.localRotation;
            }

            switch (StartingPosition)
            {
                case Phase.Rising:
                    break;

                case Phase.Up:
                    Offset = 1;
                    break;

                case Phase.Lowering:
                    Offset = 1;
                    break;

                case Phase.Down:
                    break;
            }

            Rotation.Normalize();
        }

        private void FixedUpdate()
        {
            switch (StartingPosition)
            {
                case Phase.Rising:
                    Offset += 2 / (SecondsForCompleteCycle - 2 * Dwell) * Time.fixedDeltaTime;

                    if (Offset > 1)
                    {
                        StartingPosition = Phase.Up;
                        DwellTimer = 0;
                    }

                    break;

                case Phase.Up:
                    DwellTimer += Time.fixedDeltaTime;

                    if (DwellTimer > Dwell)
                        StartingPosition = Phase.Lowering;

                    break;

                case Phase.Lowering:
                    Offset -= 2 / (SecondsForCompleteCycle - 2 * Dwell) * Time.fixedDeltaTime;

                    if (Offset <= 0)
                    {
                        StartingPosition = Phase.Down;
                        DwellTimer = 0;
                    }

                    break;

                case Phase.Down:
                    DwellTimer += Time.fixedDeltaTime;

                    if (DwellTimer > Dwell)
                        StartingPosition = Phase.Rising;

                    break;
            }

            Offset = Mathf.Clamp(Offset, 0, 1);

            float movement = Offset * DistanceTravelled;

            Vector3 position = new Vector3(
                Axis == TranslationAxis.X ? movement : 0.0f,
                Axis == TranslationAxis.Y ? movement : 0.0f,
                Axis == TranslationAxis.Z ? movement : 0.0f);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                // we don't support local transforms with RigidBodies
                UnityEngine.Assertions.Assert.IsTrue(!SceneryMode);

                rb.position = BasePosition + position;
                if (Rotation != Quaternion.identity)
                {
                    rb.rotation = Quaternion.Lerp(BaseRotation, Rotation * BaseRotation, Offset);
                }
            }
            else
            {
                if (!SceneryMode)
                {
                    transform.position = BasePosition + position;
                    if (Rotation != Quaternion.identity)
                    {
                        transform.rotation = Quaternion.Lerp(BaseRotation, Rotation * BaseRotation, Offset);
                    }
                } else
                {
                    transform.localPosition = BasePosition + position;
                    if (Rotation != Quaternion.identity)
                    {
                        transform.localRotation = Quaternion.Lerp(BaseRotation, Rotation * BaseRotation, Offset);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            var originalPosition = transform.position;
            var originalRotation = transform.rotation;

            Vector3 position = new Vector3(
                Axis == TranslationAxis.X ? DistanceTravelled : 0.0f,
                Axis == TranslationAxis.Y ? DistanceTravelled : 0.0f,
                Axis == TranslationAxis.Z ? DistanceTravelled : 0.0f);

            if (!SceneryMode)
            {
                transform.position += position;
                transform.rotation = Rotation.normalized * transform.rotation;
            } 
            else
            {
                transform.localPosition += position;
                transform.localRotation = Rotation.normalized * transform.localRotation;
            }

            foreach (var child in GetComponentsInChildren<MeshFilter>())
            {
                Gizmos.DrawWireMesh(child.sharedMesh, child.transform.position, child.transform.rotation, child.transform.lossyScale);
            }

            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}
