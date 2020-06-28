using System;
using System.Linq;
using LIBMAData.Model;
using System.Collections.Generic;

namespace LIBMAService
{
    public class DataHelpers
    {
        public static IEnumerable<string> HumanizeBusinessHours(IEnumerable<BranchHours> branchHours)
        {
            var hours = new List<string>();

            foreach (var item in branchHours)
            {
                var day = HumanizeDay(item.DayOfWeek);
                var timeOpen = HumanizeTime(item.OpenTime);
                var timeClose = HumanizeTime(item.CloseTime);

                var timeEntry = $"{day} {timeOpen} to {timeClose}";
                hours.Add(timeEntry);
            }
            return hours;
        }

        private static string HumanizeTime(int number)
        {
            return Enum.GetName(typeof(DayOfWeek), number);

        }

        private static string HumanizeDay(int time)
        {
            return TimeSpan.FromHours(time).ToString("hh':'mm");
        }
    }
}