using RestBoy.ViewModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestBoy.Control
{
    /// <summary>
    /// JsonForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class JsonForm : UserControl
    {

        
        public JsonForm()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btnDeleteTab = sender as Button;
            if (btnDeleteTab == null)
                return;

            string delTabName = btnDeleteTab.CommandParameter.ToString();
            var delTab = this.dynmTab.Items.Cast<TabItem>().
                Where(tabItem => tabItem.Name.Equals(delTabName)).SingleOrDefault();

            if (delTab == null)
                return;

            /*
            if (this.TabItems.Count < 3)
            {
                MessageBox.Show("마지막 탭은 제거할 수 없습니다");
                return;
            }

            // 선택된 탭을 가져온다 (삭제될 수 도 있으며 삭제되지 않을 수 도 있다)
            var selectedTab = this.dynmTab.SelectedItem as TabItem;

            // 삭제 버튼이 클릭된 탭을 제거한다
            this.dynmTab.DataContext = null;
            this.TabItems.Remove(delTab);
            this.dynmTab.DataContext = this.TabItems;

            // 이전에 선택했던 탭으로 이동하며, 만약 삭제됐다면 처음 탭으로 이동한다
            if (selectedTab == null || selectedTab.Equals(delTab))
                selectedTab = this.TabItems[0];
            this.dynmTab.SelectedItem = selectedTab;
            */
        }
    }
}
