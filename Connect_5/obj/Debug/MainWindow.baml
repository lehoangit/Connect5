   M S B A M L   `   `   `  ����D  @Connect_5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Connect_5	Connect_5  5   	       Connect_5.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Connect_5     5   	   -   5          �Name$  wpfConnect_5��5   	    � Title$ MiniGame Connect 5��5   	   $	��579��6$   $��1134.2��61    � Icon$ Images/favicon.png��6@   $
��Auto=�6Z   ���	#FFF0EEEE � 
ResizeMode$ NoResize=�6�    � Cursor$ Arrow��6�    � WindowStartupLocation$ CenterScreen=�6�    � 	Resources 5	       
  Brush2 �� Color��%-   &	        �� 5
   
   $ 	#FFFB9B9B��6)   5      .��5      ]� .+�5   
   � -   5         �Name$ gridMain��5   $   $	��Top=�6   	 � ColumnDefinitions	 5      �� 5      $
��281* �6#   �� 5      $
��550* �6#   �� 5      $
��297* �6#   .+�5      �� -   5        
 ��Name$
 image��5      $
��Left=�6#   $	��144��6>   $	��Top=�6K   $	��246��6c   $��Images/logo.png��6o   $��10,0,0,0q�6�   �� 5      -   5         ��Name$ lblCurrentPlayer��5      $
��Left=�6.   $��
10,144,0,0q�6I   $	��Top=�6]   $��32��6u   $	��148��6�   $  ��6�   $��Arial)�6�   $��18.667(�6�   $
��Bold"�6�   $��Current Player:��6�   �� 5      -   5         ��Name$ 
btnNewGame��5      $��New Game��6)   $
��Left=�6<   $��
10,296,0,0q�6W   $	��Top=�6k   $	��234��6�   $��51��6�   
�������$��Arial Black)�6�   $��16(�6�   ��5      �� 5       �� ImageSource$ Images/btn_7.png��6!   5      �� 5      -   5        $ btnExit��5      $
��Exit��6&   $
��Left=�65   $��
10,473,0,0q�6P   $	��Top=�6d   $	��234��6|   $��50��6�   $��Arial Black)�6�   $��16(�6�   ���	#FF707070��5      �� 5      $ Images/btn_7.png��6!   5      �� 5       -   5         ��Name$ canvasBoard��5       $��1��6*   $
��Left=�6:   $	��530��6U   $��	10,10,0,0q�6b   $	��Top=�6u   $	��530��6�   	� 5!      -   5         	�Name$ radTwoHuman��5!      $��Two human plays��6/   $
��Left=�6I   $��
44,208,0,0q�6d   $	��Top=�6x   $��Times New Roman)�6�   $��16(�6�   	� 5"      -	   5        $ radHumanComputer��5"      $��Human vs Computer��64   $
��Left=�6P   $��
44,230,0,0q�6k   $	��Top=�6   $��Times New Roman)�6�   $��16(�6�   	� 5#      -
   5        $ radHumanOnline��5#      $��Human Online��62   $
��Left=�6I   $��
44,252,0,0q�6d   $	��Top=�6x   $��Times New Roman)�6�   $��16(�6�   	� 5$      -   5        $ radComputerOnline��5$      $��Computer Online��65   $
��Left=�6O   $��
44,274,0,0q�6j   $	��Top=�6~   $��Times New Roman)�6�   $��16(�6�   �� 5%      -   5         ��Name$ txtNickname��5%      $
��Left=�6+   $��22��6F   $��
44,181,0,0q�6R    �� TextWrapping$
 Wrap=�6f   $��Bỏ đồ án��6z   $	��Top=�6�   $	��202��6�   $��Times New Roman)�6�   $��16(�6�   �� 5&      -   5        $ lblProgress��5&      $	��0 %��6)   $��185,149,37,369q�67   6O      Connect_5.Messager  5'      -   5          Name$ mgChat��5'      $��2��6-   $
��Left=�6=   $��	10,10,0,0q�6X   $	��Top=�6k   $	��469��6�   $	��283��6�    J� RenderTransformOrigin$ 0.511,0.661&�6�   5(      5)      5*      