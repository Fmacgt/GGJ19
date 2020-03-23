
namespace GGJ2019
{
    public sealed class TouchInputSet
    {
        public int capacity;
        public int activeCount;
        public int nonIgnoredActiveCount;

        public bool[] active;
        public bool[] ignored;

        public TouchInputState[] states;

        /////////////////////////////////////////////////////////////////////////////////////

        public void getActiveTouchIndices(int[] buffer)
        {
            int bufferSize = buffer.Length;
            int copyPtr = 0;
            int touchIdx = 0;
            while (touchIdx < capacity && copyPtr < bufferSize) {
                if (!active[touchIdx] || ignored[touchIdx]) {
                    touchIdx++;
                } else {
                    buffer[copyPtr] = touchIdx;

                    touchIdx++;
                    copyPtr++;
                }
            }

            nonIgnoredActiveCount = copyPtr;
        }

        // NOTE: assuming both buffers have enough capacity for the input count
        public void copyInputStates(int count, int[] indices, TouchInputState[] buffer)
        {
            for (int i = 0; i < count; i++) {
                buffer[i] = states[indices[i]];
            }
        }
    }
}
