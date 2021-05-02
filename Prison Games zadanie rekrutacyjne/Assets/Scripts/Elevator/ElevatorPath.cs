using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.red;

                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetFloor(i), 0.2f);
                Gizmos.DrawLine(GetFloor(i), GetFloor(j));
            }
        }

        public int GetNextIndex(int i)
        {
            //Floor loop
            if (i + 1 >= transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetFloor(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}