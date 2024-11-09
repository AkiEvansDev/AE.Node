using AE.Core.WPF;
using AE.Dal;
using AE.Node.Controls.Models;
using AE.Node.Items;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Wpf.Ui.Markup;

namespace AE.Node.Controls.ViewModels
{
    public partial class NodeViewModel : ObservableObject
    {
        public readonly int Id;
        public readonly string Type;

        private readonly string NodeColor;

        public NodeViewModel(NodeItem nodeItem)
        {
            Id = nodeItem.Id;
            Type = nodeItem.Type;

            NodeColor = nodeItem.Color;

            color = new LinearGradientBrush(
                NodeColor.ToColor(FactorType.Shade, 5),
                NodeColor.ToColor(), 
                25
            );

            title = nodeItem.Title;

            foreach (var pinItem in nodeItem.In)
                inPins.Add(new PinViewModel(pinItem));

            foreach (var pinItem in nodeItem.Out)
                outPins.Add(new PinViewModel(pinItem));

            foreach (var value in nodeItem.Values)
                values.Add(new PinViewModel(value));

            pin = nodeItem.Pin;
            enabled = nodeItem.Enabled;
            collapse = nodeItem.Collapse;
            state = NodeBorderState.Normal;
            shape = nodeItem.Shape;

            RefreshState();
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected; 
            set
            {
                isSelected = value;
                RefreshState();
            }
        }

        private NodeBorderState state;
        public NodeBorderState State
        {
            get => state;
            set
            {
                state = value;
                RefreshState();
            }
        }

        private NodeShapeType shape;
        public NodeShapeType Shape
        {
            get => shape;
            set
            {
                shape = value;
                RefreshState();
            }
        }

        [ObservableProperty]
        private Brush color = Brushes.Transparent;

        [ObservableProperty]
        private Brush border = Brushes.Transparent;

        [ObservableProperty]
        private CornerRadius borderRadius = new(0);

        [ObservableProperty]
        private CornerRadius innerRadius = new(0);

        [ObservableProperty]
        private string title = "";

        [ObservableProperty]
        private bool pin = false;

        [ObservableProperty]
        private bool enabled = false;

        private bool collapse;
        public bool Collapse
        {
            get => collapse;
            set
            {
                SetProperty(ref collapse, value);
                RefreshState();
            }
        }

        [ObservableProperty]
        private ObservableCollection<ActionButtonModel> actions = [];

        [ObservableProperty]
        private ObservableCollection<PinViewModel> inPins = [];

        [ObservableProperty]
        private ObservableCollection<PinViewModel> outPins = [];

        [ObservableProperty]
        private ObservableCollection<PinViewModel> values = [];

        private void RefreshState()
        {
            switch (State)
            {
                case NodeBorderState.Normal:
                    Border = IsSelected 
                        ? new LinearGradientBrush(new GradientStopCollection([
                            new GradientStop(NodeColor.ToColor(FactorType.Shade, 5), 0),
                            new GradientStop(ThemeResource.SolidBackgroundFillColorTertiary.GetResourseColor(), 0.4),
                            new GradientStop(ThemeResource.SolidBackgroundFillColorTertiary.GetResourseColor(), 0.6),
                            new GradientStop(NodeColor.ToColor(), 1)
                        ]), 45)
                        : new SolidColorBrush(ThemeResource.SolidBackgroundFillColorTertiary.GetResourseColor());
                    break;
                case NodeBorderState.Debug:
                    var brush = new LinearGradientBrush(new GradientStopCollection([
                        new GradientStop(ThemeResource.SolidBackgroundFillColorTertiary.GetResourseColor(), 0),
                        new GradientStop(ThemeResource.TextFillColorPrimary.GetResourseColor(), 0.5),
                        new GradientStop(ThemeResource.SolidBackgroundFillColorTertiary.GetResourseColor(), 1)
                    ]));

                    brush.BeginAnimation(LinearGradientBrush.EndPointProperty, new PointAnimation
                    {
                        RepeatBehavior = RepeatBehavior.Forever,
                        From = new Point(0, 0),
                        To = new Point(1, 1.5),
                        Duration = TimeSpan.FromSeconds(0.5),
                        AutoReverse = true,
                    });

                    Border = brush;
                    break;
                case NodeBorderState.Error:
                    Border = IsSelected
                        ? new LinearGradientBrush(new GradientStopCollection([
                            new GradientStop(NodeColor.ToColor(FactorType.Shade, 5), 0),
                            new GradientStop(ThemeResource.SystemFillColorCritical.GetResourseColor(), 0.4),
                            new GradientStop(ThemeResource.SystemFillColorCritical.GetResourseColor(), 0.6),
                            new GradientStop(NodeColor.ToColor(), 1)
                        ]), 45)
                        : new SolidColorBrush(ThemeResource.SystemFillColorCritical.GetResourseColor());
                    break;
            }

            switch (Shape)
            {
                case NodeShapeType.Box:
                    BorderRadius = new CornerRadius(0);
                    InnerRadius = new CornerRadius(0);
                    break;
                case NodeShapeType.Round:
                    BorderRadius = new CornerRadius(6);
                    InnerRadius = Collapse 
                        ? new CornerRadius(6)
                        : new CornerRadius(4, 4, 0, 0);
                    break;
            }
        }
    }
}
