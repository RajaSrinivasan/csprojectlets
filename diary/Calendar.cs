using System;
namespace diary
{

    public class Calendar
    {
        public Calendar()
        {
        }

        public static DateTime BeginDate()
        {
            DateTime now = System.DateTime.Now;
            for (int off = 1; off < 7; off ++)
            {
                if (now.DayOfWeek == DayOfWeek.Sunday)
                {
                    Console.WriteLine("Found a sunday");   
                    return now.Date;
                }
                else
                {
                    Console.WriteLine($"Searching for sunday. now {now}");
                }
                now = now.AddDays(-1.0);
            }
            return now.Date;
        }
        public static DateTime EndDate(DateTime beg, int weeks)
        {
            DateTime temp = beg;
            temp = temp.AddDays((double)weeks * 7.0);
            temp = temp.AddSeconds(-1.0);
            return temp;

        }
        public static Sprint Create(int weeks)
        {
            Sprint sprint = new Sprint();
            sprint.start = BeginDate();
            sprint.end = EndDate(sprint.start,weeks);
            return sprint;
        }
    }
}
