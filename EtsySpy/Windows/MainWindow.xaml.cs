using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EtsySpy.Classes;
using EtsySpy.Properties;
using EtsySpy.Utils;
using Xceed.Wpf.DataGrid;

namespace EtsySpy.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, DataGridItemProperty> dataGridItemProperties = new Dictionary<string, DataGridItemProperty>();
        private HistoryManager historyManager = new HistoryManager();

        public int ProductResultCount { get; set; }

        public List<EtsyQuery> QueryHistory { get; set; } 

        public MainWindow()
        {
            this.QueryHistory = historyManager.GetQueryHistory().History;

            InitializeComponent();
            SetDataGridItemProperties();

            EtsyQuery lastQuery = historyManager.GetLastQuery();
            tbQuery.Text = lastQuery.QueryText;
            if (lastQuery.QueryType == EtsyQuery.QueryTypes.Shop)
            {
                rbByShop.IsChecked = true;
            }

            //RebindQueryHistory();

            /*
            if (Settings.Default.NeedsUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.NeedsUpgrade = false;
                Settings.Default.Save();
            }
            */
        }

        #region DataGrid Column Setup
        private void SetDataGridItemProperties()
        {
            DataGridItemProperty dgip = new DataGridItemProperty("Title", typeof (string));
            dataGridItemProperties.Add("Title", dgip);

            dgip = new DataGridItemProperty("State", typeof(string));
            dataGridItemProperties.Add("State", dgip);

            dgip = new DataGridItemProperty("OriginalCreationTsz", typeof(DateTime));
            dgip.Title = "Creation Date";
            dataGridItemProperties.Add("OriginalCreationTsz", dgip);

            dgip = new DataGridItemProperty("EndingTsz", typeof(DateTime));
            dgip.Title = "End Date";
            dataGridItemProperties.Add("EndingTsz", dgip);

            dgip = new DataGridItemProperty("LastModifiedTsz", typeof(DateTime));
            dgip.Title = "Last Modified";
            dataGridItemProperties.Add("LastModifiedTsz", dgip);

            dgip = new DataGridItemProperty("StateTsz", typeof(DateTime));
            dgip.Title = "State Last Modified";
            dataGridItemProperties.Add("StateTsz", dgip);

            dgip = new DataGridItemProperty("Price", typeof(string));
            dataGridItemProperties.Add("Price", dgip);

            dgip = new DataGridItemProperty("Quantity", typeof(int));
            dataGridItemProperties.Add("Quantity", dgip);

            dgip = new DataGridItemProperty("Views", typeof(int));
            dataGridItemProperties.Add("Views", dgip);

            dgip = new DataGridItemProperty("NumFavorers", typeof(int));
            dgip.Title = "Favorites";
            dataGridItemProperties.Add("NumFavorers", dgip);

            dgip = new DataGridItemProperty("ShouldAutoRenew", typeof(bool));
            dgip.Title = "Auto Renew";
            dataGridItemProperties.Add("ShouldAutoRenew", dgip);

            dgip = new DataGridItemProperty("Tags", typeof(List<string>));     
            dgip.Converter = new ArrayToStringValueConverter();
            dataGridItemProperties.Add("Tags", dgip);

        }
        #endregion

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.ProductResultCount = 666;

            if (rbByShop.IsChecked.HasValue && rbByShop.IsChecked.Value)
            {                
                GetEtsyProductResultsForShopAsync(tbQuery.Text);
            }
            else
            {
                lblShopDetails.Visibility = Visibility.Hidden;
                GetEtsyProductResultsAsync(tbQuery.Text);
            }
            
            //RebindQueryHistory();
        }

        async void GetEtsyProductResultsAsync(string id)
        {
            EtsyApi ea = new EtsyApi();

            EtsyProductResults etsyProductResults = await Task.Run(() => ea.GetEtsyProduct(id));

            BindProductResults(etsyProductResults.Results);

            //MessageBox.Show("Async Done");

            this.ProductResultCount = etsyProductResults.Count;

            if (etsyProductResults.Count > 0)
            {
                historyManager.SaveLastQuery(tbQuery.Text, tbQuery.Text, EtsyQuery.QueryTypes.Product);
                lbQueryHistory.Items.Refresh();
            }

            lblResultCount.Visibility = Visibility.Visible;
            

        }

        async void GetEtsyProductResultsForShopAsync(string id)
        {
            EtsyApi ea = new EtsyApi();

            EtsyShopResults etsyShopResults = await Task.Run(() => ea.GetEtsyShop(id));

            BindProductResults(etsyShopResults.Results.First().Listings);

            this.ProductResultCount = etsyShopResults.Count;


            if (etsyShopResults.Count > 0)
            {
                historyManager.SaveLastQuery(tbQuery.Text, tbQuery.Text, EtsyQuery.QueryTypes.Shop);
                lbQueryHistory.Items.Refresh();
            }



            lblShopDetails.Visibility = Visibility.Visible;
        }

        private void BindProductResults(List<EtsyProduct> results)
        {
            DataGridCollectionView dgcv = new DataGridCollectionView(results, typeof(EtsyProduct), false, false);


            dgcv.ItemProperties.Add(dataGridItemProperties["Title"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["State"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["OriginalCreationTsz"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["EndingTsz"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["LastModifiedTsz"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["StateTsz"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["Price"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["Quantity"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["Views"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["NumFavorers"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["ShouldAutoRenew"]);
            dgcv.ItemProperties.Add(dataGridItemProperties["Tags"]);

            dgResults.ItemsSource = dgcv;
        }

        private void dgResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            if (e.ButtonState != MouseButtonState.Pressed)
            {
                return;
            }

            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while (dep != null && !(dep is DataCell) )
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep != null && dep is DataCell)
            {
                DataCell c = (DataCell) dep;
                MessageBox.Show(c.Content.ToString());
            }

        }

        private void RebindQueryHistory()
        {
            QueryHistory queryHistory = historyManager.GetQueryHistory();
            if (queryHistory != null && queryHistory.History.Count > 0)
            {
                lbQueryHistory.ItemsSource = queryHistory.History;
                

            }
        }

        private void EtsySpy_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void lbQueryHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbQueryHistory.SelectedItem != null)
            {
                EtsyQuery q = (EtsyQuery)lbQueryHistory.SelectedItem;
                tbQuery.Text = q.QueryText;

                if (q.QueryType == EtsyQuery.QueryTypes.Product)
                {
                    rbByProduct.IsChecked = true;
                }
                else
                {
                    rbByShop.IsChecked = true;
                }

                lbQueryHistory.SelectedItem = null;

            }



        }
    }
}
