using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace AccountingSystem.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavDashboard_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "لوحة المؤشرات التنفيذية (Executive Dashboard)";
            ContentContainer.Child = CreateDashboardView();
        }

        private void NavPOS_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "نقطة البيع السريعة (POS Terminal - أجهزة الشبكات)";
            ContentContainer.Child = CreatePOSView();
        }

        private void NavInventory_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "إدارة المخازن والأصناف (Inventory Ledger)";
            ContentContainer.Child = CreateInventoryView();
        }

        private void NavCustomers_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "إدارة العملاء والموردين والذمم المالية (Accounts Ledger)";
            ContentContainer.Child = CreateCustomersView();
        }

        private void NavVouchers_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "السندات المالية والقيود اليومية (Vouchers & Journal Entries)";
            ContentContainer.Child = CreateVouchersView();
        }

        private void NavTransfers_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "تحويل المخزون والمرتجعات (Stock Transfers & Adjustments)";
            ContentContainer.Child = CreateTransfersView();
        }

        private void NavReports_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "التقارير المالية والضريبية الشاملة (Reports Center)";
            ContentContainer.Child = CreateReportsView();
        }

        private void NavSettings_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "إعدادات النظام والربط السحابي (System & Cloud Settings)";
            ContentContainer.Child = CreateSettingsView();
        }

        private void SyncNow_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("تمت مزامنة البيانات بنجاح مع السحابة (Railway Cloud Backend).\nجميع الحركات والعمليات محدثة.", "مزامنة سحابية", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private UIElement CreateDashboardView()
        {
            var scroll = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            var stack = new StackPanel();

            stack.Children.Add(new TextBlock { Text = "نظرة عامة على أداء معارض ومستودعات الشبكات والإلكترونيات", FontSize = 16, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString("#1E293B") as Brush, Margin = new Thickness(0, 0, 0, 4) });
            stack.Children.Add(new TextBlock { Text = "تحديث لحظي لعمليات المبيعات، المخزون الحرج، وحركة الذمم المالية.", FontSize = 13, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 0, 0, 20) });

            // KPI Cards Grid
            var kpiGrid = new UniformGrid { Columns = 4, Margin = new Thickness(0, 0, 0, 24) };
            kpiGrid.Children.Add(CreateKpiCard("إجمالي مبيعات اليوم", "4,850.00 $", "↑ 18% مقارنة بالأمس", "#16A34A"));
            kpiGrid.Children.Add(CreateKpiCard("أصناف منخفضة المخزون", "3 أصناف", "تحتاج إعادة طلب عاجلة", "#DC2626"));
            kpiGrid.Children.Add(CreateKpiCard("أرصدة العملاء (مديونيات)", "5,230.00 $", "الذمم المدينة المستحقة", "#7C3AED"));
            kpiGrid.Children.Add(CreateKpiCard("النقدية في الصندوق والبنك", "32,400.00 $", "السيولة المتاحة للتشغيل", "#2563EB"));
            stack.Children.Add(kpiGrid);

            stack.Children.Add(new TextBlock { Text = "آخر الفواتير المسجلة في النظام", FontSize = 15, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString("#0F172A") as Brush, Margin = new Thickness(0, 0, 0, 12) });

            var dg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 240,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                BorderThickness = new Thickness(1),
                RowHeight = 32
            };

            dg.Columns.Add(new DataGridTextColumn { Header = "رقم الفاتورة", Binding = new Binding("InvoiceNo"), Width = 110 });
            dg.Columns.Add(new DataGridTextColumn { Header = "العميل", Binding = new Binding("CustomerName"), Width = 180 });
            dg.Columns.Add(new DataGridTextColumn { Header = "التاريخ والوقت", Binding = new Binding("DateTime"), Width = 150 });
            dg.Columns.Add(new DataGridTextColumn { Header = "المبلغ الإجمالي", Binding = new Binding("TotalAmount"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "طريقة الدفع", Binding = new Binding("PaymentMethod"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "حالة الدفع", Binding = new Binding("Status"), Width = 110 });
            dg.Columns.Add(new DataGridTextColumn { Header = "المستخدم المسؤول", Binding = new Binding("User"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            dg.ItemsSource = new List<object>
            {
                new { InvoiceNo = "INV-2026-001", CustomerName = "شركة الأفق للاتصالات والتقنية", DateTime = "2026-08-12 10:30", TotalAmount = "850.00 $", PaymentMethod = "نقدي (صندوق)", Status = "مدفوعة", User = "المدير العام" },
                new { InvoiceNo = "INV-2026-002", CustomerName = "مؤسسة الوجيه للشبكات", DateTime = "2026-08-12 11:15", TotalAmount = "1,420.00 $", PaymentMethod = "آجل (ذمم)", Status = "مستحقة", User = "محاسب المبيعات" },
                new { InvoiceNo = "INV-2026-003", CustomerName = "مهندس / أحمد علي", DateTime = "2026-08-12 12:00", TotalAmount = "320.00 $", PaymentMethod = "تحويل بنكي", Status = "مدفوعة", User = "المدير العام" },
                new { InvoiceNo = "INV-2026-004", CustomerName = "شبكات السعيد المنزلية", DateTime = "2026-08-12 13:45", TotalAmount = "640.00 $", PaymentMethod = "نقدي (صندوق)", Status = "مدفوعة", User = "محاسب المبيعات" }
            };

            stack.Children.Add(dg);
            stack.Children.Add(CreateDashboardOperationsPanel());
            scroll.Content = stack;
            return scroll;
        }

        private Grid CreateDashboardOperationsPanel()
        {
            var panel = new Grid { Margin = new Thickness(0, 20, 0, 0) };
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var trendBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 12, 0)
            };
            var trendStack = new StackPanel();
            trendStack.Children.Add(new TextBlock { Text = "اتجاه المبيعات خلال آخر 7 أيام", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString("#0F172A") as Brush });
            trendStack.Children.Add(new TextBlock { Text = "مؤشر تشغيلي للمتابعة اليومية قبل فتح التقارير التفصيلية", FontSize = 11, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 3, 0, 14) });

            var bars = new UniformGrid { Columns = 7, Height = 150, VerticalAlignment = VerticalAlignment.Bottom };
            var dayLabels = new[] { "السبت", "الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "اليوم" };
            var barHeights = new[] { 64.0, 92.0, 78.0, 112.0, 86.0, 128.0, 145.0 };
            for (var i = 0; i < dayLabels.Length; i++)
            {
                var barStack = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Center };
                barStack.Children.Add(new Border
                {
                    Background = new BrushConverter().ConvertFromString(i == dayLabels.Length - 1 ? "#2563EB" : "#93C5FD") as Brush,
                    Width = 28,
                    Height = barHeights[i],
                    CornerRadius = new CornerRadius(5, 5, 2, 2),
                    Margin = new Thickness(4, 0, 4, 6)
                });
                barStack.Children.Add(new TextBlock { Text = dayLabels[i], FontSize = 10, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, HorizontalAlignment = HorizontalAlignment.Center });
                bars.Children.Add(barStack);
            }
            trendStack.Children.Add(bars);
            trendBorder.Child = trendStack;
            panel.Children.Add(trendBorder);

            var operationsBorder = new Border
            {
                Background = new BrushConverter().ConvertFromString("#F8FAFC") as Brush,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16)
            };
            Grid.SetColumn(operationsBorder, 1);

            var operationsStack = new StackPanel();
            operationsStack.Children.Add(new TextBlock { Text = "تنبيهات وإجراءات تشغيلية", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString("#0F172A") as Brush });
            operationsStack.Children.Add(CreateAlertRow("3 أصناف وصلت إلى الحد الأدنى", "المخزون", "#DC2626"));
            operationsStack.Children.Add(CreateAlertRow("5,230.00 $ أرصدة مستحقة", "الذمم المدينة", "#7C3AED"));
            operationsStack.Children.Add(CreateAlertRow("آخر مزامنة منذ دقيقتين", "الحالة السحابية", "#16A34A"));
            operationsStack.Children.Add(new Separator { Margin = new Thickness(0, 10, 0, 10), Background = new BrushConverter().ConvertFromString("#CBD5E1") as Brush });

            var quickActions = new UniformGrid { Columns = 2 };
            var posButton = new Button { Content = "فتح نقطة البيع", Margin = new Thickness(0, 0, 6, 6), Padding = new Thickness(8), FontSize = 11 };
            posButton.Click += NavPOS_Click;
            var stockButton = new Button { Content = "فحص المخزون", Margin = new Thickness(6, 0, 0, 6), Padding = new Thickness(8), FontSize = 11 };
            stockButton.Click += NavInventory_Click;
            var reportButton = new Button { Content = "مركز التقارير", Margin = new Thickness(0, 6, 6, 0), Padding = new Thickness(8), FontSize = 11 };
            reportButton.Click += NavReports_Click;
            var voucherButton = new Button { Content = "سند قبض / صرف", Margin = new Thickness(6, 6, 0, 0), Padding = new Thickness(8), FontSize = 11 };
            voucherButton.Click += NavVouchers_Click;
            quickActions.Children.Add(posButton);
            quickActions.Children.Add(stockButton);
            quickActions.Children.Add(reportButton);
            quickActions.Children.Add(voucherButton);
            operationsStack.Children.Add(quickActions);
            operationsBorder.Child = operationsStack;
            panel.Children.Add(operationsBorder);

            return panel;
        }

        private Border CreateAlertRow(string message, string category, string colorHex)
        {
            var row = new Border { Background = Brushes.White, BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(6), Padding = new Thickness(9), Margin = new Thickness(0, 10, 0, 0) };
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = category, FontSize = 10, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString(colorHex) as Brush });
            stack.Children.Add(new TextBlock { Text = message, FontSize = 11, Foreground = new BrushConverter().ConvertFromString("#334155") as Brush, Margin = new Thickness(0, 2, 0, 0) });
            row.Child = stack;
            return row;
        }

        private Grid CreateListToolbar(string searchHint, string filterLabel, string primaryActionLabel, string exportLabel)
        {
            var toolbar = new Grid { Margin = new Thickness(0, 0, 0, 14) };
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(130) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(130) });

            var search = new TextBox { ToolTip = searchHint, Padding = new Thickness(9), Margin = new Thickness(0, 0, 8, 0) };
            Grid.SetColumn(search, 0);
            toolbar.Children.Add(search);

            var filter = new ComboBox { ToolTip = "اختر نطاق التصفية", Padding = new Thickness(6), Margin = new Thickness(0, 0, 8, 0) };
            filter.Items.Add(new ComboBoxItem { Content = filterLabel, IsSelected = true });
            filter.Items.Add(new ComboBoxItem { Content = "الحركات النشطة" });
            filter.Items.Add(new ComboBoxItem { Content = "الحركات المؤرشفة" });
            Grid.SetColumn(filter, 1);
            toolbar.Children.Add(filter);

            var primary = new Button { Content = primaryActionLabel, Padding = new Thickness(8, 7, 8, 7), Margin = new Thickness(0, 0, 8, 0), FontSize = 11 };
            primary.Click += (s, e) => MessageBox.Show($"فتح نافذة {primaryActionLabel}...", "إجراء جديد", MessageBoxButton.OK, MessageBoxImage.Information);
            Grid.SetColumn(primary, 2);
            toolbar.Children.Add(primary);

            var export = new Button { Content = exportLabel, Padding = new Thickness(8, 7, 8, 7), FontSize = 11 };
            export.Click += (s, e) => MessageBox.Show($"تم تجهيز {exportLabel} بصيغة Excel/PDF.", "تصدير البيانات", MessageBoxButton.OK, MessageBoxImage.Information);
            Grid.SetColumn(export, 3);
            toolbar.Children.Add(export);

            return toolbar;
        }

        private Border CreateKpiCard(string title, string value, string subtitle, string colorHex)
        {
            var border = new Border
            {
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 12, 0)
            };

            var sp = new StackPanel();
            sp.Children.Add(new TextBlock { Text = title, FontSize = 12, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, FontWeight = FontWeights.SemiBold });
            sp.Children.Add(new TextBlock { Text = value, FontSize = 22, FontWeight = FontWeights.ExtraBold, Foreground = new BrushConverter().ConvertFromString(colorHex) as Brush, Margin = new Thickness(0, 6, 0, 0) });
            sp.Children.Add(new TextBlock { Text = subtitle, FontSize = 11, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 4, 0, 0) });

            border.Child = sp;
            return border;
        }

        private UIElement CreatePOSView()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(380) });

            // Left: Products Catalog
            var leftStack = new StackPanel { Margin = new Thickness(0, 0, 16, 0) };
            leftStack.Children.Add(new TextBlock { Text = "اختيار أصناف الشبكات والإلكترونيات للبيع السريع", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 10) });

            var searchBox = new TextBox { Text = "بحث بالاسم، الباركود، أو SKU...", Padding = new Thickness(8), Margin = new Thickness(0, 0, 0, 10) };
            leftStack.Children.Add(searchBox);

            var posDg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 450,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                RowHeight = 35
            };
            posDg.Columns.Add(new DataGridTextColumn { Header = "كود الصنف (SKU)", Binding = new Binding("SKU"), Width = 110 });
            posDg.Columns.Add(new DataGridTextColumn { Header = "اسم الصنف", Binding = new Binding("Name"), Width = 220 });
            posDg.Columns.Add(new DataGridTextColumn { Header = "المستودع", Binding = new Binding("Warehouse"), Width = 110 });
            posDg.Columns.Add(new DataGridTextColumn { Header = "المخزون المتوفر", Binding = new Binding("Stock"), Width = 100 });
            posDg.Columns.Add(new DataGridTextColumn { Header = "سعر البيع ($)", Binding = new Binding("Price"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            posDg.ItemsSource = new List<object>
            {
                new { SKU = "SKU-RB951", Name = "راوتر ميكروتيك RB951UiAS-2HnD", Warehouse = "الرئيسي", Stock = "14", Price = "65.00" },
                new { SKU = "SKU-CC1009", Name = "راوتر ميكروتيك Cloud Core CC1009", Warehouse = "الرئيسي", Stock = "5", Price = "380.00" },
                new { SKU = "SKU-SW16", Name = "سويتش تي بي لينك 16 بورت جيجابت", Warehouse = "فرع المعرض", Stock = "22", Price = "85.00" },
                new { SKU = "SKU-CAT6", Name = "لفة كابل شبكات CAT6 أصلية (305م)", Warehouse = "الرئيسي", Stock = "45", Price = "110.00" },
                new { SKU = "SKU-CAM4", Name = "كاميرا مراقبة داهوا IP 4MP خارجية", Warehouse = "فرع المعرض", Stock = "8", Price = "95.00" },
                new { SKU = "SKU-RJ45", Name = "رأس موصل كابل RJ45 (علبة 100 حبة)", Warehouse = "الرئيسي", Stock = "60", Price = "12.00" }
            };
            leftStack.Children.Add(posDg);
            grid.Children.Add(leftStack);

            // Right: Invoice Cart & Checkout
            var rightBorder = new Border
            {
                Background = new BrushConverter().ConvertFromString("#F8FAFC") as Brush,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16)
            };

            Grid.SetColumn(rightBorder, 1);

            var rightStack = new StackPanel();
            rightStack.Children.Add(new TextBlock { Text = "سلة الفاتورة الحالية (POS Cart)", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });

            var cartDg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 200,
                Background = Brushes.White,
                RowHeight = 28
            };
            cartDg.Columns.Add(new DataGridTextColumn { Header = "الصنف", Binding = new Binding("Item"), Width = 140 });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "الكمية", Binding = new Binding("Qty"), Width = 50 });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "السعر", Binding = new Binding("Price"), Width = 65 });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "الإجمالي", Binding = new Binding("Total"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            cartDg.ItemsSource = new List<object>
            {
                new { Item = "راوتر ميكروتيك RB951", Qty = "2", Price = "65.00", Total = "130.00 $" },
                new { Item = "لفة كابل CAT6", Qty = "1", Price = "110.00", Total = "110.00 $" }
            };
            rightStack.Children.Add(cartDg);

            // Totals and Checkout Buttons
            var totalsStack = new StackPanel { Margin = new Thickness(0, 15, 0, 15) };
            totalsStack.Children.Add(new TextBlock { Text = "المجموع الفرعي: 240.00 $", FontSize = 13, FontWeight = FontWeights.SemiBold });
            totalsStack.Children.Add(new TextBlock { Text = "ضريبة القيمة المضافة (0%): 0.00 $", FontSize = 13, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 4, 0, 0) });
            totalsStack.Children.Add(new TextBlock { Text = "الإجمالي النهائي: 240.00 $", FontSize = 16, FontWeight = FontWeights.ExtraBold, Foreground = new BrushConverter().ConvertFromString("#2563EB") as Brush, Margin = new Thickness(0, 8, 0, 0) });
            rightStack.Children.Add(totalsStack);

            var invoiceActions = new UniformGrid { Columns = 2, Margin = new Thickness(0, 0, 0, 0) };
            var previewBtn = new Button { Content = "👁 معاينة قبل الطباعة", Padding = new Thickness(8, 10, 8, 10), Margin = new Thickness(0, 0, 6, 0), FontSize = 11 };
            previewBtn.Click += (s, ev) => ShowInvoicePrintPreview();
            invoiceActions.Children.Add(previewBtn);

            var checkoutBtn = new Button { Content = "💳 إتمام البيع", Background = new BrushConverter().ConvertFromString("#16A34A") as Brush, Foreground = Brushes.White, FontWeight = FontWeights.Bold, Padding = new Thickness(10, 10, 10, 10), Cursor = System.Windows.Input.Cursors.Hand };
            checkoutBtn.Click += (s, ev) => MessageBox.Show("تم إتمام عملية البيع بنجاح!\n• تم خصم الكميات من المخزون\n• تم تسجيل القيد المحاسبي\n• أصبحت الفاتورة جاهزة للطباعة أو المشاركة", "نجاح العملية", MessageBoxButton.OK, MessageBoxImage.Information);
            invoiceActions.Children.Add(checkoutBtn);
            rightStack.Children.Add(invoiceActions);

            rightBorder.Child = rightStack;
            grid.Children.Add(rightBorder);

            return grid;
        }

        private void ShowInvoicePrintPreview()
        {
            var previewWindow = new Window
            {
                Title = "معاينة الفاتورة قبل الطباعة - INV-2026-005",
                Width = 840,
                Height = 650,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                FlowDirection = FlowDirection.RightToLeft,
                Background = Brushes.White
            };

            var outer = new Grid { Margin = new Thickness(18) };
            outer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            outer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            outer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var settings = new Border { Background = new BrushConverter().ConvertFromString("#F8FAFC") as Brush, BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(8), Padding = new Thickness(12), Margin = new Thickness(0, 0, 0, 12) };
            var settingsRow = new StackPanel { Orientation = Orientation.Horizontal };
            settingsRow.Children.Add(new TextBlock { Text = "الطابعة:", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 8, 0) });
            var printer = new ComboBox { Width = 170, Padding = new Thickness(6), Margin = new Thickness(0, 0, 18, 0) };
            printer.Items.Add(new ComboBoxItem { Content = "POS-80 Thermal Printer", IsSelected = true });
            printer.Items.Add(new ComboBoxItem { Content = "Microsoft Print to PDF" });
            settingsRow.Children.Add(printer);
            settingsRow.Children.Add(new TextBlock { Text = "مقاس الورق:", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 8, 0) });
            var paper = new ComboBox { Width = 100, Padding = new Thickness(6), Margin = new Thickness(0, 0, 18, 0) };
            paper.Items.Add(new ComboBoxItem { Content = "Receipt", IsSelected = true });
            paper.Items.Add(new ComboBoxItem { Content = "A5" });
            paper.Items.Add(new ComboBoxItem { Content = "A4" });
            settingsRow.Children.Add(paper);
            settingsRow.Children.Add(new TextBlock { Text = "النسخ:", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 8, 0) });
            settingsRow.Children.Add(new TextBox { Text = "1", Width = 45, Padding = new Thickness(6) });
            settings.Child = settingsRow;
            Grid.SetRow(settings, 0);
            outer.Children.Add(settings);

            var invoice = new Border { Background = Brushes.White, BorderBrush = new BrushConverter().ConvertFromString("#CBD5E1") as Brush, BorderThickness = new Thickness(1), Padding = new Thickness(28), Margin = new Thickness(50, 0, 50, 12) };
            var invoiceStack = new StackPanel();
            invoiceStack.Children.Add(new TextBlock { Text = "معرض الأفق للشبكات والإلكترونيات", FontSize = 20, FontWeight = FontWeights.ExtraBold, HorizontalAlignment = HorizontalAlignment.Center, Foreground = new BrushConverter().ConvertFromString("#0F172A") as Brush });
            invoiceStack.Children.Add(new TextBlock { Text = "فاتورة مبيعات رقم INV-2026-005  |  12/08/2026 14:20", FontSize = 12, HorizontalAlignment = HorizontalAlignment.Center, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 5, 0, 18) });

            var invoiceGrid = new DataGrid { AutoGenerateColumns = false, CanUserAddRows = false, IsReadOnly = true, Height = 220, RowHeight = 30, HeadersVisibility = DataGridHeadersVisibility.Column };
            invoiceGrid.Columns.Add(new DataGridTextColumn { Header = "الصنف", Binding = new Binding("Item"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            invoiceGrid.Columns.Add(new DataGridTextColumn { Header = "الكمية", Binding = new Binding("Qty"), Width = 70 });
            invoiceGrid.Columns.Add(new DataGridTextColumn { Header = "السعر", Binding = new Binding("Price"), Width = 100 });
            invoiceGrid.Columns.Add(new DataGridTextColumn { Header = "الإجمالي", Binding = new Binding("Total"), Width = 110 });
            invoiceGrid.ItemsSource = new List<object>
            {
                new { Item = "راوتر ميكروتيك RB951UiAS-2HnD", Qty = "2", Price = "65.00 $", Total = "130.00 $" },
                new { Item = "لفة كابل شبكات CAT6 أصلية (305م)", Qty = "1", Price = "110.00 $", Total = "110.00 $" }
            };
            invoiceStack.Children.Add(invoiceGrid);
            invoiceStack.Children.Add(new TextBlock { Text = "الإجمالي النهائي: 240.00 $", FontSize = 18, FontWeight = FontWeights.ExtraBold, HorizontalAlignment = HorizontalAlignment.Left, Foreground = new BrushConverter().ConvertFromString("#2563EB") as Brush, Margin = new Thickness(0, 16, 0, 0) });
            invoice.Child = invoiceStack;
            Grid.SetRow(invoice, 1);
            outer.Children.Add(invoice);

            var footer = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
            var printBtn = new Button { Content = "🖨 طباعة الفاتورة", Padding = new Thickness(18, 9, 18, 9), Margin = new Thickness(8, 0, 0, 0) };
            printBtn.Click += (s, e) => MessageBox.Show("تم إرسال الفاتورة إلى الطابعة المحددة.", "الطباعة", MessageBoxButton.OK, MessageBoxImage.Information);
            var pdfBtn = new Button { Content = "📄 تصدير PDF", Padding = new Thickness(18, 9, 18, 9), Margin = new Thickness(8, 0, 0, 0) };
            pdfBtn.Click += (s, e) => MessageBox.Show("تم تجهيز نسخة PDF من الفاتورة.", "التصدير", MessageBoxButton.OK, MessageBoxImage.Information);
            var closeBtn = new Button { Content = "إغلاق", Padding = new Thickness(18, 9, 18, 9), Margin = new Thickness(8, 0, 0, 0) };
            closeBtn.Click += (s, e) => previewWindow.Close();
            footer.Children.Add(closeBtn);
            footer.Children.Add(pdfBtn);
            footer.Children.Add(printBtn);
            Grid.SetRow(footer, 2);
            outer.Children.Add(footer);

            previewWindow.Content = outer;
            previewWindow.ShowDialog();
        }

        private UIElement CreateInventoryView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "إدارة المخازن والأصناف وجرد المعدات", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(CreateListToolbar("بحث باسم الصنف أو SKU أو الباركود", "كل المستودعات", "إضافة صنف", "تصدير المخزون"));

            var dg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 480,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                RowHeight = 35
            };

            dg.Columns.Add(new DataGridTextColumn { Header = "SKU", Binding = new Binding("SKU"), Width = 110 });
            dg.Columns.Add(new DataGridTextColumn { Header = "اسم الصنف", Binding = new Binding("Name"), Width = 240 });
            dg.Columns.Add(new DataGridTextColumn { Header = "التصنيف", Binding = new Binding("Category"), Width = 140 });
            dg.Columns.Add(new DataGridTextColumn { Header = "المستودع", Binding = new Binding("Warehouse"), Width = 110 });
            dg.Columns.Add(new DataGridTextColumn { Header = "الكمية المتاحة", Binding = new Binding("Stock"), Width = 100 });
            dg.Columns.Add(new DataGridTextColumn { Header = "سعر التكلفة", Binding = new Binding("Cost"), Width = 100 });
            dg.Columns.Add(new DataGridTextColumn { Header = "سعر البيع", Binding = new Binding("Price"), Width = 100 });
            dg.Columns.Add(new DataGridTextColumn { Header = "حالة المخزون", Binding = new Binding("Status"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            dg.ItemsSource = new List<object>
            {
                new { SKU = "SKU-RB951", Name = "راوتر ميكروتيك RB951UiAS", Category = "أجهزة راوتر", Warehouse = "المستودع الرئيسي", Stock = "14", Cost = "45.00 $", Price = "65.00 $", Status = "متوفر" },
                new { SKU = "SKU-SW16", Name = "سويتش تي بي لينك 16 بورت", Category = "سويتشات شبكات", Warehouse = "فرع المعرض", Stock = "22", Cost = "60.00 $", Price = "85.00 $", Status = "متوفر" },
                new { SKU = "SKU-CAM4", Name = "كاميرا مراقبة داهوا 4MP", Category = "كاميرات المراقبة", Warehouse = "فرع المعرض", Stock = "3", Cost = "70.00 $", Price = "95.00 $", Status = "منخفض (إعادة طلب)" },
                new { SKU = "SKU-CAT6", Name = "لفة كابل CAT6 أصلية", Category = "كابلات وملحقات", Warehouse = "المستودع الرئيسي", Stock = "45", Cost = "80.00 $", Price = "110.00 $", Status = "متوفر" }
            };

            stack.Children.Add(dg);
            return stack;
        }

        private UIElement CreateCustomersView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "إدارة العملاء والموردين وحسابات الذمم المدينة والدائنة", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(CreateListToolbar("بحث بالاسم أو كود الحساب أو الهاتف", "العملاء والموردون", "إضافة حساب", "تصدير كشف الحسابات"));

            var dg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 480,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                RowHeight = 35
            };

            dg.Columns.Add(new DataGridTextColumn { Header = "كود الحساب", Binding = new Binding("Code"), Width = 110 });
            dg.Columns.Add(new DataGridTextColumn { Header = "اسم العميل / المورد", Binding = new Binding("Name"), Width = 240 });
            dg.Columns.Add(new DataGridTextColumn { Header = "النوع", Binding = new Binding("Type"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "رقم الهاتف", Binding = new Binding("Phone"), Width = 140 });
            dg.Columns.Add(new DataGridTextColumn { Header = "الرصيد الحالي", Binding = new Binding("Balance"), Width = 130 });
            dg.Columns.Add(new DataGridTextColumn { Header = "طبيعة الرصيد", Binding = new Binding("Nature"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            dg.ItemsSource = new List<object>
            {
                new { Code = "CUS-1001", Name = "شركة الأفق للاتصالات والتقنية", Type = "عميل", Phone = "+964 770 123 4567", Balance = "1,250.00 $", Nature = "مدين (لنا)" },
                new { Code = "CUS-1002", Name = "مؤسسة الوجيه للشبكات", Type = "عميل", Phone = "+964 750 987 6543", Balance = "2,400.00 $", Nature = "مدين (لنا)" },
                new { Code = "SUP-2001", Name = "مجموعة ميكروتيك العالمية للتوريد", Type = "مورد", Phone = "+971 4 555 7890", Balance = "4,100.00 $", Nature = "دائن (علينا)" },
                new { Code = "SUP-2002", Name = "شركة تي بي لينك الشرق الأوسط", Type = "مورد", Phone = "+971 4 333 1122", Balance = "1,850.00 $", Nature = "دائن (علينا)" }
            };

            stack.Children.Add(dg);
            return stack;
        }

        private UIElement CreateVouchersView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "سندات القبض والصرف والقيود المحاسبية اليومية المزدوجة", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(CreateListToolbar("بحث برقم السند أو الطرف أو البيان", "كل أنواع السندات", "سند قبض / صرف", "تصدير السندات"));

            var dg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 480,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                RowHeight = 35
            };

            dg.Columns.Add(new DataGridTextColumn { Header = "رقم السند", Binding = new Binding("No"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "نوع السند", Binding = new Binding("Type"), Width = 140 });
            dg.Columns.Add(new DataGridTextColumn { Header = "الطرف / المستفيد", Binding = new Binding("Party"), Width = 220 });
            dg.Columns.Add(new DataGridTextColumn { Header = "المبلغ", Binding = new Binding("Amount"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "التاريخ", Binding = new Binding("Date"), Width = 140 });
            dg.Columns.Add(new DataGridTextColumn { Header = "البيان", Binding = new Binding("Description"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            dg.ItemsSource = new List<object>
            {
                new { No = "REC-501", Type = "سند قبض نقدي", Party = "شركة الأفق للاتصالات", Amount = "450.00 $", Date = "2026-08-12", Description = "دفعة من حساب الفواتير السابقة" },
                new { No = "PAY-301", Type = "سند صرف بنكي", Party = "مجموعة ميكروتيك العالمية", Amount = "1,200.00 $", Date = "2026-08-11", Description = "سداد جزء من ثمن بضائع مشتراة" },
                new { No = "JRN-101", Type = "قيد يومية تسوية", Party = "تسوية مخزون تالف", Amount = "85.00 $", Date = "2026-08-10", Description = "تلف عابر سرج راوتر نتيجة ماس كهربائي" }
            };

            stack.Children.Add(dg);
            return stack;
        }

        private UIElement CreateTransfersView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "إدارة تحويل المخزون بين المستودعات وتسجيل المرتجعات", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(new TextBlock { Text = "سجل موحد للتحويلات، مرتجعات المبيعات، مرتجعات المشتريات، وتسويات الكميات بين المستودعات.", FontSize = 13, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 0, 0, 14) });
            stack.Children.Add(CreateListToolbar("بحث برقم الحركة أو الصنف أو المستودع", "كل الحركات المخزنية", "إنشاء سند تحويل", "تصدير الحركات"));

            var dg = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 430,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                RowHeight = 35
            };
            dg.Columns.Add(new DataGridTextColumn { Header = "رقم الحركة", Binding = new Binding("Reference"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "النوع", Binding = new Binding("Type"), Width = 150 });
            dg.Columns.Add(new DataGridTextColumn { Header = "الصنف", Binding = new Binding("Product"), Width = 220 });
            dg.Columns.Add(new DataGridTextColumn { Header = "من / إلى", Binding = new Binding("Route"), Width = 180 });
            dg.Columns.Add(new DataGridTextColumn { Header = "الكمية", Binding = new Binding("Quantity"), Width = 80 });
            dg.Columns.Add(new DataGridTextColumn { Header = "التاريخ", Binding = new Binding("Date"), Width = 120 });
            dg.Columns.Add(new DataGridTextColumn { Header = "المستخدم", Binding = new Binding("User"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dg.ItemsSource = new List<object>
            {
                new { Reference = "TRF-1002", Type = "تحويل مخزني", Product = "سويتش TP-Link 16-Port", Route = "الرئيسي ← فرع المعرض", Quantity = "6", Date = "2026-08-12", User = "المدير العام" },
                new { Reference = "SAL-RET-504", Type = "مرتجع مبيعات", Product = "راوتر MikroTik RB951", Route = "العميل ← فرع المعرض", Quantity = "1", Date = "2026-08-11", User = "محاسب المبيعات" },
                new { Reference = "PUR-RET-208", Type = "مرتجع مشتريات", Product = "محول طاقة 24V", Route = "فرع المعرض ← المورد", Quantity = "3", Date = "2026-08-10", User = "أمين المخزن" }
            };
            stack.Children.Add(dg);
            return stack;
        }

        private UIElement CreateReportsView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "مركز التقارير المالية والضريبية الشاملة", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(new TextBlock { Text = "اختر التقرير، حدد الفترة والمستودع، ثم اعرض النتائج أو اطبعها أو صدّرها للمشاركة.", FontSize = 13, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 0, 0, 14) });
            stack.Children.Add(CreateListToolbar("بحث باسم التقرير أو التصنيف", "كل التقارير", "فتح التقرير", "تصدير التقرير"));

            var reportsGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                IsReadOnly = true,
                Height = 300,
                Background = Brushes.White,
                BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush,
                RowHeight = 35
            };
            reportsGrid.Columns.Add(new DataGridTextColumn { Header = "اسم التقرير", Binding = new Binding("Name"), Width = 260 });
            reportsGrid.Columns.Add(new DataGridTextColumn { Header = "التصنيف", Binding = new Binding("Category"), Width = 130 });
            reportsGrid.Columns.Add(new DataGridTextColumn { Header = "الفترة", Binding = new Binding("Period"), Width = 140 });
            reportsGrid.Columns.Add(new DataGridTextColumn { Header = "آخر تحديث", Binding = new Binding("Updated"), Width = 140 });
            reportsGrid.Columns.Add(new DataGridTextColumn { Header = "الحالة", Binding = new Binding("Status"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            reportsGrid.ItemsSource = new List<object>
            {
                new { Name = "قائمة الدخل والأرباح والخسائر", Category = "محاسبة", Period = "هذا الشهر", Updated = "قبل دقيقتين", Status = "جاهز للعرض" },
                new { Name = "تقرير مبيعات أصناف الشبكات", Category = "مبيعات", Period = "آخر 30 يوماً", Updated = "قبل 5 دقائق", Status = "جاهز للعرض" },
                new { Name = "حركة المخزون والأصناف الحرجة", Category = "مخزون", Period = "هذا الشهر", Updated = "قبل 8 دقائق", Status = "يحتاج مراجعة" },
                new { Name = "كشف حساب العملاء والموردين", Category = "ذمم", Period = "حتى اليوم", Updated = "قبل دقيقة", Status = "جاهز للعرض" }
            };
            stack.Children.Add(reportsGrid);

            var exportStack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 14, 0, 0) };
            var previewBtn = new Button { Content = "👁 معاينة التقرير", Width = 170, Margin = new Thickness(0, 0, 10, 0) };
            previewBtn.Click += (s, ev) => MessageBox.Show("تم فتح معاينة التقرير مع الفلاتر والملخص التنفيذي.", "معاينة التقرير", MessageBoxButton.OK, MessageBoxImage.Information);
            var pdfBtn = new Button { Content = "📄 تصدير PDF", Width = 150, Margin = new Thickness(0, 0, 10, 0) };
            pdfBtn.Click += (s, ev) => MessageBox.Show("تم تصدير التقرير المالي بصيغة PDF بنجاح!", "تصدير", MessageBoxButton.OK, MessageBoxImage.Information);
            var excelBtn = new Button { Content = "📊 تصدير Excel", Width = 150 };
            excelBtn.Click += (s, ev) => MessageBox.Show("تم تصدير البيانات إلى ملف Excel بنجاح!", "تصدير", MessageBoxButton.OK, MessageBoxImage.Information);
            exportStack.Children.Add(previewBtn);
            exportStack.Children.Add(pdfBtn);
            exportStack.Children.Add(excelBtn);
            stack.Children.Add(exportStack);

            return stack;
        }

        private UIElement CreateSettingsView()
        {
            var scroll = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "إعدادات النظام، الطابعة الحرارية، والربط السحابي", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 4) });
            stack.Children.Add(new TextBlock { Text = "إدارة الاتصال، المزامنة، أجهزة الطباعة، والنسخ الاحتياطي من مركز واحد.", FontSize = 13, Foreground = new BrushConverter().ConvertFromString("#64748B") as Brush, Margin = new Thickness(0, 0, 0, 16) });

            var statusGrid = new UniformGrid { Columns = 3, Margin = new Thickness(0, 0, 0, 18) };
            statusGrid.Children.Add(CreateKpiCard("حالة الاتصال بالسحابة", "متصل Online", "Railway Backend / API", "#16A34A"));
            statusGrid.Children.Add(CreateKpiCard("حالة المزامنة", "مكتملة", "آخر مزامنة قبل دقيقتين", "#2563EB"));
            statusGrid.Children.Add(CreateKpiCard("الطابعة الحرارية", "متصلة", "POS-80 / Receipt 80mm", "#7C3AED"));
            stack.Children.Add(statusGrid);

            var connectionBorder = new Border { Background = Brushes.White, BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(8), Padding = new Thickness(16), Margin = new Thickness(0, 0, 0, 14) };
            var connectionStack = new StackPanel();
            connectionStack.Children.Add(new TextBlock { Text = "إعدادات الخادم والمزامنة Offline-First", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString("#0F172A") as Brush });
            connectionStack.Children.Add(new TextBlock { Text = "عنوان خادم الباك إند: https://accounting-system-backend-production-97e3.up.railway.app", FontSize = 12, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush, Margin = new Thickness(0, 8, 0, 4) });
            connectionStack.Children.Add(new TextBlock { Text = "الوضع الحالي: Online مع الاحتفاظ بالعمليات المحلية عند انقطاع الاتصال، ثم دفع التغييرات عند عودة الشبكة.", FontSize = 12, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush });
            var syncActions = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 14, 0, 0) };
            var testConnection = new Button { Content = "اختبار الاتصال", Padding = new Thickness(14, 8, 14, 8), Margin = new Thickness(0, 0, 8, 0) };
            testConnection.Click += (s, e) => MessageBox.Show("تم الاتصال بنجاح بخدمة المزامنة السحابية.", "اختبار الاتصال", MessageBoxButton.OK, MessageBoxImage.Information);
            var syncNow = new Button { Content = "مزامنة الآن", Padding = new Thickness(14, 8, 14, 8) };
            syncNow.Click += SyncNow_Click;
            syncActions.Children.Add(testConnection);
            syncActions.Children.Add(syncNow);
            connectionStack.Children.Add(syncActions);
            connectionBorder.Child = connectionStack;
            stack.Children.Add(connectionBorder);

            var hardwareBorder = new Border { Background = Brushes.White, BorderBrush = new BrushConverter().ConvertFromString("#E2E8F0") as Brush, BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(8), Padding = new Thickness(16) };
            var hardwareStack = new StackPanel();
            hardwareStack.Children.Add(new TextBlock { Text = "الأجهزة والنسخ الاحتياطي", FontSize = 14, FontWeight = FontWeights.Bold, Foreground = new BrushConverter().ConvertFromString("#0F172A") as Brush });
            hardwareStack.Children.Add(new TextBlock { Text = "الطابعة: POS-80 Thermal Printer — USB / Bluetooth — جاهزة للطباعة الحرارية.", FontSize = 12, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush, Margin = new Thickness(0, 8, 0, 4) });
            hardwareStack.Children.Add(new TextBlock { Text = "النسخ الاحتياطي: تلقائي يومي مع إمكانية تصدير نسخة يدوية قبل نهاية الوردية.", FontSize = 12, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush });
            var hardwareActions = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 14, 0, 0) };
            var testPrinter = new Button { Content = "اختبار الطابعة", Padding = new Thickness(14, 8, 14, 8), Margin = new Thickness(0, 0, 8, 0) };
            testPrinter.Click += (s, e) => MessageBox.Show("تم إرسال صفحة اختبار إلى POS-80 Thermal Printer.", "اختبار الطابعة", MessageBoxButton.OK, MessageBoxImage.Information);
            var backup = new Button { Content = "إنشاء نسخة احتياطية", Padding = new Thickness(14, 8, 14, 8) };
            backup.Click += (s, e) => MessageBox.Show("تم تجهيز طلب النسخ الاحتياطي المحلي.", "النسخ الاحتياطي", MessageBoxButton.OK, MessageBoxImage.Information);
            hardwareActions.Children.Add(testPrinter);
            hardwareActions.Children.Add(backup);
            hardwareStack.Children.Add(hardwareActions);
            hardwareBorder.Child = hardwareStack;
            stack.Children.Add(hardwareBorder);

            scroll.Content = stack;
            return scroll;
        }
    }
}
