   M S B A M L   `   `   `  ����W  SLabadamiLoyaltyCardGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:LabadamiLoyaltyCardGeneratorLabadamiLoyaltyCardGenerator  5   	   -    'LabadamiLoyaltyCardGenerator.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:LabadamiLoyaltyCardGenerator     5   	   U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  5   	     � Title$%  Labadami Loyalty Card Generator��5	   	   $	��650��5
   	   $
��1000��6    � WindowStyle$ ThreeDBorderWindow=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��400 �6   �� 5      .+�5   
   � 
������ � RowDefinitions 5      �� 5      $��50 �6    �� 5      $	��100 �6    �� 5      $��50 �6    �� 5      $��50 �6    �� 5      $��50 �6    �� 5      $��50 �6    �� 5      $��50 �6    �� 5       5!      �� 5"      $	��120 �6#   �� 5#      .+�5&      �� $��Branch��6   $��Center=�6&   $��10q�6A   
�������$��18(�5'   +   $
��Bold"�69   � 5(      $��1��6    5)      �� 5*      �� 5+      $��70 �6'   .+�5-      �� -   5        $��	txtBranch��5-      $��5q�6+   $��14(�66   $��30��5.      $��Center=�6&   
�������5/      �� 50      -   5        $��btnSelectBranch��50      $��1��60   $��Select��6@   $��30��51      $��5q�6%   $��14(�60   52      �� 55      $��Address��6   $��1��6'   $��Center=�64   $��10q�56      
�������$��18(�67   $
��Bold"�6E   �� 57      -   5        $��
txtAddress��57      $��1��6(   $��1��65   $��5q�6E   $��14(�58      $��90��6$   
������� �� AcceptsReturn .  �� TextWrapping$
 Wrap=�59      �� 5;      $��	CP Number��6   $��2��6)   $��Center=�66   $��10q�5<      
�������$��18(�67   $
��Bold"�6E   �� 5=      -   5        $��txtCPNumber��5=      $��2��6)   $��1��66   $��5q�6F   $��14(�5>      $��30��6$   $��Center=�60   
�������$�� ��5?   )   �� 5A      $��Output Path��6   $��3��6+   $��Center=�68   $��10q�5B      
�������$��16(�67   $
��Bold"�6E   � 5C      $��3��6   $��1��6     5D      �� 5E      �� 5F      $��70 �6'   .+�5H      �� -   5        $��txtOutputPath��5H       �� 
IsReadOnly . $��5q�6A   $��14(�6L   $��30��5I      $��Center=�6&   
�������5J      �� 5K      -   5        $��btnBrowseOutputPath��5K      $��1��64   $��Browse��6D   $��30��5L      $��5q�6%   $��14(�60   5M      �� 5O      $��No. of Card��6   $��4��6+   $��Center=�68   $��10q�5P      
�������$��18(�67   $
��Bold"�6E   �� 5Q      -   5        $��txtCard��5Q      $��4��6%   $��1��62   $��5q�6B   $��14(�5R      $��30��6$   $��Center=�60   
�������$	��100��5S   )   �� 5U      -   5        $��btnGenerate��5U      $��GENERATE��6(   $��5��6;   $��2��6H   $��40��5V      $��5q�6%   $��14(�60   $	��Top=�6>   �� 5W      -	   5        $��btnSave��5W      $
��SAVE��6$   $��6��63   $��2��6@   $��40��5X      $��5q�6%   $��14(�60   $	��Top=�6>   � 5Z      -
   5        $��
gridStatus��5Z      $��7��6%   $��2��62   
������� 5[      �� 5\      �� 5]      $��50 �6$   .+�5_      �� -   5        $��
imgLoading��5_      "  WpfAnimatedGif.ImageBehavior	   AnimatedSource$	 /Resources/hourglass.gif��6*   $	��100��6f   �� 5a      -   5        $��	lblStatus��5a      $*��$Generating 1 of 100 Loyalty Cards...��6-   $��1��6Y   $��Center=�5b      $��Center=�67   $��18(�6T   $
��Bold"�6b   5c      5d      � 5f   
   $��1��6   
������� 5g      �� 5h      �� 5i      .+�5k      �� -   5        $��Imgfront��5k      
 J� SnapsToDevicePixels
 . $��Uniform=�6?   $��Center=�5l      $	��450��6-   $��/Resources/Front.png��6<   �� 5n      -   5        $��ImgQR��5n      
 . $��Uniform=�6<   $��Center=�5o      $	��130��6-   $	��130��69   $��230,-30,0,0q�5p      ��5q      �� 5r       �� 	UriSource$ /Resources/QR.jpg?�6"    �� DecodePixelWidth$	 300��5s      5u      �� 5w      -   5        $��	lblBranch��5w      $��SUMULONG��6)   $��	-70,0,0,0q�69   $	��150��6L   $��25��5x      $��18(�6$   $
��Bold"�62   $��Verdana)�6D    �� TextAlignment$ Center=�5y      �� 5|      -   5        $��
lblAddress��5|      $��
-80,70,0,0q�6*   $��160px��6>   $
��50px��6L   $��10(�5}      $
��Bold"�6&   $��Verdana)�68   $
��Wrap=�5~      $ Center=�6,   SQ226 Sumulong Highway, Brgy. Mambugan Antipolo City, Rizal Philippines, 1870 asd f5�      �� 5�      -   5        $��lblCPNumber��5�      $��09675198479 | 0285718622��6+   $��-80,150,0,0q�5�      $	��180��6-   $��25��5�      $��10(�6$   $
��Bold"�62   $��Verdana)�6D   $ Center=�5�      �� 5�      -   5        $��Imgback��5�      $��1��6#   
 . $��Uniform=�6K   $��Center=�5�      $	��450��6-   $��/Resources/back.jpg��6<   5�      5�      5�      