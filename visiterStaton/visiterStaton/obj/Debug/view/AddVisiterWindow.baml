   M S B A M L   `   `   `  ����H  DvisiterStaton, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:visiterStatonvisiterStaton  5   	   !    visiterStaton.VisiterWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:visiterStaton     5   	     � Title$  AddVisiterWindow��5	   	   $��659.252��6"   $��769.63��63    � 	Resources 5      ��%    5      .��5      �� .��5   
   � .+�5      �� .+�5      � .+�5      �� .+�5      �� .��6    Имя6%   �� 5      ��6#   �� *Name+ �� ValidatesOnDataErrors .  �� UpdateSourceTrigger$ PropertyChanged=�6|   5      �� 5      .+�5      �� .��6    Фамилия6)   �� 5      ��6#   �� *
LastName+ . 6[   5      �� 5      .+�5      �� .��6    Должность6+   �� 5      ��6#   �� *
Position+ . 6[   5      5      9� 5       $��Документ��6    � DataContext 6-   �� *
Document+.��5!      �� .+�5"      ��  �� Orientation$ 
Horizontal=�6%   .+�5#      �� .+�5$   "   �� .��6(   
Серия6/   �� 5%   "   ��6*   �� *Series+ . $ PropertyChanged=�6�   5&      �� 5'      .+�5(   "   �� .��6(   
Номер6/   �� 5)   "   $��10,0,0,0q�6�   ��6*   �� *Number+ . $ PropertyChanged=�6�   5*      5+      �� 5,      .+�5-      �� .��6$   Дата выдачи61   (  "System.Windows.Controls.DatePicker  5.      -   5        $	��300��5.   k   $
��Left=�6z      SelectedDate 6)   �� *Date_of_issue+6�   5/      � 50       � Orientation$ 
Horizontal=�6$   .+�51      �� .+�52   "   �� $��Тип документа��6(   �� 53   "   $ 
Horizontal=�6-   .+�54   &   �� $ 
Horizontal=�61   .+�55   *   ��  �� DisplayMemberPath$
 Name��57   0   ��55   4   �� *DocumentTypes+	 �� Text	 56   0   �� *DocumentType.Name+
 �� Mode$
 OneWay=� �� SelectedItem 58   0   �� *DocumentType+$
 TwoWay=�$ PropertyChanged=� .  �� SelectedIndex 59   0   �� *DocumentType.Sel+$
 OneWayToSource=��� 5:   *      
EditButton#��[  6Y    61   �� *NewdocumentType+6Y   �� 5;   *     NewItemButton#��[ 6Y    61   �� *NewdocumentType+6Y   �� 5<   *     DeleteItemButton#��[ 6Y    61   �� *NewdocumentType+6Y   5=   '   5>   #   5?      �� 5@      .+�5A   "   �� $2��,Орган выдавший документ��6(   �� 5B   "   $ 
Horizontal=�6-     
errorIdent#��[ 6F   .+�5C   &   �� $
 Name��5E   %   ��5C   0   �� *IssuingAuthoritys+	 5D   %   �� *IssuingAuthority.Name+$
 OneWay=� 5F   %   �� *IssuingAuthority+$
 TwoWay=�$ PropertyChanged=� .  5G   %   �� *IssuingAuthority.Sel+$
 OneWayToSource=�5H   '   �� 5I   &   #��[  6^    6-   �� *EditableIssuingAuthority+6^   �� 5J   &   #��[ 6^    6-   �� *EditableIssuingAuthority+6^   �� 5K   &   #��[ 6^    6-   �� *EditableIssuingAuthority+6^   5L   #   5M      5N      5O      5P      � 5Q      .+�5R      �� .+�5S      �� $��Место работы��6$   6:   �� 5T      $ 
Horizontal=�6%   #��[ 6>   .+�5U      �� $
 Name��5W   %   ��5U   (   �� *Organizations+	 5V   %   �� *Organization.Name+$
 OneWay=� 5X   %   �� *Organization+ . $
 TwoWay=�$ PropertyChanged=� 5Y   %   �� *Organization.Sel+$
 OneWayToSource=�5Z      �� 5[      #��[  6R    6%   �� *EditableOrganization+6R   �� 5\      #��[ 6R    6%   �� *EditableOrganization+6R   �� 5]      #��[ 6R    6%   �� *EditableOrganization+6R   5^      5_      5`      9� 5a      $*��$Разрешение на съмку��6   .��5b      �� .+�5c      &   System.Windows.Controls.DataGrid  -   5        $��c1DataGrid1��5c   �      AutoGenerateColumns .  ��6#   �� *ShootingPermissions+ 6k   �� *ShootingPermission+   Columns 5d      0  *System.Windows.Controls.DataGridTextColumn  5e   "   ,  &System.Windows.Controls.DataGridColumn   Header$ Тип камеры��65   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6I   �� *
