using System;
using System.Windows;
using System.Windows.Controls;

namespace RedisViewer.UI.Controls
{
    [TemplateVisualState(GroupName = "ActiveStates", Name = "Active")]
    [TemplateVisualState(GroupName = "ActiveStates", Name = "Inactive")]
    public partial class ProgressRing : Control
    {
        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChanged)));

        // Using a DependencyProperty as the backing store for TemplateSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register("TemplateSettings", typeof(TemplateSettingValues), typeof(ProgressRing), new PropertyMetadata(null));

        private bool hasAppliedTemplate = false;

        public ProgressRing()
        {
            DefaultStyleKey = typeof(ProgressRing);
            TemplateSettings = new TemplateSettingValues(60);
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public TemplateSettingValues TemplateSettings
        {
            get { return (TemplateSettingValues)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            hasAppliedTemplate = true;
            UpdateState(IsActive);
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            var width = 20d;
            var height = 20d;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                width = double.IsNaN(Width) == false ? Width : availableSize.Width;
                height = double.IsNaN(Height) == false ? Height : availableSize.Height;
            }

            TemplateSettings = new TemplateSettingValues(Math.Min(width, height));
            return base.MeasureOverride(availableSize);
        }

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var pr = (ProgressRing)d;
            var isActive = (bool)args.NewValue;
            pr.UpdateState(isActive);
        }

        private void UpdateState(bool isActive)
        {
            if (hasAppliedTemplate)
            {
                string state = isActive ? "Active" : "Inactive";
                VisualStateManager.GoToState(this, state, true);
            }
        }
    }

    public class TemplateSettingValues : System.Windows.DependencyObject
    {
        // Using a DependencyProperty as the backing store for MaxSideLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxSideLengthProperty =
            DependencyProperty.Register("MaxSideLength", typeof(double), typeof(TemplateSettingValues), new PropertyMetadata(0D));

        // Using a DependencyProperty as the backing store for EllipseDiameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EllipseDiameterProperty =
            DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(TemplateSettingValues), new PropertyMetadata(0D));

        // Using a DependencyProperty as the backing store for EllipseOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EllipseOffsetProperty =
            DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(TemplateSettingValues), new PropertyMetadata(default(Thickness)));

        public TemplateSettingValues(double width)
        {
            MaxSideLength = 400;

            if (width <= 40)
            {
                EllipseDiameter = (width / 10) + 1;
            }
            else
            {
                EllipseDiameter = width / 10;
            }

            EllipseOffset = new System.Windows.Thickness(0, EllipseDiameter * 2.5, 0, 0);
        }

        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            set { SetValue(MaxSideLengthProperty, value); }
        }

        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }

        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            set { SetValue(EllipseOffsetProperty, value); }
        }
    }
}