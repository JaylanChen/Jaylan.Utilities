using System.Linq;
using System.Management;

namespace Jaylan.Utilities
{
    public class GetMethodTimer
    {
        private static void GetProcessor()
        {
            var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
            var cpuTimes = searcher.Get().Cast<ManagementObject>().Select(mo => new
            {
                Name = mo["Name"],
                Usage = mo["PercentProcessorTime"]
            }).ToList();
            var query = cpuTimes.Where(x => x.Name.ToString() == "_Total").Select(x => x.Usage);
            var cpuUsage = query.SingleOrDefault();
        }
    }
}
