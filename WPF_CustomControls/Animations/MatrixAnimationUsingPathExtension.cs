using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPF_CustomControls.Animations
{
    public class MatrixAnimationUsingPathExtension : MatrixAnimationBase
    {
        private bool _isValid;

        private Vector _accumulatingOffset;

        private double _accumulatingAngle;

        /// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.MatrixAnimationUsingPath.DoesRotateWithTangent" /> dependency property.</summary>
        public static readonly DependencyProperty DoesRotateWithTangentProperty = DependencyProperty.Register("DoesRotateWithTangent", typeof(bool), typeof(MatrixAnimationUsingPathExtension), new PropertyMetadata(false));

        /// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.MatrixAnimationUsingPath.IsAngleCumulative" /> dependency property.</summary>
        public static readonly DependencyProperty IsAngleCumulativeProperty = DependencyProperty.Register("IsAngleCumulative", typeof(bool), typeof(MatrixAnimationUsingPathExtension), new PropertyMetadata(false));

        /// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.MatrixAnimationUsingPath.IsOffsetCumulative" /> dependency property.</summary>
        public static readonly DependencyProperty IsOffsetCumulativeProperty = DependencyProperty.Register("IsOffsetCumulative", typeof(bool), typeof(MatrixAnimationUsingPathExtension), new PropertyMetadata(false));

        /// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.MatrixAnimationUsingPath.PathGeometry" /> dependency property.</summary>
        public static readonly DependencyProperty PathGeometryProperty = DependencyProperty.Register("PathGeometry", typeof(PathGeometry), typeof(MatrixAnimationUsingPathExtension), new PropertyMetadata((object)null));

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(MatrixAnimationUsingPathExtension), new PropertyMetadata());

        /// <summary>Gets or sets a value indicating whether the object rotates along the tangent of the path.</summary>
        /// <returns>
        ///   <see langword="true" /> if the object will rotate along the tangent of the path; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
        public bool DoesRotateWithTangent
        {
            get =>(bool)GetValue(DoesRotateWithTangentProperty);
            set => SetValue(DoesRotateWithTangentProperty, value);
        }

        /// <summary>Gets or sets a value that indicates whether the target property's current value should be added to this animation's starting value.</summary>
        /// <returns>
        ///   <see langword="true" /> if the target property's current value should be added to this animation's starting value; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
        public bool IsAdditive
        {
            get
            {
                return (bool)GetValue(AnimationTimeline.IsAdditiveProperty);
            }
            set
            {
                SetValue(AnimationTimeline.IsAdditiveProperty, value);
            }
        }

        /// <summary>Gets or sets a value that specifies whether the rotation angle of the animated matrix should accumulate over repetitions.</summary>
        /// <returns>
        ///   <see langword="true" /> if the animation's rotation angle should accumulate over repetitions; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
        public bool IsAngleCumulative
        {
            get
            {
                return (bool)GetValue(IsAngleCumulativeProperty);
            }
            set
            {
                SetValue(IsAngleCumulativeProperty, value);
            }
        }

        /// <summary>Gets or sets a value indicating whether the offset produced by the animated matrix will accumulate over repetitions.</summary>
        /// <returns>
        ///   <see langword="true" /> if the object will accumulate over repeats of the animation; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
        public bool IsOffsetCumulative
        {
            get
            {
                return (bool)GetValue(IsOffsetCumulativeProperty);
            }
            set
            {
                SetValue(IsOffsetCumulativeProperty, value);
            }
        }

        /// <summary>Gets or sets the geometry used to generate this animation's output values.</summary>
        /// <returns>The geometry used to generate this animation's output values. The default is <see langword="null" />.</returns>
        public PathGeometry PathGeometry
        {
            get
            {
                return (PathGeometry)GetValue(PathGeometryProperty);
            }
            set
            {
                SetValue(PathGeometryProperty, value);
            }
        }

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.MatrixAnimationUsingPath" /> class.</summary>
        public MatrixAnimationUsingPathExtension()
        {
        }

        /// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.MatrixAnimationUsingPath" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
        /// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true." /></returns>
        public new MatrixAnimationUsingPathExtension Clone()
        {
            return (MatrixAnimationUsingPathExtension)base.Clone();
        }

        /// <summary>Creates a new instance of the <see cref="T:System.Windows.Media.Animation.MatrixAnimationUsingPath" />.</summary>
        /// <returns>The new instance.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new MatrixAnimationUsingPathExtension();
        }

        /// <summary>Called when this <see cref="T:System.Windows.Media.Animation.MatrixAnimationUsingPath" /> is modified.</summary>
        protected override void OnChanged()
        {
            _isValid = false;
            base.OnChanged();
        }

        /// <summary>Calculates a value that represents the current value of the property being animated, as determined by the <see cref="T:System.Windows.Media.Animation.MatrixAnimationUsingPath" />.</summary>
        /// <param name="defaultOriginValue">The suggested origin value, used if the animation does not have its own explicitly set start value.</param>
        /// <param name="defaultDestinationValue">The suggested destination value, used if the animation does not have its own explicitly set end value.</param>
        /// <param name="animationClock">An <see cref="T:System.Windows.Media.Animation.AnimationClock" /> that generates the <see cref="P:System.Windows.Media.Animation.Clock.CurrentTime" /> or <see cref="P:System.Windows.Media.Animation.Clock.CurrentProgress" /> used by the animation.</param>
        /// <returns>The calculated value of the property, as determined by the current animation.</returns>
        protected override Matrix GetCurrentValueCore(Matrix defaultOriginValue, Matrix defaultDestinationValue, AnimationClock animationClock)
        {
            PathGeometry pathGeometry = PathGeometry;
            if (pathGeometry == null)
            {
                return defaultDestinationValue;
            }
            if (!_isValid)
            {
                Validate();
            }
            double clockValue = animationClock.CurrentProgress.Value;
            IEasingFunction easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                clockValue = easingFunction.Ease(clockValue);
            }
            pathGeometry.GetPointAtFractionLength(clockValue, out var point, out var tangent);
            double num = 0.0;
            if (DoesRotateWithTangent)
            {
                num = CalculateAngleFromTangentVector(tangent.X, tangent.Y);
            }
            Matrix matrix = default(Matrix);
            double num2 = (animationClock.CurrentIteration - 1).Value;
            if (num2 > 0.0)
            {
                if (IsOffsetCumulative)
                {
                    point += _accumulatingOffset * num2;
                }
                if (DoesRotateWithTangent && IsAngleCumulative)
                {
                    num += _accumulatingAngle * num2;
                }
            }
            matrix.Rotate(num);
            matrix.Translate(point.X, point.Y);
            if (IsAdditive)
            {
                return Matrix.Multiply(matrix, defaultOriginValue);
            }
            return matrix;
        }

        private void Validate()
        {
            if (IsOffsetCumulative || IsAngleCumulative)
            {
                PathGeometry pathGeometry = PathGeometry;
                pathGeometry.GetPointAtFractionLength(0.0, out var point, out var tangent);
                pathGeometry.GetPointAtFractionLength(1.0, out var point2, out var tangent2);
                _accumulatingAngle = CalculateAngleFromTangentVector(tangent2.X, tangent2.Y) - CalculateAngleFromTangentVector(tangent.X, tangent.Y);
                _accumulatingOffset.X = point2.X - point.X;
                _accumulatingOffset.Y = point2.Y - point.Y;
            }
            _isValid = true;
        }

        internal static double CalculateAngleFromTangentVector(double x, double y)
        {
            double num = Math.Acos(x) * (180.0 / Math.PI);
            if (y < 0.0)
            {
                num = 360.0 - num;
            }
            return num;
        }
    }
}
