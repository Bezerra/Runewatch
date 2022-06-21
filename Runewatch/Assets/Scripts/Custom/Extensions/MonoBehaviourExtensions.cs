using System;
using System.Collections;
using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Static class for MonoBehaviour extention methods.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Stops a coroutine. Sets the coroutine variable. Starts the coroutine.
        /// Whenever this method is called, it will stop the coroutine and start it again.
        /// </summary>
        /// <param name="mb">MonoBehaviour who called this method,</param>
        /// <param name="coroutineVariable">IEnumerator variable.</param>
        /// <param name="coroutineMethod">IEnumerator method.</param>
        public static void StartCoroutineWithReset(
            this MonoBehaviour mb, ref IEnumerator coroutineVariable, IEnumerator coroutineMethod)
        {
            if (coroutineVariable != null) mb.StopCoroutine(coroutineVariable);
            coroutineVariable = coroutineMethod;
            mb.StartCoroutine(coroutineVariable);
        }

        /// <summary>
        /// Starts a coroutine to repeat an action very X seconds for a limited time.
        /// </summary>
        /// <param name="mb">MonoBehaviour who called this method,</param>
        /// <param name="maxTime">Max time to run this coroutine.</param>
        /// <param name="waitingTimeInterval">Time interval for each action to be executed.</param>
        /// <param name="method">Method to run.</param>
        public static void RepeatCoroutine(
            this MonoBehaviour mb, float maxTime, YieldInstruction waitingTimeInterval, Action method)
        {
            mb.StartCoroutine(RepeatingCoroutine(maxTime, waitingTimeInterval, method));
        }

        /// <summary>
        /// Runs an action every x seconds.
        /// </summary>
        /// <param name="maxTime">Max time to run this coroutine.</param>
        /// <param name="waitingTimeInterval">Time interval for each action to be executed.</param>
        /// <param name="method">Method to run.</param>
        /// <returns>Waiting time.</returns>
        private static IEnumerator RepeatingCoroutine(
            float maxTime, YieldInstruction waitingTime, Action metod)
        {
            float currentTime = Time.time;
            while (Time.time - currentTime < maxTime)
            {
                metod.Invoke();
                yield return waitingTime;
            }
        }
    }
}
