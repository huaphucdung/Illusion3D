using System.Collections;
using UnityEngine;

namespace Project.Utilities{
    public static class CoroutineExtensions{
        public static IEnumerator WaitForAll(MonoBehaviour runner, params IEnumerator[] coroutines){
            if(coroutines == null || runner == null) yield break;
            int total = coroutines.Length;

            for(int i = 0; i < total; i++){
                runner.StartCoroutine(RunCoroutineWrapper(coroutines[i]));
            }

            while(total > 0) yield return null;

            IEnumerator RunCoroutineWrapper(IEnumerator coroutine){
                yield return coroutine;
                --total;
            }
        }
    }
}