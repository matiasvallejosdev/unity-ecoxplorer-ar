using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    public class ARObjectAnimationDisplay : MonoBehaviour
    {
        public Transform transformObject;
        public float rotationSpeed = 10f;
        public float moveUpSpeed = 1f;
        public float moveUpDistance = 0.1f;

        void LoopRotateAndMoveUp()
        {
            // Rotate around the GameObject's pivot
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Calculate the up and down movement relative to the current position
            float newY = Mathf.Sin(Time.time * moveUpSpeed) * moveUpDistance;
            // Apply the oscillation on top of the current Y position
            transform.position = new Vector3(transform.position.x, transform.position.y + newY - Mathf.Sin((Time.time - Time.deltaTime) * moveUpSpeed) * moveUpDistance, transform.position.z);
        }

        void Update()
        {
            LoopRotateAndMoveUp();
        }
    }
}