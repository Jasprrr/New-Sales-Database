namespace SchoolsMailing.Controls
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Animation;

    using SchoolsMailing.Extensions;

    public class ContactListView : ListView
    {

        public event EventHandler<ListViewItem> ItemInvoked;

        public ContactListView()
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.IsItemClickEnabled = true;
            this.ItemClick += this.OnItemClick;
            
            this.Loaded += this.OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            for (var i = 0; i < this.ItemContainerTransitions.Count; i++)
            {
                if (this.ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    this.ItemContainerTransitions.RemoveAt(i);
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var splitView = this.GetParentByType<SplitView>();
            if (splitView == null)
            {
                throw new InvalidOperationException(
                    "SplitViewPaneMenu cannot be applied to a control that is not of type SplitView");
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }

        private void InvokeItem(object item)
        {
            var lvi = item as ListViewItem;

            var isAlreadySelected = lvi != null && lvi.IsSelected;

            this.SetSelected(lvi);

            if (!isAlreadySelected)
            {
                this.ItemInvoked?.Invoke(this, item as ListViewItem);
            }
        }

        public void SetSelected(ListViewItem item)
        {
            var idx = -1;
            if (item != null)
            {
                idx = this.IndexFromContainer(item);
            }

            for (var i = 0; i < this.Items.Count; i++)
            {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                if (i != idx)
                {
                    lvi.IsSelected = false;
                }
                else if (i == idx)
                {
                    lvi.IsSelected = true;
                }
            }
        }

        public void ClearSelected()
        {
            for (var i = 0; i < this.Items.Count; i++)
            {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                lvi.IsSelected = true;
            }
        }
    }
}