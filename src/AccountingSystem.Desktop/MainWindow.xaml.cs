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
            scroll.Content = stack;
            return scroll;
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

            var checkoutBtn = new Button { Content = "💳 إتمام البيع وطباعة الفاتورة الحرارية", Background = new BrushConverter().ConvertFromString("#16A34A") as Brush, Foreground = Brushes.White, FontWeight = FontWeights.Bold, Padding = new Thickness(10, 12, 10, 12), Cursor = System.Windows.Input.Cursors.Hand };
            checkoutBtn.Click += (s, ev) => MessageBox.Show("تم إتمام عملية البيع بنجاح!\n• تم خصم الكميات من المخزون\n• تم تسجيل القيد المحاسبي\n• تم إرسال الفاتورة للطابعة الحرارية", "نجاح العملية", MessageBoxButton.OK, MessageBoxImage.Information);
            rightStack.Children.Add(checkoutBtn);

            rightBorder.Child = rightStack;
            grid.Children.Add(rightBorder);

            return grid;
        }

        private UIElement CreateInventoryView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "إدارة المخازن والأصناف وجرد المعدات", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });

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
            stack.Children.Add(new TextBlock { Text = "• تحويل أصناف شبكات من المستودع الرئيسي إلى فرع المعرض.\n• تسجيل وتسوية مرتجعات المبيعات والمشتريات بضمان الجودة.", FontSize = 14, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush, Margin = new Thickness(0, 0, 0, 20) });

            var btn = new Button { Content = "+ إنشاء سند تحويل مخزني جديد", Width = 220, HorizontalAlignment = HorizontalAlignment.Right };
            btn.Click += (s, ev) => MessageBox.Show("فتح نافذة تحويل المخزون...", "تحويل", MessageBoxButton.OK, MessageBoxImage.Information);
            stack.Children.Add(btn);

            return stack;
        }

        private UIElement CreateReportsView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "مركز التقارير المالية والضريبية الشاملة", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(new TextBlock { Text = "• قائمة الدخل والأرباح والخسائر (Income Statement)\n• ميزان المراجعة والأستاذ العام (Trial Balance & Ledger)\n• تقارير مبيعات أصناف الشبكات والضرائب\n• تصدير الفوري للتقارير بصيغة PDF و Excel.", FontSize = 14, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush, Margin = new Thickness(0, 0, 0, 20) });

            var exportStack = new StackPanel { Orientation = Orientation.Horizontal };
            var pdfBtn = new Button { Content = "📄 تصدير تقرير الأرباح PDF", Width = 200, Margin = new Thickness(0, 0, 10, 0) };
            pdfBtn.Click += (s, ev) => MessageBox.Show("تم تصدير التقرير المالي بصيغة PDF بنجاح!", "تصدير", MessageBoxButton.OK, MessageBoxImage.Information);

            var excelBtn = new Button { Content = "📊 تصدير البيانات إلى Excel", Width = 200 };
            excelBtn.Click += (s, ev) => MessageBox.Show("تم تصدير البيانات إلى ملف Excel بنجاح!", "تصدير", MessageBoxButton.OK, MessageBoxImage.Information);

            exportStack.Children.Add(pdfBtn);
            exportStack.Children.Add(excelBtn);
            stack.Children.Add(exportStack);

            return stack;
        }

        private UIElement CreateSettingsView()
        {
            var stack = new StackPanel();
            stack.Children.Add(new TextBlock { Text = "إعدادات النظام، الطابعة الحرارية، والربط السحابي", FontSize = 15, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 12) });
            stack.Children.Add(new TextBlock { Text = "• عنوان خادم الباك إند: https://accounting-system-backend-production-97e3.up.railway.app\n• الطابعة الحرارية: POS-80 Thermal Printer (متصلة)\n• وضع العمل: متصل بالانترنت مع دعم العمل دون اتصال (Offline-First)\n• النسخ الاحتياطي: تلقائي يومي", FontSize = 14, Foreground = new BrushConverter().ConvertFromString("#475569") as Brush, Margin = new Thickness(0, 0, 0, 20) });

            return stack;
        }
    }
}
