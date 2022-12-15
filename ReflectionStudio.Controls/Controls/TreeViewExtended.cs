﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReflectionStudio.Controls
{
	/// <summary>
	/// TreeViewExtended provide populate on demand and item selection with the right mouse click
	/// </summary>
	public class TreeViewExtended : TreeView
	{
		/// <summary>
		/// Constructor
		/// </summary>
		static TreeViewExtended()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewExtended),
																		new FrameworkPropertyMetadata(typeof(TreeViewExtended)));
		}

		/// <summary>
		/// OnApplyTemplate allow to attach to PreviewMouseRightButtonDown for selecting tree item before context menu display
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.TreeViewExtended_PreviewMouseRightButtonDown);
		}

		#region --------------------DEPENDENCY PROPERTIES--------------------

		/// <summary>
		/// PopulateOnDemand DependencyProperty
		/// </summary>
		public static readonly DependencyProperty PopulateOnDemandProperty =
			   DependencyProperty.Register("PopulateOnDemand", typeof(bool), typeof(TreeViewExtended));

		/// <summary>
		/// PopulateOnDemand property
		/// </summary>
		public bool PopulateOnDemand
		{
			get { return (bool)GetValue(PopulateOnDemandProperty); }
			set { SetValue(PopulateOnDemandProperty, value); }
		}

		#endregion

		#region --------------------EVENTS--------------------

		/// <summary>
		/// Allow to select an item before the context menu pop's up	
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TreeViewExtended_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeView control = sender as TreeView;

			IInputElement clickedItem = control.InputHitTest(e.GetPosition(control));
			
			while ((clickedItem != null) && !(clickedItem is TreeViewItem))
			{
				FrameworkElement frameworkkItem = (FrameworkElement)clickedItem;
				clickedItem = (IInputElement)(frameworkkItem.Parent ?? frameworkkItem.TemplatedParent);
			}
			
			if (clickedItem != null)
				((TreeViewItem)clickedItem).IsSelected = true;
		}
		#endregion

		#region --------------------TREEVIEWITEM POPULATE EVENT--------------------

		/// <summary>
		/// ItemNeedPopulateEvent event
		/// </summary>
		public static readonly RoutedEvent ItemNeedPopulateEvent = EventManager.RegisterRoutedEvent("ItemNeedPopulateEvent",
																RoutingStrategy.Bubble,
																typeof(ItemNeedPopulateEventHandler), typeof(TreeViewExtended));

		/// <summary>
		/// ItemNeedPopulateEventHandler delegate signature
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void ItemNeedPopulateEventHandler(object sender, System.Windows.RoutedEventArgs e);

		/// <summary>
		/// ItemNeedPopulateEventHandler
		/// </summary>
		public event ItemNeedPopulateEventHandler OnItemNeedPopulate
		{
			add { AddHandler(ItemNeedPopulateEvent, value); }
			remove { RemoveHandler(ItemNeedPopulateEvent, value); }
		}

		/// <summary>
		/// shorcut for raising ItemNeedPopulateEvent
		/// </summary>
		/// <param name="item"></param>
		protected void RaiseItemNeedPopulate( TreeViewItem item )
		{
			RoutedEventArgs args = new RoutedEventArgs();
			args.RoutedEvent = ItemNeedPopulateEvent;
			args.Source = item;
			RaiseEvent(args);
		}

		#endregion

		#region --------------------TREEVIEWITEM MANAGEMENT--------------------

		/// <summary>
		/// Internal management of the treeview item expansion. remove the dummy node if needed and fire the ItemNeedPopulateEvent
		/// event when the PopulateOnDemand property is set
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void node_Expanded(object sender, RoutedEventArgs e)
		{
			TreeViewItem opened = (TreeViewItem)sender;

			if (opened.Items[0] is TreeViewItemDummy && PopulateOnDemand)
			{
				opened.Items.Clear();
				RaiseItemNeedPopulate(opened);
			}
		}

		/// <summary>
		/// Add a new treeview item to the specified parent. Manage dummy node and PopulateOnDemand
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="label"></param>
		/// <param name="tag"></param>
		/// <param name="needDummy"></param>
		/// <returns></returns>
		public TreeViewItem AddItem(TreeViewItem parent, string label, object tag, bool needDummy = true)
		{
			TreeViewItem node = new TreeViewItem();
			node.Header = label;
			node.Tag = tag;

			if (PopulateOnDemand && needDummy)
			{
				node.Expanded += new RoutedEventHandler(node_Expanded);
				node.Items.Add(new TreeViewItemDummy());
			}

			if (parent != null)
				parent.Items.Add(node);
			else
				this.Items.Add(node);

			return node;
		}

		/// <summary>
		/// Add a dummy treeview item to the specified parent. Manage dummy node and PopulateOnDemand
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="needDummy"></param>
		/// <returns></returns>
		public void AddDummyItem(TreeViewItem parent, bool needDummy = true)
		{
			if (PopulateOnDemand && needDummy)
			{
				parent.Expanded += new RoutedEventHandler(node_Expanded);
				parent.Items.Add(new TreeViewItemDummy());
			}
		}

		/// <summary>
		/// Return the parent of a treeview item which contains the given Type in his Tag property
		/// </summary>
		/// <param name="item"></param>
		/// <param name="searchedType"></param>
		/// <returns></returns>
		public TreeViewItem FindParentNode(object item, Type searchedType)
		{
			DependencyObject parent = (DependencyObject)item;

			while (parent != null && !(parent is TreeView))
			{
				if( ((TreeViewItem)parent).Tag != null )
				{
					if( ((TreeViewItem)parent).Tag.GetType() == searchedType )
						return (TreeViewItem)parent;
				}
				parent = ItemsControl.ItemsControlFromItemContainer(parent);
			}
			return (TreeViewItem)parent;
		}

		/// <summary>
		/// Return the parent of a treeview item
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public TreeViewItem ParentNode(TreeViewItem item)
		{
			DependencyObject parent = (DependencyObject)item;
			return (TreeViewItem)ItemsControl.ItemsControlFromItemContainer(parent);
		}

		#endregion
	}

	/// <summary>
	/// TreeViewItemDummy
	/// </summary>
	internal class TreeViewItemDummy : TreeViewItem
	{
		/// <summary>
		/// Constructor, init the header with dummy string
		/// </summary>
		public TreeViewItemDummy()
		{
			Header = "Dummy";
		}
	}
}
