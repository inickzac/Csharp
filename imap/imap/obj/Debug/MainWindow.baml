   M S B A M L   `   `   `  ����?  ;imap, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:imapimap  5   	   clr-namespace:imapimap  5   	       imap.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:imap     5   	   #selfclr-namespace:imap     5   	     � Title$  
MainWindow��5	   	   $��564.681��6   $��943.352��6-   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   $��60��6*   6;   �� 5      $
��auto �6   $	��184��6*   6<   �� 5       � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   R� -   5        $��treePath��5      $��1��6V   ��5      �� 5       �� DataType   imap.MenuItem"  6+    �� ItemsSource 6M   �� *Items+.R�5      �� ��6    �� *Title+69   5      5      �� 5   
   -   5         ��Name$ terminal��5      $��2��6*   $��0��67   $ �� HorizontalScrollBarVisibility$
 Auto=�6G   " �� VerticalScrollBarVisibility$
 Auto=�6l   �� 5    
   -   5         ��Name$ MessageText��5       $��1��6'   $��3��67   $
 Auto=�6H   $
 Auto=�6m   6�   � 5!   
   $��0��6   $��0��6!   .+�5"      �� $��Введите домен��6   $��Center=�6,   6I   �� 5#      -   5        $ Domain��5#      $	��186��6&   $��1q�62   $��imap.gmail.com��6=   $��26��6S   .��6_   6b   �� 5$      	 d� 	IsChecked$
	 TrueY�6   {�.  $"��Использовать SSL��6:   $��Center=�6U   $��10q�6p   6~   �� 5%      
 �� Orientation$
 
Horizontal=�6   .+�5&      �� $��
Логин��6   $��Center=�6(   6E   �� 5'      -   5        $ Login��5'      $	��194��6)   $��1q�65   $"��testmail9999899999@gmail.com��6@   $��26��6d   6r   5(      �� 5)      $
 
Horizontal=�6   .+�5*      �� $��Пароль��6   $��Center=�6)   6F   �� 5+      -   5        $ Password��5+      $	��158��6,   $��1q�68   $��12345678test��6C   $��26��6W   6e   5,      �� 5-      $
 
Horizontal=�6   .+�5.      �� -   5         ��Name$ connect��5.      $��Подключиться��6&   $��1q�6=   6`   �� 5/      -   5        $ 	Disconect��5/      $��Отключиться��6(   $��1q�6>   6c   50      �� 51      -	   5         ��Name$ 
StatusConn��51   U   $L��FСостояние подключения: Не подключенно��6   
���  ��6k   52      55      56      