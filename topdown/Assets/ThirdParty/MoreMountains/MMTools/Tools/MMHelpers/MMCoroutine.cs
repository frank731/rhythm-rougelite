using System.Collections;

namespace MoreMountains.Tools
{
    public static class MMCoroutine
    {
        public static IEnumerator WaitForFrames(int frameCount)
        {
            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }
    }
}