CameraType+6j     5f   "   $% Разрешение выдал��65    6O   �� *ShootingAllowed+6u     5g   "   $ Обьект съемки��65    6L   �� *SubjectOfShooting+6t     5h   "   $( "Дата начала съемки��65    6Q   �� *Start+6m     5i   "   $. (Дата окончания съемки��65    6T   �� *End+6n     5j   "   $ Цель съемки��65    6J   �� *ShootingPurpose+6p   4  .System.Windows.Controls.DataGridTemplateColumn  5k   "      CellTemplate 5l   &   �� 5m   *   .R�5n   .   �� $ 
Horizontal=�69   .+�5o   2   �� $��Удалить��69   ���	#FFE4ACAC��6L   �� *DeleteCommand+��6n   �� *EditableShootingPermission+6�   �� 5p   2   $��Изменить��69   ��6L   �� *EditCommand+��6l   �� *EditableShootingPermission+�� 5q   2   $/��)Генерировать документ��69   ��6Y   �� *DocumentGenerate+��6~   �� *
DocumentTP+5r   /   5s   +   5u   #   5w      �� 5x      -   5        $��Добавить��5x   !   ��64   �� *
AddNewItem+��6S   �� *EditableShootingPermission+6�   5y      5z      9� 5{      $'��!Временный пропуск��6   .��5|      �� .+�5}         .  ��6#   �� *TemporarPasses+ 6f   �� *TemporarPass+ 5~        5   "   $% Разрешение выдал��65    6O   �� *
PassIssued+6p     5�   "   $, &Дата начала действия��65    6S   �� *	ValidWith+6s     5�   "   $2 ,Дата окончания действия��65    6V   �� *
ValidUntil+6w     5�   "   $ Цель выдачи��65    6J   �� *PurposeOfIssuance+6r     5�   "    5�   &   �� 5�   *   .R�5�   .   �� $ 
Horizontal=�69   .+�5�   2   �� $��Удалить��69   ���	#FFE4ACAC��6L   �� *DeleteCommand+��6n   �� *EditabletemporarPass+6�   �� 5�   2   $��Изменить��69   ��6L   �� *EditCommand+��6l   �� *EditabletemporarPass+�� 5�   2   $/��)Генерировать документ��69   ��6Y   �� *DocumentGenerate+��6~   �� *
DocumentTP+5�   /   5�   +   5�   #   5�      �� 5�      -   5        $��Добавить��5�   !   ��64   �� *
AddNewItem+��6S   �� *EditabletemporarPass+6�   5�      5�      9� 5�      $#��Разовый пропуск��6   .��5�      �� .+�5�         .  ��6#   �� *SinglePasses+ 6d   �� *
SinglePass+ 5�        5�   "   $% Разрешение выдал��65    6O   �� *SinglePassIssued+6v     5�   "   $, &Дата начала действия��65    6S   �� *
PassIssued+6t     5�   "   $2 ,Дата окончания действия��65    6V   �� *
ValidUntil+6w     5�   "   $ Цель выдачи��65    6J   �� *PurposeOfIssuance+6r     5�   "   $" Сопровождающий��65    6M   �� *Accompanying+6p     5�   "    5�   &   �� 5�   *   .R�5�   .   �� $ 
Horizontal=�69   .+�5�   2   �� $��Удалить��69   ���	#FFE4ACAC��6L   �� *DeleteCommand+��6n   �� *EditableSinglePass+6�   �� 5�   2   $��Изменить��69   ��6L   �� *EditCommand+��6l   �� *EditableSinglePass+�� 5�   2   $/��)Генерировать документ��69   ��6Y   �� *DocumentGenerate+��6~   �� *
DocumentTP+5�   /   5�   +   5�   #   5�      �� 5�      -   5        $��Добавить��5�   !   ��64   �� *
AddNewItem+��6S   �� *EditableSinglePass+6�   5�      5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� ��6   �� *SaveCommand+��6=   �� .��6[   Сохранить6f   �� 5�      -   5        .��5�   4   Отмена6<   5�      5�      5�      5�      5�      