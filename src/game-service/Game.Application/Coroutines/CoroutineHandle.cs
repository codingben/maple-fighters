/*
 
MIT License

Copyright (c) 2017 Chevy Ray Johnston

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is

furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System.Collections;

namespace Coroutines
{
    /// <summary>
    /// A handle to a (potentially running) coroutine.
    /// </summary>
    public struct CoroutineHandle
    {
        /// <summary>
        /// Reference to the routine's runner.
        /// </summary>
        public CoroutineRunner Runner;

        /// <summary>
        /// Reference to the routine's enumerator.
        /// </summary>
        public IEnumerator Enumerator;

        /// <summary>
        /// Construct a coroutine. Never call this manually, only use return values from Coroutines.Run().
        /// </summary>
        /// <param name="runner">The routine's runner.</param>
        /// <param name="enumerator">The routine's enumerator.</param>
        public CoroutineHandle(CoroutineRunner runner, IEnumerator enumerator)
        {
            Runner = runner;
            Enumerator = enumerator;
        }

        /// <summary>
        /// Stop this coroutine if it is running.
        /// </summary>
        /// <returns>True if the coroutine was stopped.</returns>
        public bool Stop()
        {
            return IsRunning && Runner.Stop(Enumerator);
        }

        /// <summary>
        /// A routine to wait until this coroutine has finished running.
        /// </summary>
        /// <returns>The wait enumerator.</returns>
        public IEnumerator Wait()
        {
            if (Enumerator != null)
                while (Runner.IsRunning(Enumerator))
                    yield return null;
        }

        /// <summary>
        /// True if the enumerator is currently running.
        /// </summary>
        public bool IsRunning
        {
            get { return Enumerator != null && Runner.IsRunning(Enumerator); }
        }
    }
}