   M S B A M L   `   `   `  ����U  QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089 "  Brush_DataGridHeaderBackground  �� 
StartPoint �� EndPoint � Color � Offset + 'Brush_DataGridHeaderMouseOverBackground  Brush_DataGridHeaderBorder $  Brush_DataGridHeaderSortedBorder ( $Brush_DataGridHeaderSortedBackground  Brush_DataGridSelected �� Color $  Brush_DataGridSelectedForeground " Brush_DataGridAltRowBackground  Style_HeaderGripper �� 
TargetType �� Property �� Value �� 
TargetType	 �� Padding@    :Microsoft.Windows.Controls.Primitives.DataGridColumnHeader
 � ColumnDefinitions ��Name F�Name �� Stretch J� RenderTransformOrigin n�Name �� Triggers P� Value P� Property �� 
TargetName �� Angle=   7Microsoft.Windows.Controls.Primitives.DataGridRowHeader J� SnapsToDevicePixels-   'Microsoft.Windows.Controls.DataGridCell,   &Microsoft.Windows.Controls.DataGridRowB   <Microsoft.Windows.Controls.Primitives.SelectiveScrollingGrid � RowDefinitionsB   <Microsoft.Windows.Controls.Primitives.DataGridCellsPresenter   
ItemsPanel$   SelectiveScrollingOrientation �� RelativeSource �� AncestorType)   #Microsoft.Windows.Controls.DataGrid �� Path!   HeadersVisibilityConverter �� 	Converter:   4Microsoft.Windows.Controls.DataGridHeadersVisibility
   Row �� ConverterParameter�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit   5      %�  &	        &	 �     &	 (    &	 �    &	 P    &	 �    &	     &	 &    &	 G    '   Y    7  7  7  7  7  7  '      '  �    '  
    7  7  �� 5      $	  0,0&�6A   $	 0,1&�6R   .��5   
   � $ #739D20��6   $ 0Q�6'   � 5   
   $ #4F8300��6   $ 1Q�6'   5      �� 5      $	  0,0&�6J   $	 0,1&�6[   .��5   
   � $ #90B321��6   $ 0Q�6'   � 5   
   $ #66A002��6   $ 1Q�6'   5      �� 5      $	  0,0&�6=   $	 0,1&�6N   .��5   
   � $ #9ECD42��6   $ 0Q�6'   � 5   
   $ #3D5808��6   $ 1Q�6'   5      �� 5      $	  0,0&�6C   $	 0,1&�6T   .��5   
   � $ #B58801��6   $ 0Q�6'   � 5   
   $ #681900��6   $ 1Q�6'   5      �� 5!      $	  0,0&�6G   $	 0,1&�6X   .��5"   
   � $ #C46007��6   $ 0Q�6'   � 5#   
   $ #AF2600��6   $ 1Q�6'   5$      �� 5&      $ #3E4955��65   �� 5(      $ #FFFFFF��6?   �� 5*      $ #F4F4EE��6=   �� 5F      " n�6(   .�5G   
   ��  � ��$ 8��6"   �� 5H   
    � ��
 ���� 6'   �� 5I   
    �@�Cursor$ SizeWE��6#   �� 5J   
    � �� 5K      �� 5L      " n�6"   .R�5M      �� #	 z��6   #��z��6A   5N      5P      5Q      �� 5S      "   6   .�5T   
   ��  �@J�SnapsToDevicePixels . 60   �� 5U   
    � ��$ 0��6%   �� 5V   
    � ��$ 28��6&   �� 5W   
    � ��
 �����6'   �� 5X   
    �@�Cursor$
 Hand��6#   �� 5Y   
    � �� 5Z      �� 5[      "   6"   .R�5\      � 
 5]      �� 5^      $��* �6/   �� 5_      $
��Auto �6/   .+�5a      �� $ BackgroundBorder��6!   $��0,1,0,1q�6;   8��  8�� $��2��6�   �� 5b      $��6,3,6,3q�6+   $��Center=�6<   F� 5c      $ 	SortArrow��6   $y�	Collapsed=�62   "����        �@B     � � @B 	$
 Fill=�6b   $��1��6q   $��8��6�   $��6��6�   
�������$��0,0,8,0q�6�   $��Center=�6�   $ 0.5,0.4&�6�   �� 5d      $��1��6$   ���#AAC377$��Right=�6=   $��2��6Y   �� 5e      $��1��6$   $��0,0,1,0q�6.   ���#425B10$��Right=�6N   $��2��6j   n� 5f      $ PART_LeftHeaderGripper��6    $
��Left=�6@   8�� 6[   n� 5g      $ PART_RightHeaderGripper��6    $��Right=�6A   8�� 6]   5h       5i      P� 5j       6;   Z�  �@  SortDirection. �5k      �� $ BackgroundBorder��6%    � ��
 ���� 6Y   �� 5l      $ BackgroundBorder��6%    � ��
 ���� 6[   5m      P� 5n       �@J�IsMouseOver . . �5o      �� $ BackgroundBorder��6%   8   � ��6Y   �� 5p      $ BackgroundBorder��6%   8   � ��6Z   5q      P� 5r       �@  SortDirection$ 	AscendingX�6;   . �5s      �� $ 	SortArrow��6%    � y�$ Visible=�6R   �� 5t      $ 	SortArrow��6%    � z� 5u   "   �� 5v   &   $	 180Q�66   5x      5y      P� 5z       �@  SortDirection$ 
DescendingX�6;   . �5{      �� $ 	SortArrow��6%    � y�$ Visible=�6R   5|      P� 5}       �@  DisplayIndex$ 0��6:   . �5~      �� $ PART_LeftHeaderGripper��6M    � y�$ 	Collapsed=�6;   6s   5      5�      5�      5�      �� 5�      "  62   .�5�   
   ��  �@J�SnapsToDevicePixels . 60   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      �� 
������ ._�5�      �� # z 6+   5�      5�      5�      5�      �� 5�      "  6   .�5�   
   ��  �@�OverridesDefaultStyle . 62   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      �� $ BackgroundBorder��6   
������ ._�5�      �� $��Center=�6+   $��4,0,6,0q�6F   5�      5�      5�      5�      �� 5�      "  6,   .�5�   
   ��  � ��# � ��6'   �� 5�   
    �@J�SnapsToDevicePixels . 60   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      �� $ 
DGR_Border��5�      #��z��5�      #��z��5�      #��z��5�       . ._�5�        
 5�      �� 5�   "   $
��Auto �63   �� 5�   "   $��* �63    5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   .+�5�        $��1��5�      #��z 5�      # z 5�        5�      $ Vertical=�5�      $��2��5�      y�5�      ��  �� "  $ HeadersVisibility�# Z # Z 5�      5�       5�      P� 5�       �@ 
IsSelected . . �5�      �� $ 
DGR_Border��6%   8    � ��6S   �� 5�      $ 
DGR_Border��6%   8   � ��6_   5�      5�      5�      5�      5&     