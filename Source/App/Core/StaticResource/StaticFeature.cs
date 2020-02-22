using System;
using System.Collections.Generic;
using Project.Core.Enums;
using Project.Core.StaticResource.Models;

namespace Project.Core.StaticResource
{
    public static class StaticFeature
    {
        public  static StaticFeatureDto Users = new StaticFeatureDto
        {
            Name = "Maximum Users",
            Value = "0", //0 = unlimited
            ValueType = FeatureValueType.Number,
            IsStatic = true,
            Order = 1
        };

        public static StaticFeatureDto Showroom = new StaticFeatureDto
        {
            Name = "Maximum Showroom",
            Value = "1",
            ValueType = FeatureValueType.Number,
            IsStatic = true,
            Order = 2
        };
        public static StaticFeatureDto Warehouse = new StaticFeatureDto
        {
            Name = "Maximum Warehouse",
            Value = "4",
            ValueType = FeatureValueType.Number,
            IsStatic = true,
            Order = 3
        };
        public  static StaticFeatureDto MonthlyEmailNotification = new StaticFeatureDto
        {
            Name = "Monthly Email Notification",
            Value = "100",
            ValueType = FeatureValueType.Number,
            IsStatic = true,
            Order = 4
        };
        public  static StaticFeatureDto HourlyBackup = new StaticFeatureDto
        {
            Name = "Hourly Data Backup",
            Value = "true",
            ValueType = FeatureValueType.Boolean,
            IsStatic = true,
            Order = 5
        };

        public static List<string> GetAllName()
        {
            return new List<string>
            {
                Users.Name,
                Showroom.Name,
                Warehouse.Name,
                MonthlyEmailNotification.Name,
                HourlyBackup.Name
            };
        }

        public static List<StaticFeatureDto> GetAll()
        {
            return new List<StaticFeatureDto>
            {
                Users,
                //MultiBranch,
                Showroom,
                Warehouse,
                MonthlyEmailNotification,
                //DailyBackup,
                HourlyBackup,                
            };
        }
    }
}