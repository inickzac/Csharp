   M S B A M L   `   `   `  ����H  DvisiterStaton, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:visiterStatonvisiterStaton  5          visiterStaton.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      -localclr-namespace:visiterStaton     5        �� 	Resources  5         
errorIdent �� Triggers P� Property P� Value �� Property �� Value �� RelativeSource �� Path �� 
TargetType	 �� BasedOn  ButtonWithComboBox  
EditButton  NewItemButton  DeleteItemButton(  "System.Windows.Controls.DatePicker��%�  &	        '�� �     7   &	 0    &	 �    7  &	 �    7  &	 �    7  '�� y    7   '9� y    '  �    �� 5       5      P� 5       �@=�HasError . . �5	      ��  �@�ToolTip 5
      ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�5      5      �� 5   
   " ��6   8	   .�5      ��  �@��IsTextSearchEnabled .  6D   �� 5       �@��StaysOpenOnEdit . 6?   �� 5       �@��
IsEditable . 6:   �� 5       � ��$	 150��6)   67   �� 5      % �@��IsSynchronizedWithCurrentItem$
 trueY�6>   6M   -   5        �� 5       � ��$ 10,0,0,0q�6'   6:   5      �� 5   
   .�5      ��  � �� 68   �� 6M   �� 5       � ��$ 15��60   6=   5      �� 5   
   8	   .�5      ��  � �� 6/   �� *EditCommand+6O   �� 5       � �� 5       �� 5!      .R�5"      �� $.��(pack://application:,,,/Resources/pen.png��6    $��10��6S   6`   5#      5%      5&      �� 5'   
   8	   .�5(      ��  � �� 6/   �� *
AddNewItem+6N   �� 5)       � �� 5*      �� 5+      .R�5,      �� $/��)pack://application:,,,/Resources/plus.png��6    $��10��6T   6a   5-      5/      50      �� 51   
   8	   .�52      ��  � �� 53      �� 54      .R�55      �� $0��*pack://application:,,,/Resources/minus.png��6    $��10��6U   6b   56      58      �� 59       � �� 6/   �� *DeleteCommand+6Q   5:      �� 5;   
   " ��6   8	   .�5<      ��  � ��$	 100��6)   67   �� 5=       � ��$
 Left=�64   6C   �� 5>       � ��$	 300��6*   68   �� 5?       � ��$ 20��6*   67   �� 5@       � ��$ 10,0,0,0q�6'   6:   5A      �� 5B   
   " 9�6   .�5C      ��  �@9�
IsExpanded . 6:   5D      �� 5E   
   "  6   .�5F      ��  � ��$ 10,0,0,0q�6'   6;   5G      5H      5I      