using System;
using System.Collections.Generic;

namespace Jaylan.Utilities
{
    public class Sorter
    {
        #region 选择排序
        /// <summary>
        /// 选择排序
        /// 找出参与排序的数组最大值，放到末尾（或找到最小值放到开头）
        /// </summary>
        /// <param name="data"></param>
        public static void SelectSort(IList<int> data)
        {
            for (var i = 0; i < data.Count - 1; i++)
            {
                var min = i;
                var temp = data[i];
                for (var j = i + 1; j < data.Count; j++)
                {
                    if (data[j] < temp)
                    {
                        min = j;
                        temp = data[j];
                    }
                }
                if (min != i)
                    Swap(data, min, i);
            }
        } 
        #endregion

        #region 冒泡排序
        /// <summary>
        /// 冒泡排序
        /// 从头开始，每一个元素和它的下一个元素比较，如果它大，就将它与比较的元素交换，否则不动。
        /// 这意味着，大的元素总是在向后慢慢移动直到遇到比它更大的元素。所以每一轮交换完成都能将最大值冒到最后。
        /// </summary>
        /// <param name="data"></param>
        public static void BubbleSort(IList<int> data)
        {
            for (var i = data.Count - 1; i > 0; i--)
            {
                for (var j = 0; j < i; j++)
                {
                    if (data[j] > data[j + 1])
                        Swap(data, j, j + 1);
                }
            }
        }

        /// <summary>
        /// 通过标识提升冒泡排序
        /// </summary>
        /// <param name="data"></param>
        public static void BubbleSortImprovedWithFlag(IList<int> data)
        {
            bool flag;
            for (var i = data.Count - 1; i > 0; i--)
            {
                flag = true;
                for (var j = 0; j < i; j++)
                {
                    if (data[j] > data[j + 1])
                    {
                        Swap(data, j, j + 1);
                        flag = false;
                    }
                }
                if (flag) break;
            }
        }

        /// <summary>
        /// 鸡尾酒排序（来回排序）对冒泡排序进行更大的优化
        /// </summary>
        /// <param name="data"></param>
        public static void BubbleCocktailSort(IList<int> data)
        {
            bool flag;
            int m = 0, n = 0;
            for (var i = data.Count - 1; i > 0; i--)
            {
                flag = true;
                if (i % 2 == 0)
                {
                    for (var j = n; j < data.Count - 1 - m; j++)
                    {
                        if (data[j] > data[j + 1])
                        {
                            Swap(data, j, j + 1);
                            flag = false;
                        }
                    }
                    if (flag) break;
                    m++;
                }
                else
                {
                    for (var k = data.Count - 1 - m; k > n; k--)
                    {
                        if (data[k] < data[k - 1])
                        {
                            Swap(data, k, k - 1);
                            flag = false;
                        }
                    }
                    if (flag) break;
                    n++;
                }
            }
        } 
        #endregion

        #region 插入排序
        /// <summary>
        /// 插入排序
        /// 通过构建有序数列，将未排序的数从后向前比较，找到合适位置并插入。
        /// </summary>
        /// <param name="data"></param>
        public static void InsertSort(IList<int> data)
        {
            int temp;
            for (var i = 1; i < data.Count; i++)
            {
                temp = data[i];
                for (var j = i - 1; j >= 0; j--)
                {
                    if (data[j] > temp)
                    {
                        data[j + 1] = data[j];
                        if (j == 0)
                        {
                            data[0] = temp;
                            break;
                        }
                    }
                    else
                    {
                        data[j + 1] = temp;
                        break;
                    }
                }
            }
        } 
        #endregion

        #region 二分查找法优化插入排序
        /// <summary>
        /// 二分查找法优化插入排序
        /// 通过二分查找法的方式找到一个位置索引。当要排序的数插入这个位置时，大于前一个数，小于后一个数。
        /// </summary>
        /// <param name="data"></param>
        public static void InsertSortImprovedWithBinarySearch(IList<int> data)
        {
            int temp;
            int tempIndex;
            for (var i = 1; i < data.Count; i++)
            {
                temp = data[i];
                tempIndex = BinarySearchForInsertSort(data, 0, i, i);
                for (var j = i - 1; j >= tempIndex; j--)
                {
                    data[j + 1] = data[j];
                }
                data[tempIndex] = temp;
            }
        }

