using UnityEngine;

namespace Project.Utilities
{
    public static class VectorExtensions
    {
        public static readonly int[] SnapDegrees = new int[] { 0, 90, 180, 270 };
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Vector3 Round(Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Vector3 SnapRotation(Vector3 euler)
        {
            Vector3 clampedEuler = euler;
            clampedEuler[0] = SnapToClosest(euler[0], snapStep: 90f);
            clampedEuler[1] = SnapToClosest(euler[1], snapStep: 90f);
            clampedEuler[2] = SnapToClosest(euler[2], snapStep: 90f);

            return clampedEuler;
        }

        private static float SnapToClosest(float angle, float snapStep = 90f)
        {
            // Normalize the angle to be within the range [0, 360)
            angle = Mathf.Repeat(360f - angle, 360f);

            // Snap to the nearest multiple of 90 degrees
            float snappedAngle = Mathf.Round(angle / snapStep) * snapStep;

            return 360f - Mathf.Repeat(snappedAngle, 360f);
        }
    }
}