using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TrmWpfUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;


        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        // total
        public string SubTotal
        {
            get
            {
                decimal subTotal = 0;

                foreach (var item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }
                return subTotal.ToString("C");
            }

        }

        public string Total
        {
            get
            {
                // TODO - Replace with calculation
                return "$0.00";
            }

        }

        public string Tax
        {
            get
            {
                // TODO - Replace with calculation
                return "$0.00";
            }

        }


        // buttons 

        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                // check if item is selected 
                // and that quantity has value
                if (ItemQuantity > 0 &&
                    SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }

        }

        public void AddToCart()
        {
            // find exsiting objecct in cart
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);
            }

            // update the quantity in DB
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1; // reset the ItemQuantity
            NotifyOfPropertyChange(() => SubTotal);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                // check if item is selected 

                return output;
            }

        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                // check if something is in the cart

                return output;
            }

        }

        public void CheckOut()
        {

        }

    }
}
