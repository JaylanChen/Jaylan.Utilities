using System;
using System.Runtime.InteropServices;

namespace Jaylan.Utilities
{
    public class GuidHelper
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);

        /// <summary>
        /// 生成有序 的Guid
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateSequentialId()
        {
            const int RPC_S_OK = 0;
            Guid guid;
            var result = UuidCreateSequential(out guid);
            return result == RPC_S_OK ? guid : Guid.NewGuid();
        }
    }
}
