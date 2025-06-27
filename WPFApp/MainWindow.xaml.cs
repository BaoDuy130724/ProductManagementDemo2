using BusinessObjects;
using Services;
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

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        public MainWindow()
        {
            InitializeComponent();
            productService = new ProductService();
            categoryService = new CategoryService();
        }
        public void LoadCategoryList()
        {
            try
            {
                var catList = categoryService.GetCategories();
                cboCategory.ItemsSource = null;
                cboCategory.ItemsSource = catList;
                cboCategory.DisplayMemberPath = "CategoryName";
                cboCategory.SelectedValuePath = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadProductList()
        {
            try
            {
                var productList = productService.GetProducts();
                var categoryList = categoryService.GetCategories();
                foreach (var product in productList)
                {
                    var category = categoryList.FirstOrDefault(c => c.CategoryId == product.CategoryId);
                    if (category != null)
                    {
                        product.Category = category;
                    }
                }
                dgData.ItemsSource = null;
                dgData.ItemsSource = productList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of products");
            }
            finally
            {
                resetInput();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategoryList();
            LoadProductList();
        }

        private void resetInput()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtPrice.Text = "";
            txtUnitsInStock.Text = "";
            cboCategory.SelectedValue = 0;
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Product selectedProduct)
            {
                txtProductID.Text = selectedProduct.ProductId.ToString();
                txtProductName.Text = selectedProduct.ProductName ?? "";
                txtPrice.Text = selectedProduct.UnitPrice?.ToString() ?? "0";
                txtUnitsInStock.Text = selectedProduct.UnitsInStock?.ToString() ?? "0";
                cboCategory.SelectedValue = selectedProduct.CategoryId;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product();
                product.ProductName = txtProductName.Text.Trim();
                product.UnitPrice = decimal.Parse(txtPrice.Text.Trim());
                product.UnitsInStock = short.Parse(txtUnitsInStock.Text.Trim());
                product.CategoryId = Int32.Parse(cboCategory.SelectedValue.ToString());
                productService.SaveProduct(product);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadProductList();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtProductID.Text.Length > 0)
                {
                    Product product = new Product();
                    product.ProductId = Int32.Parse(txtProductID.Text.Trim());
                    product.ProductName = txtProductName.Text.Trim();
                    product.UnitPrice = decimal.Parse(txtPrice.Text.Trim());
                    product.UnitsInStock = short.Parse(txtUnitsInStock.Text.Trim());
                    product.CategoryId = Int32.Parse(cboCategory.SelectedValue.ToString());
                    productService.UpdateProduct(product);
                }
                else
                {
                    MessageBox.Show("You must select a Product.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadProductList();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtProductID.Text.Length > 0)
                {
                    Product product = new Product();
                    product.ProductId = Int32.Parse(txtProductID.Text.Trim());
                    product.ProductName = txtProductName.Text;
                    product.UnitPrice = decimal.Parse(txtPrice.Text.Trim());
                    product.UnitsInStock = short.Parse(txtUnitsInStock.Text.Trim());
                    product.CategoryId = Int32.Parse(cboCategory.SelectedValue.ToString());
                    productService.DeleteProduct(product);
                }
                else
                {
                    MessageBox.Show("You must select a Product.");
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                LoadProductList();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}