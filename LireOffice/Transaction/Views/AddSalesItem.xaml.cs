﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for AddSalesItem.xaml
    /// </summary>
    public partial class AddSalesItem : UserControl
    {
        public AddSalesItem()
        {
            InitializeComponent();

            Loaded += AddSalesItem_Loaded;

            SearchTextBox.KeyUp += SearchTextBox_KeyUp;
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.SelectAll();
        }

        private void AddSalesItem_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Focus();
            SearchTextBox.SelectAll();
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                dataGrid.Focus();
            }
        }
    }
}