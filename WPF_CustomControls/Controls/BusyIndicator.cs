﻿using System;
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
using WPF_CustomControls.Models;

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
        #region Dependency Properties

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


        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(BusyIndicator),
            new UIPropertyMetadata(new CornerRadius(0)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        public static DependencyProperty IndicatorCountProperty = DependencyProperty.Register(
            "IndicatorCount",
            typeof(int),
            typeof(BusyIndicator),
            new UIPropertyMetadata(6, OnIndicatorAnimationPropertyChanged),
            new ValidateValueCallback(IsValidIndicatorCount));

        public int IndicatorCount
        {
            get => (int)GetValue(IndicatorCountProperty);
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


        public static DependencyProperty CentralControlTemplateProperty = DependencyProperty.Register(
            "CentralControlTemplate",
            typeof(ControlTemplate),
            typeof(BusyIndicator),
            new UIPropertyMetadata());

        public ControlTemplate CentralControlTemplate
        {
            get => (ControlTemplate)GetValue(CentralControlTemplateProperty);
            set => SetValue(CentralControlTemplateProperty, value);
        }


        public static DependencyProperty SecondaryForegroundProperty = DependencyProperty.Register(
            "SecondaryForeground",
            typeof(Brush),
            typeof(BusyIndicator),
            new UIPropertyMetadata());

        public Brush SecondaryForeground
        {
            get => (Brush)GetValue(SecondaryForegroundProperty);
            set => SetValue(SecondaryForegroundProperty, value);
        }


        public static DependencyProperty TraceBrushProperty = DependencyProperty.Register(
            "TraceBrush",
            typeof(Brush),
            typeof(BusyIndicator),
            new UIPropertyMetadata());

        public Brush TraceBrush
        {
            get => (Brush)GetValue(TraceBrushProperty);
            set => SetValue(TraceBrushProperty, value);
        }


        public static DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
            "EasingFunction",
            typeof(IEasingFunction),
            typeof(BusyIndicator),
            new UIPropertyMetadata(
                new PropertyChangedCallback(OnIndicatorAnimationPropertyChanged)));

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }


        public static DependencyProperty CycleDurationProperty = DependencyProperty.Register(
            "CycleDuration",
            typeof(Duration),
            typeof(BusyIndicator),
            new UIPropertyMetadata(
                new Duration(TimeSpan.FromSeconds(1)),
                new PropertyChangedCallback(OnIndicatorAnimationPropertyChanged)));

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


        public static DependencyProperty ItemsControlAngularVelocityProperty = DependencyProperty.Register(
            "ItemsControlAngularVelocity",
            typeof(double),
            typeof(BusyIndicator),
            new UIPropertyMetadata(0d));

        public double ItemsControlAngularVelocity
        {
            get => (double)GetValue(ItemsControlAngularVelocityProperty);
            set => SetValue(ItemsControlAngularVelocityProperty, value);
        }

        #endregion

        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public BusyIndicator()
        {
            SetupAngleAndPhaseList();
        }


        private static void OnIndicatorAnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BusyIndicator indicator)
            {
                indicator.SetupAngleAndPhaseList();
            }
        }

        private static bool IsValidIndicatorCount(object value)
        {
            var intVal = (int)value;
            return intVal >= 0 && intVal <= 64;
        }


        private void SetupAngleAndPhaseList()
        {
            // Calculate interval between shapes
            var count = IndicatorCount;
            var phaseInterval = CycleDuration.TimeSpan / (count * 5);
            var angleInterval = 360d / (count * 1);
            
            var angleAndPhases = new List<AngleAndPhase>();
            for (int i = 0; i < count; i++)
            {
                var angleAndPhase = new AngleAndPhase(
                    i,
                    i * angleInterval,
                    i * phaseInterval,
                    CycleDuration,
                    EasingFunction);
                angleAndPhases.Add(angleAndPhase);
            }
            AngleAndPhaseList = angleAndPhases;
            // TODO trigger the animation to start again
        }

        internal class AngleAndPhase
        { 
            public int Index { get; set; }
            public double StartAngle { get; set; }
            public double FinalAngle { get; set; }
            public TimeSpan Phase { get; set; }
            public Duration Duration { get; set; }
            public IEasingFunction? EasingFunction { get; set; }

            public AngleAndPhase(int index, double startAngle, TimeSpan phase, Duration duration, IEasingFunction? easingFunction)
            {
                Index = index;
                StartAngle = startAngle;
                FinalAngle = 360 + startAngle;
                Phase = phase;
                Duration = duration;
                EasingFunction = easingFunction;
            }
        }
    }
}
