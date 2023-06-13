using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // Tham chiếu đến transform của người chơi

        private void LateUpdate()
        {
            if (target != null)
            {
                // Cập nhật vị trí của camera theo vị trí của người chơi
                transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
            }
        }

    }
}
