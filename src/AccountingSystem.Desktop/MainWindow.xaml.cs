using System.Windows;
using System.Windows.Controls;

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
            PageTitleText.Text = "نقطة البيع (POS Terminal)";
            ContentContainer.Child = CreatePOSView();
        }

        private void NavInventory_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "إدارة المخازن والأصناف (Inventory Ledger)";
            ContentContainer.Child = CreateInventoryView();
        }

        private void NavCustomers_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "إدارة العملاء والموردين (Accounts Ledger)";
            ContentContainer.Child = CreateCustomersView();
        }

        private void NavVouchers_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "السندات والقيود المالية (Vouchers & Journal Entries)";
            ContentContainer.Child = CreateVouchersView();
        }

        private void NavTransfers_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "تحويل المخزون والمرتجعات (Stock Transfers & Adjustments)";
            ContentContainer.Child = CreateTransfersView();
        }

        private void NavReports_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "التقارير المالية والضريبية (Financial & Tax Reports)";
            ContentContainer.Child = CreateReportsView();
        }

        private void NavSettings_Click(object sender, RoutedEventArgs e)
        {
            PageTitleText.Text = "إعدادات النظام والأجهزة (System & Hardware Settings)";
            ContentContainer.Child = CreateSettingsView();
        }

        private UIElement CreateDashboardView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "لوحة التحكم التنفيذية - إحصائيات حية لقطع الشبكات", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• صافي الإيرادات اليومية: 3,450.00 $\n• عدد الفواتير: 32 فاتورة\n• أصناف منخفضة المخزون: 4 أصناف\n• الأصول النقدية والبنكية: 24,850.00 $", FontSize = 14 });
            return panel;
        }

        private UIElement CreatePOSView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "نقطة البيع السريعة (POS Terminal - شبكات وإلكترونيات)", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• اختر الصنف (راوتر ميكروتيك، سويتش تي بي لينك، كابلات CAT6)\n• تحديد الكمية وإضافتها لسلة الفاتورة\n• طباعة الفاتورة عبر الطابعة الحرارية مباشرة", FontSize = 14 });
            return panel;
        }

        private UIElement CreateInventoryView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "إدارة المخازن والأصناف (Inventory Ledger)", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• راوتر ميكروتيك RB951UiAS | SKU-RB951 | المخزون: 14 | السعر: 55.00 $\n• كاميرا مراقبة داهوا 4MP | SKU-CAM4 | المخزون: 3 (منخفض) | السعر: 35.00 $\n• سويتش تي بي لينك 16 بورت | SKU-SW16 | المخزون: 22 | السعر: 85.00 $", FontSize = 14 });
            return panel;
        }

        private UIElement CreateCustomersView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "إدارة العملاء والموردين والذمم المالية", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• شركة التميز لتقنية المعلومات | رصيد مدين: 1,250.00 $\n• مؤسسة الأفق للشبكات | رصيد مدين: 450.00 $\n• شركة ميكروتيك العالمية | رصيد دائن: -3,400.00 $", FontSize = 14 });
            return panel;
        }

        private UIElement CreateVouchersView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "سندات القبض والصرف والقيود المزدوجة", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• سند قبض رقم REC-501 بمبلغ 450.00 $\n• سند صرف رقم PAY-301 بمبلغ 250.00 $\n• قيد تسوية مخزون رقم JRN-101", FontSize = 14 });
            return panel;
        }

        private UIElement CreateTransfersView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "تحويل المخزون بين المستودعات والمرتجعات", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• تحويل من المستودع الرئيسي إلى فرع المعرض (TRF-1002)\n• مرتجع مبيعات أجهزة شبكات (ADJ-504)", FontSize = 14 });
            return panel;
        }

        private UIElement CreateReportsView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "التقارير المالية والضريبية الشاملة", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• قائمة الدخل والأرباح والخسائر (تصدير PDF / Excel)\n• ميزان المراجعة والأستاذ العام\n• تقارير مبيعات نقاط البيع والضرائب", FontSize = 14 });
            return panel;
        }

        private UIElement CreateSettingsView()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "إعدادات النظام والربط السحابي والأجهزة", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });
            panel.Children.Add(new TextBlock { Text = "• رابط الباك إند: https://accounting-system-backend-production-97e3.up.railway.app\n• الطابعة الحرارية: POS-80 (USB / Bluetooth)\n• حالة المزامنة: مفعلة في الخلفية", FontSize = 14 });
            return panel;
        }
    }
}
