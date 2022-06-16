using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smartplank.Scripts
{
    public class CyclicMove : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float speed;
        [SerializeField] private Vector3 direction;

        private void Update()
        {
            if (spriteRenderer.transform.position.x + spriteRenderer.bounds.size.x < 0)
            {
                spriteRenderer.transform.position -= direction * spriteRenderer.bounds.size.x;
            }
            spriteRenderer.transform.position += direction * speed * Time.deltaTime;
        }
    }
}
