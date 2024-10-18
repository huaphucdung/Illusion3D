using UnityEngine;

namespace Project.Utilities{
    public static class VectorExtensions{

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Vector3 Round(Vector3 v){
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
    }
}