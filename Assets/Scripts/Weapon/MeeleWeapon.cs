using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MySampleEx
{
    public class MeeleWeapon : MonoBehaviour
    {
        //무기 공격시 상대에게 데미지 입히는 포인트
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;

#if UNITY_EDITOR
            public List<Vector3> previousPosition = new List<Vector3>();
#endif
        }

        #region Variables
        public int damage = 1;             //hit시 데미지 Point

        public AttackPoint[] attackPoints = new AttackPoint[0];
        public TimeEffect[] effects;
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint attackPoint = attackPoints[i];
                if (attackPoint.attackRoot != null)
                {
                    Vector3 worldPos = attackPoint.attackRoot.TransformVector(attackPoint.offset);
                    Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                    Gizmos.DrawSphere(attackPoint.attackRoot.position + worldPos, attackPoint.radius);
                }
                if (attackPoint.previousPosition.Count > 0)
                {
                    UnityEditor.Handles.DrawAAPolyLine(10, attackPoint.previousPosition.ToArray());
                }
            }
        }
#endif
    }
}