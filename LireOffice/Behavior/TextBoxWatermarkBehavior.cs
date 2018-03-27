using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace LireOffice.Behavior
{
    public class TextBoxWatermarkBehavior : System.Windows.Interactivity.Behavior<TextBox>
    {
        private TextBlockAdorner adorner;
        private WeakPropertyChangeNotifier notifier;

        #region DependencyProperty's

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached("Label", typeof(string), typeof(TextBoxWatermarkBehavior));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.RegisterAttached("LabelStyle", typeof(Style), typeof(TextBoxWatermarkBehavior));

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.AssociatedObjectLoaded;
            this.AssociatedObject.TextChanged += this.AssociatedObjectTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= this.AssociatedObjectLoaded;
            this.AssociatedObject.TextChanged -= this.AssociatedObjectTextChanged;

            this.notifier = null;
        }

        private void AssociatedObjectTextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateAdorner();
        }

        private void AssociatedObjectLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.adorner = new TextBlockAdorner(this.AssociatedObject, this.Label, this.LabelStyle);

            this.UpdateAdorner();

            //AddValueChanged for IsFocused in a weak manner
            this.notifier = new WeakPropertyChangeNotifier(this.AssociatedObject, UIElement.IsFocusedProperty);
            this.notifier.ValueChanged += new EventHandler(this.UpdateAdorner);
        }

        private void UpdateAdorner(object sender, EventArgs e)
        {
            this.UpdateAdorner();
        }


        private void UpdateAdorner()
        {
            if (!String.IsNullOrEmpty(this.AssociatedObject.Text) || this.AssociatedObject.IsFocused)
            {
                // Hide the Watermark Label if the adorner layer is visible
                this.AssociatedObject.TryRemoveAdorners<TextBlockAdorner>();
            }
            else
            {
                // Show the Watermark Label if the adorner layer is visible
                this.AssociatedObject.TryAddAdorner<TextBlockAdorner>(adorner);
            }
        }
    }

    public static class AdornerExtensions
    {
        public static void TryRemoveAdorners<T>(this UIElement elem) where T : System.Windows.Documents.Adorner
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(elem);
            if (adornerLayer != null)
            {
                adornerLayer.RemoveAdorners<T>(elem);
            }
        }

        public static void RemoveAdorners<T>(this AdornerLayer adr, UIElement elem)
            where T : Adorner
        {
            Adorner[] adorners = adr.GetAdorners(elem);

            if (adorners == null) return;

            for (int i = adorners.Length - 1; i >= 0; i--)
            {
                if (adorners[i] is T)
                    adr.Remove(adorners[i]);
            }
        }

        public static void TryAddAdorner<T>(this UIElement elem, System.Windows.Documents.Adorner adorner)
            where T : Adorner
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(elem);
            if (adornerLayer != null && !adornerLayer.ContainsAdorner<T>(elem))
            {
                adornerLayer.Add(adorner);
            }
        }

        public static bool ContainsAdorner<T>(this AdornerLayer adr, UIElement elem)
            where T : Adorner
        {
            Adorner[] adorners = adr.GetAdorners(elem);

            if (adorners == null) return false;

            for (int i = adorners.Length - 1; i >= 0; i--)
            {
                if (adorners[i] is T)
                    return true;
            }
            return false;
        }

        public static void RemoveAllAdorners(this AdornerLayer adr, UIElement elem)
        {
            Adorner[] adorners = adr.GetAdorners(elem);

            if (adorners == null) return;

            foreach (var toRemove in adorners)
                adr.Remove(toRemove);
        }
    }

    public class TextBlockAdorner : Adorner
    {
        private readonly TextBlock adornerTextBlock;

        public TextBlockAdorner(UIElement adornedElement, string label, Style labelStyle) : base(adornedElement)
        {
            adornerTextBlock = new TextBlock { Style = labelStyle, Text = label };
        }

        protected override Size MeasureOverride(Size constraint)
        {
            adornerTextBlock.Measure(constraint);
            return constraint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            adornerTextBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return adornerTextBlock;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }

    public class WeakPropertyChangeNotifier : DependencyObject, IDisposable
    {
        #region Member Variables
        private readonly WeakReference _propertySource;
        #endregion // Member Variables

        #region Constructor
        public WeakPropertyChangeNotifier(DependencyObject propertySource, string path)
            : this(propertySource, new PropertyPath(path))
        {
        }
        public WeakPropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
            : this(propertySource, new PropertyPath(property))
        {
        }
        public WeakPropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
        {
            if (null == propertySource)
                throw new ArgumentNullException("propertySource");
            this._propertySource = new WeakReference(propertySource);

            Binding binding = new Binding
            {
                Path = property ?? throw new ArgumentNullException("property"),
                Mode = BindingMode.OneWay,
                Source = propertySource
            };
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }
        #endregion // Constructor

        #region PropertySource
        public DependencyObject PropertySource
        {
            get
            {
                try
                {
                    // note, it is possible that accessing the target property
                    // will result in an exception so i’ve wrapped this check
                    // in a try catch
                    return this._propertySource.IsAlive
                    ? this._propertySource.Target as DependencyObject
                    : null;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion // PropertySource

        #region Value
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
        typeof(object), typeof(WeakPropertyChangeNotifier), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WeakPropertyChangeNotifier notifier = (WeakPropertyChangeNotifier)d;
            notifier.ValueChanged?.Invoke(notifier, EventArgs.Empty);
        }

        /// <summary>
        /// Returns/sets the value of the property
        /// </summary>
        /// <seealso cref="ValueProperty"/>
        [Description("Returns/sets the value of the property")]
        [Category("Behavior")]
        [Bindable(true)]
        public object Value
        {
            get
            {
                return (object)this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }
        #endregion //Value

        #region Events
        public event EventHandler ValueChanged;
        #endregion // Events

        #region IDisposable Members
        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ValueProperty);
        }
        #endregion
    }
}
