using System;
using System.Collections.Generic;
using Project.Core.Enums;
using Project.Core.StaticResource.Models;

namespace Project.Core.StaticResource
{
    public static class StaticEditionFeature
    {
        public class TrialEdition
        {
            public static StaticEditionFeatureDto Users = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Trial,
                Name = StaticFeature.Users.Name,
                Value = "3",
                ValueType = StaticFeature.Users.ValueType,
            };
            public static StaticEditionFeatureDto Showroom = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Trial,
                Name = StaticFeature.Showroom.Name,
                Value = "1",
                ValueType = StaticFeature.Showroom.ValueType,
            };
            public static StaticEditionFeatureDto Warehouse = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Trial,
                Name = StaticFeature.Warehouse.Name,
                Value = "4",
                ValueType = StaticFeature.Warehouse.ValueType,
            };
            public static StaticEditionFeatureDto MonthlyEmailNotification = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Trial,
                Name = StaticFeature.MonthlyEmailNotification.Name,
                Value = "100",
                ValueType = StaticFeature.MonthlyEmailNotification.ValueType,
            };

            public static StaticEditionFeatureDto HourlyBackup = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Trial,
                Name = StaticFeature.HourlyBackup.Name,
                Value = "false",
                ValueType = StaticFeature.HourlyBackup.ValueType,
            };
        }

        public class StarterEdition
        {
            public static StaticEditionFeatureDto Users = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Starter,
                Name = StaticFeature.Users.Name,
                Value = "3",
                ValueType = StaticFeature.Users.ValueType,
                Order = StaticFeature.Users.Order
            };

            public static StaticEditionFeatureDto Showroom = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Starter,
                Name = StaticFeature.Showroom.Name,
                Value = "1",
                ValueType = StaticFeature.Showroom.ValueType,
                Order = StaticFeature.Showroom.Order
            };
            public static StaticEditionFeatureDto Warehouse = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Starter,
                Name = StaticFeature.Warehouse.Name,
                Value = "4",
                ValueType = StaticFeature.Warehouse.ValueType,
                Order = StaticFeature.Warehouse.Order
            };
            public static StaticEditionFeatureDto MonthlyEmailNotification = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Starter,
                Name = StaticFeature.MonthlyEmailNotification.Name,
                Value = "500",
                ValueType = StaticFeature.MonthlyEmailNotification.ValueType,
                Order = StaticFeature.MonthlyEmailNotification.Order
            };

            public static StaticEditionFeatureDto HourlyBackup = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Starter,
                Name = StaticFeature.HourlyBackup.Name,
                Value = "true",
                ValueType = StaticFeature.HourlyBackup.ValueType,
                Order = StaticFeature.HourlyBackup.Order
            };
        }

        public class StandardEdition
        {
            public static StaticEditionFeatureDto Users = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Stanard,
                Name = StaticFeature.Users.Name,
                Value = "6",
                ValueType = StaticFeature.Users.ValueType,
                Order = StaticFeature.Users.Order
            };

            public static StaticEditionFeatureDto Showroom = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Stanard,
                Name = StaticFeature.Showroom.Name,
                Value = "2",
                ValueType = StaticFeature.Showroom.ValueType,
                Order = StaticFeature.Showroom.Order
            };
            public static StaticEditionFeatureDto Warehouse = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Stanard,
                Name = StaticFeature.Warehouse.Name,
                Value = "8",
                ValueType = StaticFeature.Warehouse.ValueType,
                Order = StaticFeature.Warehouse.Order
            };
            public static StaticEditionFeatureDto MonthlyEmailNotification = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Stanard,
                Name = StaticFeature.MonthlyEmailNotification.Name,
                Value = "1000",
                ValueType = StaticFeature.MonthlyEmailNotification.ValueType,
                Order = StaticFeature.MonthlyEmailNotification.Order
            };

            public static StaticEditionFeatureDto HourlyBackup = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Stanard,
                Name = StaticFeature.HourlyBackup.Name,
                Value = "true",
                ValueType = StaticFeature.HourlyBackup.ValueType,
                Order = StaticFeature.HourlyBackup.Order
            };
        }

        public class AdvancedEdition
        {
            public static StaticEditionFeatureDto Users = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Advanced,
                Name = StaticFeature.Users.Name,
                Value = "9",
                ValueType = StaticFeature.Users.ValueType,
                Order = StaticFeature.Users.Order
            };
            public static StaticEditionFeatureDto Showroom = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Advanced,
                Name = StaticFeature.Showroom.Name,
                Value = "3",
                ValueType = StaticFeature.Showroom.ValueType,
                Order = StaticFeature.Showroom.Order
            };
            public static StaticEditionFeatureDto Warehouse = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Advanced,
                Name = StaticFeature.Warehouse.Name,
                Value = "12",
                ValueType = StaticFeature.Warehouse.ValueType,
                Order = StaticFeature.Warehouse.Order
            };
            public static StaticEditionFeatureDto MonthlyEmailNotification = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Advanced,
                Name = StaticFeature.MonthlyEmailNotification.Name,
                Value = "1500",
                ValueType = StaticFeature.MonthlyEmailNotification.ValueType,
                Order = StaticFeature.MonthlyEmailNotification.Order
            };
            public static StaticEditionFeatureDto HourlyBackup = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Advanced,
                Name = StaticFeature.HourlyBackup.Name,
                Value = "true",
                ValueType = StaticFeature.HourlyBackup.ValueType,
                Order = StaticFeature.HourlyBackup.Order
            };
        }

        public class DeluxeEdition
        {
            public static StaticEditionFeatureDto Users = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Deluxe,
                Name = StaticFeature.Users.Name,
                Value = "15",
                ValueType = StaticFeature.Users.ValueType,
                Order = StaticFeature.Users.Order
            };

            public static StaticEditionFeatureDto Showroom = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Deluxe,
                Name = StaticFeature.Showroom.Name,
                Value = "5",
                ValueType = StaticFeature.Showroom.ValueType,
                Order = StaticFeature.Showroom.Order
            };
            public static StaticEditionFeatureDto Warehouse = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Deluxe,
                Name = StaticFeature.Warehouse.Name,
                Value = "20",
                ValueType = StaticFeature.Warehouse.ValueType,
                Order = StaticFeature.Warehouse.Order
            };
            public static StaticEditionFeatureDto MonthlyEmailNotification = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Deluxe,
                Name = StaticFeature.MonthlyEmailNotification.Name,
                Value = "2500",
                ValueType = StaticFeature.MonthlyEmailNotification.ValueType,
                Order = StaticFeature.MonthlyEmailNotification.Order
            };

            public static StaticEditionFeatureDto HourlyBackup = new StaticEditionFeatureDto
            {
                EditionName = StaticEdition.Deluxe,
                Name = StaticFeature.HourlyBackup.Name,
                Value = "true",
                ValueType = StaticFeature.HourlyBackup.ValueType,
                Order = StaticFeature.HourlyBackup.Order
            };
        }


        public static List<StaticEditionFeatureDto> GetAll()
        {
            return new List<StaticEditionFeatureDto>
            {
                TrialEdition.Users,
                TrialEdition.Showroom,
                TrialEdition.Warehouse,
                TrialEdition.MonthlyEmailNotification,
                TrialEdition.HourlyBackup,

                StarterEdition.Users,
                StarterEdition.Showroom,
                StarterEdition.Warehouse,
                StarterEdition.MonthlyEmailNotification,
                StarterEdition.HourlyBackup,

                StandardEdition.Users,
                StandardEdition.Showroom,
                StandardEdition.Warehouse,
                StandardEdition.MonthlyEmailNotification,
                StandardEdition.HourlyBackup,

                AdvancedEdition.Users,
                AdvancedEdition.Showroom,
                AdvancedEdition.Warehouse,
                AdvancedEdition.MonthlyEmailNotification,
                AdvancedEdition.HourlyBackup,

                DeluxeEdition.Users,
                DeluxeEdition.Showroom,
                DeluxeEdition.Warehouse,
                DeluxeEdition.MonthlyEmailNotification,
                DeluxeEdition.HourlyBackup,
            };
        }
    }
}