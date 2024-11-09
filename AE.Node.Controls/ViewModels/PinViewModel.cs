using AE.Core;
using AE.Core.WPF;
using AE.Dal;
using AE.Node.Items;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace AE.Node.Controls.ViewModels
{
    public partial class PinViewModel : ObservableObject
    {
        public PinViewModel(PinItem pinItem)
        {
            color = new LinearGradientBrush(
                WorkflowManager.GetTypeColor(pinItem.Type, FactorType.Tint, 4).ToColor(), 
                WorkflowManager.GetTypeColor(pinItem.Type, FactorType.Shade, 4).ToColor(), 
                45
            );
            title = pinItem.Title;
        }

        public PinViewModel(PinValue pinValue)
        {
            Title = pinValue.Title;

            var defaultValue = WorkflowManager.GetTypeValue(pinValue.Type);

            if (defaultValue != null)
            {
                var type = defaultValue.GetType();

                if (type == typeof(bool))
                    valueType = PinValueType.Bool;
                else if (type == typeof(string))
                    valueType = PinValueType.String;
                else if (type == typeof(byte[]))
                    valueType = PinValueType.Color;
                else if (type == typeof(byte))
                {
                    valueType = PinValueType.Number;
                    maxDecimalPlaces = 0;
                    smallChange = 1;
                    largeChange = 1;
                    minimum = 0;
                    maximum = 255;
                }
                else if (type == typeof(int))
                {
                    valueType = PinValueType.Number;
                    maxDecimalPlaces = 0;
                    smallChange = 1;
                    largeChange = 10;
                    minimum = int.MinValue;
                    maximum = int.MaxValue;
                }
                else if (type == typeof(float))
                {
                    valueType = PinValueType.Number;
                    maxDecimalPlaces = 2;
                    smallChange = 1;
                    largeChange = 10;
                    minimum = float.MinValue;
                    maximum = float.MaxValue;
                }
                else if (type == typeof(double))
                {
                    valueType = PinValueType.Number;
                    maxDecimalPlaces = 4;
                    smallChange = 1;
                    largeChange = 10;
                    minimum = double.MinValue;
                    maximum = double.MaxValue;
                }
                else if (type == typeof(double[]))
                {
                    var arr = (double[])defaultValue;

                    if (arr.Length == 2)
                        valueType = PinValueType.Point;
                    else if (arr.Length == 4)
                        valueType = PinValueType.Range;
                }
                else if (type.IsEnum)
                {
                    valueType = PinValueType.Enum;
                    values = ((Enum)defaultValue).GetDescriptions<Enum>().ToDictionary(d => d.Value, d => d.Description);
                }

                if (valueType != PinValueType.None)
                {
                    if (pinValue.Value != null && type == pinValue.Value.GetType())
                        value = pinValue.Value;
                    else
                        value = defaultValue;
                }
            }
        }

        [ObservableProperty]
        private Brush color = Brushes.Transparent;

        [ObservableProperty]
        private string title = "";

        [ObservableProperty]
        private PinValueType valueType = PinValueType.None;

        [ObservableProperty]
        private object value = null;

        [ObservableProperty]
        private Dictionary<Enum, string> values = [];

        [ObservableProperty]
        private int maxDecimalPlaces = 0;

        [ObservableProperty]
        public double smallChange = 1;

        [ObservableProperty]
        public double largeChange = 1;

        [ObservableProperty]
        public double minimum = double.MinValue;

        [ObservableProperty]
        public double maximum = double.MaxValue;
    }
}