        /// <summary>
        /// 二分查找法优化插入排序
        /// 通过二分查找法的方式找到一个位置索引。当要排序的数插入这个位置时，大于前一个数，小于后一个数。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="low">起始索引</param>
        /// <param name="high">终止索引</param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static int BinarySearchForInsertSort(IList<int> data, int low, int high, int key)
        {
            if (low >= data.Count - 1)
                return data.Count - 1;
            if (high <= 0)
                return 0;
            var mid = (low + high) / 2;
            if (mid == key) return mid;
            if (data[key] > data[mid])
            {
                if (data[key] < data[mid + 1])
                    return mid + 1;
                return BinarySearchForInsertSort(data, mid + 1, high, key);
            }
            else  // data[key] <= data[mid]
            {
                if (mid - 1 < 0) return 0;
                if (data[key] > data[mid - 1])
                    return mid;
                return BinarySearchForInsertSort(data, low, mid - 1, key);
            }
        } 
        #endregion

        #region 快速排序
        /// <summary>
        /// 快速排序
        /// 从数列中挑选一个数作为“哨兵”，使比它小的放在它的左侧，比它大的放在它的右侧。
        /// 将要排序是数列递归地分割到最小数列，每次都让分割出的数列符合“哨兵”的规则，自然就将数列变得有序。
        /// </summary>
        /// <param name="data"></param>
        public static void QuickSortStrict(IList<int> data)
        {
            QuickSortStrict(data, 0, data.Count - 1);
        }

        /// <summary>
        /// 快速排序
        /// 从数列中挑选一个数作为“哨兵”，使比它小的放在它的左侧，比它大的放在它的右侧。
        /// 将要排序是数列递归地分割到最小数列，每次都让分割出的数列符合“哨兵”的规则，自然就将数列变得有序。
        /// </summary>
        /// <param name="low">起始索引</param>
        /// <param name="high">终止索引</param>
        /// <param name="data"></param>
        public static void QuickSortStrict(IList<int> data, int low, int high)
        {
            if (low >= high) return;
            var temp = data[low];
            int i = low + 1, j = high;
            while (true)
            {
                while (data[j] > temp) j--;
                while (data[i] < temp && i < j) i++;
                if (i >= j) break;
                Swap(data, i, j);
                i++; j--;
            }
            if (j != low)
                Swap(data, low, j);
            QuickSortStrict(data, j + 1, high);
            QuickSortStrict(data, low, j - 1);
        } 
        #endregion

        #region 快速排序2
        /// <summary>
        /// 快速排序
        /// 从数列中挑选一个数作为“哨兵”，使比它小的放在它的左侧，比它大的放在它的右侧。
        /// 将要排序是数列递归地分割到最小数列，每次都让分割出的数列符合“哨兵”的规则，自然就将数列变得有序。
        /// </summary>
        /// <param name="data"></param>
        public static void QuickSortRelaxImproved(IList<int> data)
        {
            QuickSortRelaxImproved(data, 0, data.Count - 1);
        }

        /// <summary>
        /// 快速排序
        /// 从数列中挑选一个数作为“哨兵”，使比它小的放在它的左侧，比它大的放在它的右侧。
        /// 将要排序是数列递归地分割到最小数列，每次都让分割出的数列符合“哨兵”的规则，自然就将数列变得有序。
        /// </summary>
        /// <param name="low">起始索引</param>
        /// <param name="high">终止索引</param>
        /// <param name="data"></param>
        public static void QuickSortRelaxImproved(IList<int> data, int low, int high)
        {
            if (low >= high) return;
            var temp = data[(low + high) / 2];
            int i = low - 1, j = high + 1;
            var index = (low + high) / 2;
            while (true)
            {
                while (data[++i] < temp) ;
                while (data[--j] > temp) ;
                if (i >= j) break;
                Swap(data, i, j);
                if (i == index) index = j;
                else if (j == index) index = i;
            }
            if (j == i)
            {
                QuickSortRelaxImproved(data, j + 1, high);
                QuickSortRelaxImproved(data, low, i - 1);
            }
            else //i-j==1
            {
                if (index >= i)
                {
                    if (index != i)
                        Swap(data, index, i);
                    QuickSortRelaxImproved(data, i + 1, high);
                    QuickSortRelaxImproved(data, low, i - 1);
                }
                else //index < i
                {
                    if (index != j)
                        Swap(data, index, j);
                    QuickSortRelaxImproved(data, j + 1, high);
                    QuickSortRelaxImproved(data, low, j - 1);
                }
            }
        } 
        #endregion
        
