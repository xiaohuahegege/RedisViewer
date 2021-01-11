using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Button = System.Windows.Controls.Button;
using RadioButton = System.Windows.Controls.RadioButton;

namespace RedisViewer.UI.Controls
{
    public class FunctionEventArgs<T> : RoutedEventArgs
    {
        public FunctionEventArgs(T info)
        {
            Info = info;
        }

        public FunctionEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }

        public T Info { get; set; }
    }

    public class Pagination : Control
    {
        private const string ElementButtonLeft = "PART_ButtonLeft";
        private const string ElementButtonRight = "PART_ButtonRight";
        private const string ElementButtonFirst = "PART_ButtonFirst";
        private const string ElementMoreLeft = "PART_MoreLeft";
        private const string ElementPanelMain = "PART_PanelMain";
        private const string ElementMoreRight = "PART_MoreRight";
        private const string ElementButtonLast = "PART_ButtonLast";

        private PaginationButton _buttonLeft;
        private PaginationButton _buttonRight;
        private PaginationRadioButton _buttonFirst;
        private FrameworkElement _moreLeft;
        private Panel _panelMain;
        private FrameworkElement _moreRight;
        private PaginationRadioButton _buttonLast;

        private bool _appliedTemplate;

        #region Public Events

        /// <summary>
        ///     页面更新事件
        /// </summary>
        public static readonly RoutedEvent PageUpdatedEvent =
            EventManager.RegisterRoutedEvent("PageUpdated", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<int>>), typeof(Pagination));

        /// <summary>
        ///     页面更新事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<int>> PageUpdated
        {
            add => AddHandler(PageUpdatedEvent, value);
            remove => RemoveHandler(PageUpdatedEvent, value);
        }

        #endregion Public Events

        public Pagination()
        {
            CommandBindings.Add(new CommandBinding(PaginationCommands.Prev, ButtonPrev_OnClick));
            CommandBindings.Add(new CommandBinding(PaginationCommands.Next, ButtonNext_OnClick));
            CommandBindings.Add(new CommandBinding(PaginationCommands.Selected, ToggleButton_OnChecked));
            
         //   Visibility = MaxPageCount > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        static Pagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(typeof(Pagination)));
        }

        #region Public Properties

        #region MaxPageCount

        /// <summary>
        ///     最大页数
        /// </summary>
        public static readonly DependencyProperty MaxPageCountProperty = DependencyProperty.Register(
            "MaxPageCount", typeof(int), typeof(Pagination), new PropertyMetadata(1, (o, args) =>
            {
                if (o is Pagination pagination && args.NewValue is int value)
                {
                    if (pagination.PageIndex > pagination.MaxPageCount)
                    {
                        pagination.PageIndex = pagination.MaxPageCount;
                    }

                    pagination.Visibility = value > 1 ? Visibility.Visible : Visibility.Collapsed;
                    pagination.Update();
                }
            }, (o, value) =>
            {
                if (!(o is Pagination)) return 1;
                var intValue = (int)value;
                if (intValue < 1)
                {
                    return 1;
                }
                return intValue;
            }));

        /// <summary>
        ///     最大页数
        /// </summary>
        public int MaxPageCount
        {
            get => (int)GetValue(MaxPageCountProperty);
            set => SetValue(MaxPageCountProperty, value);
        }

        #endregion MaxPageCount

        #region DataCountPerPage

        /// <summary>
        ///     每页的数据量
        /// </summary>
        public static readonly DependencyProperty DataCountPerPageProperty = DependencyProperty.Register(
            "DataCountPerPage", typeof(int), typeof(Pagination), new PropertyMetadata(20, (o, args) =>
            {
                if (o is Pagination pagination)
                {
                    pagination.Update();
                }
            }, (o, value) =>
            {
                if (!(o is Pagination)) return 1;
                var intValue = (int)value;
                if (intValue < 1)
                {
                    return 1;
                }
                return intValue;
            }));

        /// <summary>
        ///     每页的数据量
        /// </summary>
        public int DataCountPerPage
        {
            get => (int)GetValue(DataCountPerPageProperty);
            set => SetValue(DataCountPerPageProperty, value);
        }

        #endregion

        #region PageIndex

        /// <summary>
        ///     当前页
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(Pagination), new PropertyMetadata(1, (o, args) =>
            {
                if (o is Pagination pagination && args.NewValue is int value)
                {
                    pagination.Update();
                    pagination.RaiseEvent(new FunctionEventArgs<int>(PageUpdatedEvent, pagination)
                    {
                        Info = value
                    });
                }
            }, (o, value) =>
            {
                if (!(o is Pagination pagination)) return 1;
                var intValue = (int)value;
                if (intValue < 0)
                {
                    return 0;
                }
                if (intValue > pagination.MaxPageCount) return pagination.MaxPageCount;
                return intValue;
            }));

        /// <summary>
        ///     当前页
        /// </summary>
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        #endregion PageIndex

        #region MaxPageInterval

        /// <summary>
        ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
        /// </summary>       
        public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(
            "MaxPageInterval", typeof(int), typeof(Pagination), new PropertyMetadata(3, (o, args) =>
            {
                if (o is Pagination pagination)
                {
                    pagination.Update();
                }
            }), value =>
            {
                var intValue = (int)value;
                return intValue >= 0;
            });

        /// <summary>
        ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
        /// </summary>   
        public int MaxPageInterval
        {
            get => (int)GetValue(MaxPageIntervalProperty);
            set => SetValue(MaxPageIntervalProperty, value);
        }

        #endregion MaxPageInterval

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            _appliedTemplate = false;
            base.OnApplyTemplate();

            _buttonLeft = GetTemplateChild(ElementButtonLeft) as PaginationButton;
            _buttonRight = GetTemplateChild(ElementButtonRight) as PaginationButton;
            _buttonFirst = GetTemplateChild(ElementButtonFirst) as PaginationRadioButton;
            _moreLeft = GetTemplateChild(ElementMoreLeft) as FrameworkElement;
            _panelMain = GetTemplateChild(ElementPanelMain) as Panel;
            _moreRight = GetTemplateChild(ElementMoreRight) as FrameworkElement;
            _buttonLast = GetTemplateChild(ElementButtonLast) as PaginationRadioButton;

            CheckNull();

            _appliedTemplate = true;
            Update();
        }

        #endregion Public Methods

        #region Private Methods

        private void CheckNull()
        {
            if (_buttonLeft == null || _buttonRight == null || _buttonFirst == null ||
                _moreLeft == null || _panelMain == null || _moreRight == null ||
                _buttonLast == null) throw new Exception();
        }

        /// <summary>
        ///     更新
        /// </summary>
        private void Update()
        {
            if (!_appliedTemplate) return;
            _buttonLeft.IsEnabled = PageIndex > 1;
            _buttonRight.IsEnabled = PageIndex < MaxPageCount;
            if (MaxPageInterval == 0)
            {
                _buttonFirst.Visibility = _buttonLast.Visibility = _moreLeft.Visibility = _moreRight.Visibility = Visibility.Collapsed;
                _panelMain.Children.Clear();
                var selectButton = CreateButton(PageIndex);
                _panelMain.Children.Add(selectButton);
                selectButton.IsChecked = true;
                return;
            }
            _buttonFirst.Visibility = _buttonLast.Visibility = _moreLeft.Visibility = _moreRight.Visibility = Visibility.Visible;

            //更新最后一页
            if (MaxPageCount == 1)
            {
                _buttonLast.Visibility = Visibility.Collapsed;
            }
            else
            {
                _buttonLast.Visibility = Visibility.Visible;
                _buttonLast.Content = MaxPageCount.ToString();
            }

            //更新省略号
            var right = MaxPageCount - PageIndex;
            var left = PageIndex - 1;
            
            _moreRight.Visibility = right > MaxPageInterval ? Visibility.Visible : Visibility.Collapsed;
            _moreLeft.Visibility = left > MaxPageInterval ? Visibility.Visible : Visibility.Collapsed;

            //更新中间部分
            _panelMain.Children.Clear();
            if (PageIndex > 1 && PageIndex < MaxPageCount)
            {
                var selectButton = CreateButton(PageIndex);
                _panelMain.Children.Add(selectButton);
                selectButton.IsChecked = true;
                _buttonFirst.IsChecked = _buttonLast.IsChecked = false;
            }
            else if (PageIndex == 1)
            {
                _buttonFirst.IsChecked = true;
            }
            else
            {
                _buttonLast.IsChecked = true;
            }

            var sub = PageIndex;
            for (var i = 0; i < MaxPageInterval - 1; i++)
            {
                if (--sub > 1)
                {
                    _panelMain.Children.Insert(0, CreateButton(sub));
                }
                else
                {
                    break;
                }
            }
            var add = PageIndex;
            for (var i = 0; i < MaxPageInterval - 1; i++)
            {
                if (++add < MaxPageCount)
                {
                    _panelMain.Children.Add(CreateButton(add));
                }
                else
                {
                    break;
                }
            }
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => PageIndex--;

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => PageIndex++;

        private PaginationRadioButton CreateButton(int page)
        {
            return new PaginationRadioButton
            {
                //  Style = ResourceHelper.GetResource<Style>(ResourceToken.PaginationButtonStyle),
  
                Content = page.ToString()
            };
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is RadioButton button)) return;
            if (button.IsChecked == false) return;
            PageIndex = int.Parse(button.Content.ToString());
        }

        #endregion Private Methods       
    }
}