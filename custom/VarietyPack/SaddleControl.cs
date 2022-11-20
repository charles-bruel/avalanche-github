using System;
using System.Collections.Generic;
using UnityEngine;

namespace VarietyPack
{
    public class SaddleControl : MonoBehaviour
    {
        private List<Transform> tracked = new List<Transform>();
        private GameObject target;

        public Transform Parent;
        public Transform Pivot;
        public Transform ZeroPos;
        public Transform FirstBone;

        public float TargetAngle = 0;
        public float FadeIn = 0;
        public float BaseAngle = 0;
        public int Depth = 9;
        public int IgnoreFade = 0;

        private bool Initialized = false;

        private void Initialize()
        {
            if (Initialized) return;
            Initialized = true;
            target = gameObject;
            GetChildTransforms(target.transform);
        }

        private void GetChildTransforms(Transform t)
        {
            tracked.Add(t);
            int size = t.childCount;
            for (int i = 0; i < size; i++)
            {
                GetChildTransforms(t.GetChild(i));
            }
        }

        void Update()
        {
            // Apply();
        }

        public void Apply()
        {
            Initialize();
            if (FadeIn == 0 && BaseAngle == 0)
            {
                Pivot.localRotation = Quaternion.Euler(TargetAngle / 2, 0, 0);
                target.transform.localRotation = Quaternion.Euler(-TargetAngle / Depth, 0, 0);
                for (int i = 1; i < tracked.Count; i++)
                {
                    tracked[i].localRotation = target.transform.localRotation;
                }

                Pivot.position += Parent.position - ZeroPos.position;
            }
            else
            {
                float effective_target = TargetAngle - BaseAngle;
                float total = (Depth - IgnoreFade) * (Depth - 1 - IgnoreFade) * 0.5f * FadeIn;//Arithmetic series sum
                for (int i = 0; i < tracked.Count; i++)
                {
                    float x_euler = -BaseAngle / Depth;
                    if (i >= IgnoreFade)
                    {
                        x_euler += -(i - IgnoreFade) * FadeIn * effective_target / total;
                    }
                    Quaternion quat = Quaternion.Euler(x_euler, 0, 0);
                    tracked[i].localRotation = quat;
                }
            }
        }
    }
}