        #region 归并排序
        /// <summary>
        /// 归并排序
        /// 将两个有序的数列，通过比较，合并为一个有序数列。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<int> MergeSortOnlyList(List<int> data)
        {
            data = MergeSortOnlyList(data, 0, data.Count - 1);  //不会改变外部引用 参照C#参数传递
            return data;
        } 

        private static List<int> MergeSortOnlyList(List<int> data, int low, int high)
        {
            if (low == high)
                return new List<int> { data[low] };
            var mergeData = new List<int>();
            var mid = (low + high) / 2;
            var leftData = MergeSortOnlyList(data, low, mid);
            var rightData = MergeSortOnlyList(data, mid + 1, high);
            int i = 0, j = 0;
            while (true)
            {
                if (leftData[i] < rightData[j])
                {
                    mergeData.Add(leftData[i]);
                    if (++i == leftData.Count)
                    {
                        mergeData.AddRange(rightData.GetRange(j, rightData.Count - j));
                        break;
                    }
                }
                else
                {
                    mergeData.Add(rightData[j]);
                    if (++j == rightData.Count)
                    {
                        mergeData.AddRange(leftData.GetRange(i, leftData.Count - i));
                        break;
                    }
                }
            }
            return mergeData;
        }
        #endregion

        #region 归并排序,IList<int>版本
        /// <summary>
        /// 归并排序
        /// 将两个有序的数列，通过比较，合并为一个有序数列。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int> MergeSort(IList<int> data)
        {
            data = MergeSort(data, 0, data.Count - 1);
            return data;
        }

        private static IList<int> MergeSort(IList<int> data, int low, int high)
        {
            var length = high - low + 1;
            var mergeData = NewInstance(data, length);
            if (low == high)
            {
                mergeData[0] = data[low];
                return mergeData;
            }
            var mid = (low + high) / 2;
            var leftData = MergeSort(data, low, mid);
            var rightData = MergeSort(data, mid + 1, high);
            int i = 0, j = 0;
            while (true)
            {
                if (leftData[i] < rightData[j])
                {
                    mergeData[i + j] = leftData[i++]; //不能使用Add,Array Length不可变
                    if (i == leftData.Count)
                    {
                        var rightLeft = rightData.Count - j;
                        for (var m = 0; m < rightLeft; m++)
                        {
                            mergeData[i + j] = rightData[j++];
                        }
                        break;
                    }
                }
                else
                {
                    mergeData[i + j] = rightData[j++];
                    if (j == rightData.Count)
                    {
                        var leftleft = leftData.Count - i;
                        for (var n = 0; n < leftleft; n++)
                        {
                            mergeData[i + j] = leftData[i++];
                        }
                        break;
                    }
                }
            }
            return mergeData;

        }
        #endregion

        #region 堆排序
        /// <summary>
        /// 堆排序
        /// 将数列构建为最大堆数列（即父节点总是最大值），将最大值（即根节点）交换到数列末尾。
        /// 这样要排序的数列数总和减少，同时根节点不再是最大值，调整最大堆数列。如此重复，最后得到有序数列。
        /// </summary>
        /// <param name="data"></param>
        public static void HeapSort(IList<int> data)
        {
            BuildMaxHeapify(data);
            var j = data.Count;
            for (var i = 0; i < j; )
            {
                Swap(data, i, --j);
                if (j - 2 < 0)  //只剩下1个数 j代表余下要排列的数的个数
                    break;
                var k = 0;
                while (true)
                {
                    if (k > (j - 2) / 2) break;  //即：k > ((j-1)-1)/2 超出最后一个父节点的位置  
                    else
                    {
                        var temp = k;
                        k = ReSortMaxBranch(data, k, 2 * k + 1, 2 * k + 2, j - 1);
                        if (temp == k) break;
                    }
                }
            }
        }

        public static void BuildMaxHeapify(IList<int> data)
        {
            for (var i = data.Count / 2 - 1; i >= 0; i--)  //(data.Count-1)-1)/2为数列最大父节点索引
            {
                var temp = i;
                temp = ReSortMaxBranch(data, i, 2 * i + 1, 2 * i + 2, data.Count - 1);
                if (temp != i)
                {
                    var k = i;
                    while (k != temp && temp <= data.Count / 2 - 1)
                    {
                        k = temp;
                        temp = ReSortMaxBranch(data, temp, 2 * temp + 1, 2 * temp + 2, data.Count - 1);
                    }
                }
            }
        }

