using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sharomank.RegexTester
{
    /// <summary>
    /// Author: Roman Kurbangaliev (Sharomank)
    /// </summary>
    public partial class RegexTesterMain : UserControl
    {
        private static long LAST_TAB_ITEM_INDEX = 1;

        public RegexTesterMain()
        {
            InitializeComponent();
        }

        private void AddNewTab()
        {
            if (MainTabControl.Items.Count > 99)
            {
                MessageBox.Show("You open many tabs. Please close unnecessary.");
                return;
            }

            TabItem newTab = new TabItem();
            newTab.Header = GetTabItemName(MainTabControl);
            newTab.Content = new RegexTesterPage();
            newTab.IsSelected = true;
            MainTabControl.Items.Add(newTab);
        }

        private void CloseTab()
        {
            MainTabControl.Items.Remove(MainTabControl.SelectedItem);
        }

        private static String GetTabItemName(TabControl tabControl)
        {
            if (tabControl.Items.Count == 1)
                LAST_TAB_ITEM_INDEX = 0;
            return String.Format("Regex Test {0}", ++LAST_TAB_ITEM_INDEX);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabItemAdd.IsSelected)
            {
                AddNewTab();
            }
        }

        private void RootControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.T)
                    AddNewTab();
                else if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && e.Key == Key.W)
                    CloseTab();
            }
        }

        private void MenuItem_CloseTab_Click(object sender, RoutedEventArgs e)
        {
            CloseTab();
        }

        private void MenuItem_CloseOtherTabs_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = MainTabControl.SelectedIndex;
            for (int index = MainTabControl.Items.Count - 1; index > 0; index--)
            {
                if (index != selectedIndex)
                {
                    MainTabControl.Items.RemoveAt(index);
                }
                else
                {
                    selectedIndex = 0;
                }
            }
        }

        private void MenuItem_CloseAllTabs_Click(object sender, RoutedEventArgs e)
        {
            LAST_TAB_ITEM_INDEX = 0;
            for (int index = MainTabControl.Items.Count - 1; index > 0; index--)
            {
                MainTabControl.Items.RemoveAt(index);
            }
        }

        private void MenuItem_NewTab_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();
        }
    }
}