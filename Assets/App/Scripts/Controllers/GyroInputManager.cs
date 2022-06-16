using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts.UserInput
{
    public class GyroInputManager : MonoBehaviour
    {
        #region Singleton
        public static GyroInputManager Instance => _instance;
        private static GyroInputManager _instance;
        #endregion

        private Gyroscope gyro;
        private float _initAng;

        /// <summary>
        /// from -1 to 1
        /// </summary>
        public float InputGyroNormalized => _inputGyroNormalized;
        private float _inputGyroNormalized;
        private float tolerance = 0.04f;

        private const float maxAngle = 30f;

#if UNITY_EDITOR
        private float editorInput;
#endif

        private void Awake()
        {
            // Make sure device supprts Gyroscope
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                _instance = this;
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            gyro = Input.gyro;
            gyro.enabled = true;    // Must enable the gyroscope

            _initAng = 0;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.RightArrow))
                editorInput += Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow))
                editorInput -= Time.deltaTime;

            _inputGyroNormalized = Mathf.Clamp(editorInput, -1f, 1f);
            return;
#endif
            if (!SystemInfo.supportsGyroscope)
            {
                _inputGyroNormalized = 0.5f;
                return;
            }

            var newGyro = (Mathf.Clamp(GetAngle() / maxAngle, -1f, 1f));
            if(Mathf.Abs(newGyro - _inputGyroNormalized) > tolerance)
                _inputGyroNormalized = newGyro;
        }

        private float GetAngle()
        {
            Quaternion attitude = Attitude();
            float ang = Elevation(attitude) - _initAng;
            return ang;
        }

        private Quaternion Attitude()
        {
            Quaternion rawAttitude = gyro.attitude;             // Get the phones attitude in phone space
            Quaternion attitude = GyroToUnity(rawAttitude);     // Convert phone space to Unity space

            return attitude;
        }

        private static Quaternion GyroToUnity(Quaternion q)
        {
            return Quaternion.Euler(0, 90, 0) * new Quaternion(q.x, q.y, -q.z, -q.w); // 
        }

        private static float Elevation(Quaternion attitude)
        {
            Vector3 dir = attitude * Vector3.forward;           // Convert Quaterinon to Dir
            Vector2 horizontalDir = new Vector2(dir.x, dir.z);
            float horizontalDis = horizontalDir.magnitude;
            float ang = Mathf.Atan2(dir.y, horizontalDis) * Mathf.Rad2Deg;  // Calculate the elevation

            return ang;
        }
    }
}
