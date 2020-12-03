﻿namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using DrinkWater.SettingServices;

    public class ScrollClass
    {

        public const int LIQUIDS = 5;
        public List<Fluid> Fluids;
        public List<double> FluidsAmount;
        public List<BitmapImage> Images;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollClass"/> class.
        /// Get list of consumed fluids per period.
        /// </summary>
        /// <param name="period">Period of consuming fluids.</param>
        /// <param name="userId">User's id.</param>
        public ScrollClass(string period, int userId)
        {
            Fluids = new FliudInfo().GetFluids();
            Images = new List<BitmapImage>();
            FluidsAmount = new List<double>();
            foreach (var fluid in Fluids)
            {
                Images.Add(new ImageHandler().GetImagefromDB(fluid.FliudImage));
            }

            GetTotalAmount(period, userId);
            while (FluidsAmount.Count <= LIQUIDS)
            {
                FluidsAmount.Add(0);
            }
        }

        /// <summary>
        /// Get total amount of fluids per certain period.
        /// </summary>
        /// <param name="period">Period of consuming fluids.</param>
        /// <param name="userId">User's id.</param>
        public void GetTotalAmount(string period, int userId)
        {
            StatisticInfo statisticInfo = new StatisticInfo(userId);
            FluidsAmount.Clear();
            switch (period)
            {
                case "week":
                    {
                        foreach (var fluid in statisticInfo.GetTotalWeekStatistics())
                        {
                            FluidsAmount.Add((double)fluid.Sum);
                        }

                        return;
                    }

                case "month":
                    {
                        foreach (var fluid in statisticInfo.GetTotalMonthStatistics())
                        {
                            FluidsAmount.Add((double)fluid.Sum);
                        }

                        return;
                    }

                case "year":
                    {
                        foreach (var fluid in statisticInfo.GetTotalYearStatistics())
                        {
                            FluidsAmount.Add((double)fluid.Sum);
                        }

                        return;
                    }

                default:
                    throw new Exception("Period is invalid");
            }
        }
    }
}