        public static int ReSortMaxBranch(IList<int> data, int maxIndex, int left, int right, int lastIndex)
        {
            int temp;
            if (right > lastIndex)  //父节点只有一个子节点
                temp = left;
            else
            {
                if (data[left] > data[right])
                    temp = left;
                else temp = right;
            }

            if (data[maxIndex] < data[temp])
                Swap(data, maxIndex, temp);
            else temp = maxIndex;
            return temp;
        }
        #endregion

        #region 希尔排序
        /// <summary>
        /// 希尔排序
        /// 通过奇妙的步长，插入排序间隔步长的元素，随后逐渐缩短步长至1，实现数列的插入排序。
        /// </summary>
        /// <param name="data"></param>
        public static void ShellSortCorrect(IList<int> data)
        {
            int temp;
            for (var gap = data.Count / 2; gap > 0; gap /= 2)
            {
                for (var i = gap; i < data.Count; i++)      // i+ = gap 改为了 i++
                {
                    temp = data[i];
                    for (var j = i - gap; j >= 0; j -= gap)
                    {
                        if (data[j] > temp)
                        {
                            data[j + gap] = data[j];
                            if (j == 0)
                            {
                                data[j] = temp;
                                break;
                            }
                        }
                        else
                        {
                            data[j + gap] = temp;
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region 个位数桶排序
        /// <summary>
        /// 个位数桶排序
        /// </summary>
        /// <param name="data"></param>
        public static void BucketSortOnlyUnitDigit(IList<int> data)
        {
            var indexCounter = new int[10];
            for (var i = 0; i < data.Count; i++)
            {
                indexCounter[data[i]]++;
            }
            var indexBegin = new int[10];
            for (var i = 1; i < 10; i++)
            {
                indexBegin[i] = indexBegin[i - 1] + indexCounter[i - 1];
            }
            var tempList = NewInstance(data, data.Count);
            for (var i = 0; i < data.Count; i++)
            {
                var number = data[i];
                tempList[indexBegin[number]++] = data[i];
            }
            data = tempList;
        } 
        #endregion

        #region 基数排序
        /// <summary>
        /// 基数排序
        /// 将整数按位数切割成不同的数字，然后按每个位数分别比较。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<int> RadixSort(IList<int> data)
        {
            var max = data[0];
            for (var i = 1; i < data.Count; i++)
            {
                if (data[i] > max)
                    max = data[i];
            }
            var digit = 1;
            while (max / 10 != 0)
            {
                digit++;
                max /= 10;
            }
            for (var i = 0; i < digit; i++)
            {
                var indexCounter = new int[10];
                var tempList = NewInstance(data, data.Count);
                for (var j = 0; j < data.Count; j++)
                {
                    var number = (data[j] % Convert.ToInt32(Math.Pow(10, i + 1))) / Convert.ToInt32(Math.Pow(10, i));  //得出i+1位上的数
                    indexCounter[number]++;
                }
                var indexBegin = new int[10];
                for (var k = 1; k < 10; k++)
                {
                    indexBegin[k] = indexBegin[k - 1] + indexCounter[k - 1];
                }
                for (var k = 0; k < data.Count; k++)
                {
                    var number = (data[k] % Convert.ToInt32(Math.Pow(10, i + 1))) / Convert.ToInt32(Math.Pow(10, i));
                    tempList[indexBegin[number]++] = data[k];
                }
                data = tempList;
            }
            return data;
        } 
        #endregion

        #region 帮助
        public static void Swap(IList<int> data, int a, int b)
        {
            var temp = data[a];
            data[a] = data[b];
            data[b] = temp;
        }

        public static int[] OrderedSet(int length)
        {
            var result = new int[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = i;
            }
            return result;
        }
 
        public static IList<int> NewInstance(IList<int> data, int length)
        {
            IList<int> instance;
            if (data is Array)
            {
                instance = new int[length];
            }
            else
            {
                instance = new List<int>(length);
                for (var n = 0; n < length; n++)
                {
                    instance.Add(0);  // 初始添加
                }
            }
            return instance;
        }
        #endregion
    }
}
