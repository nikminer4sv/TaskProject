   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.CreateCardWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	     � Title$  Create��5   	   $	��206��6   $	��301��6%    � 
ResizeMode$ NoResize=�5	   	   .��5
      � $��15q�6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 
TitleLabel��5      $��1��6$   $��TITLE��64   $��
0,-15,0,45q�6D   $��2��6X   $��Center=�6i   $
��Bold"�6�   $��16(�6�   �� 5   
   -   5         ��Name$ TitleTextBox��5      $��1��6(   $��1��68   $	��250��6E   $��	0,14,0,17q�6T   $��1                  ��6g    � Cursor$ IBeam��6�   � 5   
   $��2��6   $��Center=�6!   $��
0,55,0,-34q�6>   $��80��6R   $��1��6]    � 	ItemWidth$ 80��6m   .+�5      �� -   5         ��Name$ 	CreateBtn��5   &   	 �� 	IsDefault	 . $��60��69   $��_Create��6G   $
 Hand��6q   $��-13,0,-18,0q�6   5      �� 5   
   -   5        $ 
ImageLabel��5      $��1��6$   $��IMAGE��64   $��	0,39,0,19q�6D   $��2��6W   $��Center=�6h   $
��Bold"�6�   $��16(�6�   $��1��6�   �� 5   
   -   5        $ PathTextBox��5      $��1��6'   $��	88,18,0,9q�67   $��2��6J   �� 5   
   -   5        $ 	ChooseBtn��5      $��Choose��6$   $
��Left=�65   $��0,10,0,0q�6P   $��2��6b   $	��Top=�6o   $��2��6�   $��83��6�   $��34��6�   6�   5      5       