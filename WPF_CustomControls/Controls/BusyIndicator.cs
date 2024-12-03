using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_CustomControls.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_CustomControls.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_CustomControls.Controls;assembly=WPF_CustomControls.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class BusyIndicator : Control
    {
        public static DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new FrameworkPropertyMetadata(false));

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public static DependencyProperty NotBusyVisibilityProperty = DependencyProperty.Register(
            "NotBusyVisibility",
            typeof(Visibility),
            typeof(BusyIndicator),
            new UIPropertyMetadata(Visibility.Hidden));

        public Visibility NotBusyVisibility
        {
            get => (Visibility)GetValue(NotBusyVisibilityProperty);
            set => SetValue(NotBusyVisibilityProperty, value);
        }

        public static DependencyProperty IndicatorCountProperty = DependencyProperty.Register(
            "IndicatorCount",
            typeof(uint),
            typeof(BusyIndicator),
            new UIPropertyMetadata((uint)6, OnIndicatorCountChanged));


        public uint IndicatorCount
        {
            get => (uint)GetValue(IndicatorCountProperty);
            set => SetValue(IndicatorCountProperty, value);
        }

        public static DependencyProperty IndicatorTemplateProperty = DependencyProperty.Register(
            "IndicatorTemplate",
            typeof(ControlTemplate),
            typeof(BusyIndicator),
            new UIPropertyMetadata());

        public ControlTemplate IndicatorTemplate
        {
            get => (ControlTemplate)GetValue(IndicatorTemplateProperty);
            set => SetValue(IndicatorTemplateProperty, value);
        }

        public static DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
            "EasingFunction",
            typeof(IEasingFunction),
            typeof(BusyIndicator),
            new UIPropertyMetadata());

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public static DependencyProperty CycleDurationProperty = DependencyProperty.Register(
            "CycleDuration",
            typeof(Duration),
            typeof(BusyIndicator),
            new UIPropertyMetadata(new Duration(TimeSpan.FromSeconds(1))));

        public Duration CycleDuration
        {
            get => (Duration)GetValue(CycleDurationProperty);
            set => SetValue(CycleDurationProperty, value);
        }

        public static DependencyProperty AngleAndPhaseListProperty = DependencyProperty.Register(
            "AngleAndPhaseList",
            typeof(IEnumerable<AngleAndPhase>),
            typeof(BusyIndicator),
            new UIPropertyMetadata(new List<AngleAndPhase>()));

        internal IEnumerable<AngleAndPhase> AngleAndPhaseList
        {
            get => (IEnumerable<AngleAndPhase>)GetValue(AngleAndPhaseListProperty);
            private set => SetValue(AngleAndPhaseListProperty, value);
        }

        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public BusyIndicator()
        {
            SetupAngleAndPhaseList();
        }

        private static void OnIndicatorCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BusyIndicator indicator)
            {
                indicator.SetupAngleAndPhaseList();
            }
        }

        private void SetupAngleAndPhaseList()
        {
            // Calculate interval between shapes
            var count = IndicatorCount;
            var phaseInterval = CycleDuration.TimeSpan / count;
            var angleInterval = 360d / count;
            
            var angleAndPhases = new List<AngleAndPhase>();
            for (int i = 0; i < count; i++)
            {
                var angleAndPhase = new AngleAndPhase(i * angleInterval, i * phaseInterval);
                angleAndPhases.Add(angleAndPhase);
            }
            AngleAndPhaseList = angleAndPhases;
            // TODO trigger the animation to start again
        }

        internal class AngleAndPhase
        { 
            public double StartAngle { get; set; }
            public double FinalAngle { get; set; }
            public TimeSpan Phase { get; set; }

            public AngleAndPhase(double startAngle, TimeSpan phase)
            {
                StartAngle = startAngle;
                FinalAngle = 360 + startAngle;
                Phase = phase;
            }
        }
    }
}
