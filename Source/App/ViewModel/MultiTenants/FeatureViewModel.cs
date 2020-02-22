using System;
using Project.Model;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Core.StaticResource;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class FeatureViewModel : ViewModelBase<Feature>
    {
        public FeatureViewModel(Feature model): base(model)
        {
            EditionId = model.EditionId;

            Name = model.Name;
            Value = model.Value;
            ValueType = model.ValueType;
            ValueTypeName = model.ValueType.GetDescription();
            IsEnabled = model.IsEnabled;
            IsStatic = model.IsStatic;
            Order = model.Order;

            IsFeature = model.IsFeature;
            IsEditionFeature = model.IsEditionFeature;
            IsTenantFeature = model.IsTenantFeature;
        }

        public string EditionId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public FeatureValueType ValueType { get; set; }
        public string ValueTypeName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsStatic { get; set; }

        public bool IsFeature { get; set; }
        public bool IsEditionFeature { get; set; }
        public bool IsTenantFeature { get; set; }

        public int? Order { get; set; }

        public string DisplayMessage => GetFeatureDisplayMessage();

        private string GetFeatureDisplayMessage()
        {
            string message = "";
            if (Name == StaticFeature.Users.Name)
            {
                message = Convert.ToInt32(Value) == 0 ? $"Unlimited {Name}" : $"{Value} {Name}";
            }
            else if (Name == StaticFeature.Warehouse.Name)
            {
                message = $"{Value} {Name}s";
            }
            else if (Name == StaticFeature.HourlyBackup.Name)
            {
                message = $"{Name}";
            }
            else
            {
                message = $"{Value} {Name}";
            }

            return message;
        }
    }
}