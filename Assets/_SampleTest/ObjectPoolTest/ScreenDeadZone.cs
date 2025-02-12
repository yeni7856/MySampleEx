using System;
using UnityEngine;

namespace MySampleEx
{
    [Serializable]
    public struct ScreenDeadZone        //값 타입
    {
        public Rect percentage;
        public Rect CalculateActualDeadZone()
        {
            return new Rect(
                percentage.x * Screen.width,
                percentage.y * Screen.height,
                percentage.width * Screen.width,
                percentage.height * Screen.height
            );
        }
        public bool IsMouseInDeadZone()
        {
            Vector2 mousePosition = Input.mousePosition;
            Rect actualDeadZone = CalculateActualDeadZone();
            mousePosition.y = Screen.height - mousePosition.y;

            return actualDeadZone.Contains(mousePosition);
        }
    }
}
