using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Jaylan.Utilities
{
    /// <summary>
    /// 性能计数器
    /// </summary>
    public static class CodeTimer
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        /// <summary>
        /// 统计CPU时钟周期
        /// 使用P/Invoke访问QueryThreadCycleTime函数
        /// </summary>
        /// <returns></returns>
        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        /// <summary>
        /// 在测试开始前调用。首先它会把当前进程及当前线程的优先级设为最高，这样便可以相对减少操作系统在调度上造成的干扰。
        /// 然后调用一次Time方法进行“预热”，让JIT将IL编译成本地代码，让Time方法尽快“进入状态”。
        /// </summary>
        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Time("", 1, () => { });
        }

        /// <summary>
        /// 性能计数方法
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="iteration">循环次数</param>
        /// <param name="action">需要执行的方法体</param>
        public static void Time(string name, int iteration, Action action)
        {
            if (string.IsNullOrEmpty(name)) return;

            // 1. Print name
            //    保留当前控制台前景色，并使用黄色输出名称参数。
            var currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            // 2. Record the latest GC counts
            //    强制GC进行收集，并记录目前各代已经收集的次数。
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            var gcCounts = new int[GC.MaxGeneration + 1];
            for (var i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 3. Run action
            //    执行代码，记录下消耗的时间及CPU时钟周期
            var watch = new Stopwatch();
            watch.Start();
            var cycleCount = GetCycleCount();
            for (var i = 0; i < iteration; i++) action();
            var cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            // 4. Print CPU
            //    恢复控制台默认前景色，并打印出消耗时间及CPU时钟周期。
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            // 5. Print GC
            //    打印执行过程中各代垃圾收集回收次数。
            for (var i = 0; i <= GC.MaxGeneration; i++)
            {
                var count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }
    }
}
