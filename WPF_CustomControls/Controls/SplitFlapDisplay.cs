using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
    ///     xmlns:MyNamespace="clr-namespace:WPF_CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_CustomControls;assembly=WPF_CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class SplitFlapDisplay : Control
    {
        private readonly static char[] _defaultCharacterSet = new char[] {
        ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
        'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4',
        '5', '6', '7', '8', '9'};

        private string _currentlyDisplayedText = string.Empty;
        private bool _suppressDisplayUpdate;

        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(SplitFlapDisplay),
            new FrameworkPropertyMetadata(string.Empty, onTextChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void onTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SplitFlapDisplay display)
            {
                display.AnimateChangedText((string)e.NewValue, (string)e.OldValue);
            }
        }

        public static DependencyProperty FlapDurationProperty = DependencyProperty.Register(
            "FlapDuration",
            typeof(Duration),
            typeof(SplitFlapDisplay),
            new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(10))));

        public Duration FlapDuration
        {
            get => (Duration)GetValue(FlapDurationProperty);
            set => SetValue(FlapDurationProperty, value);
        }

        public static DependencyProperty CharacterSetProperty = DependencyProperty.Register(
            "CharacterSet",
            typeof(char[]),
            typeof(SplitFlapDisplay),
            new FrameworkPropertyMetadata(_defaultCharacterSet));

        public char[] CharacterSet
        {
            get => (char[])GetValue(CharacterSetProperty);
            set => SetValue(CharacterSetProperty, value);
        }

        public static DependencyProperty DisplayLengthProperty = DependencyProperty.Register(
            "DisplayLength",
            typeof(int),
            typeof(SplitFlapDisplay),
            new FrameworkPropertyMetadata(16));

        public int DisplayLength
        {
            get => (int)GetValue(DisplayLengthProperty);
            set => SetValue(DisplayLengthProperty, value);
        }

        public static DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment",
            typeof(TextAlignment),
            typeof(SplitFlapDisplay),
            new FrameworkPropertyMetadata(TextAlignment.Left));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        static SplitFlapDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(typeof(SplitFlapDisplay)));
        }

        static ControlTemplate CreateDefaultControlTemplate()
        {
            return new ControlTemplate();
        }

        private void AnimateChangedText(string newText, string oldText)
        {
            if (_suppressDisplayUpdate)
            {
                _suppressDisplayUpdate = false;
                return;
            }
            var charSet = CharacterSet;
            if (newText.Any(x => !charSet.Contains(x)))
            {
                // Reset the text property to old text, and return
                _suppressDisplayUpdate = true;
                Text = oldText;
                return;
            }
            else if (newText.Length > DisplayLength)
            {
                // Trim the text down to the length of the display
                newText = newText.Substring(0, DisplayLength);
                _suppressDisplayUpdate = true;
                Text = newText;
            }
            var duration = FlapDuration;
            // cycle currently displayed text through letters to new text
        }
    }
}