using System;
using System.Collections.Generic;

namespace KittyEngine.Core.Physics.Collisions.BVH
{
    public class MergeSort
    {
        public void Sort<T>(List<T> input, Func<T, double> valueSelector)
        {
            Sort(input, valueSelector, 0, input.Count - 1);
        }

        private void Merge<T>(List<T> input, Func<T, double> valueSelector, int left, int middle, int right)
        {
            var leftArray = input.GetRange(left, middle - left + 1);
            var rightArray = input.GetRange(middle + 1, right - middle);

            int i = 0;
            int j = 0;
            for (int k = left; k < right + 1; k++)
            {
                if (i == leftArray.Count)
                {
                    input[k] = rightArray[j];
                    j++;
                }
                else if (j == rightArray.Count)
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else if (valueSelector(leftArray[i]) <= valueSelector(rightArray[j]))
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else
                {
                    input[k] = rightArray[j];
                    j++;
                }
            }
        }

        private void Sort<T>(List<T> input, Func<T, double> valueSelector, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                Sort(input, valueSelector, left, middle);
                Sort(input, valueSelector, middle + 1, right);

                Merge(input, valueSelector, left, middle, right);
            }
        }
    }
}
