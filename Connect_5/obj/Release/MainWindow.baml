   M S B A M L   `   `   `  ���� D  @Connect_5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Connect_5	Connect_5      Connect_5.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Connect_5     -     �Name$  wpfConnect_5�� � Title$ MiniGame Connect 5��$	��579��$��1134.2�� � Icon$ Images/favicon.png��$
��Auto=����	#FFF0EEEE � 
ResizeMode$ NoResize=� � Cursor$ Arrow�� � WindowStartupLocation$ CenterScreen=� � 	Resources  
  Brush2 �� Color��%   &	        �� $ 	#FFFB9B9B��.��]� .+�� -    �Name$ gridMain��$	��Top=�	 � ColumnDefinitions	 �� $
��281* ��� $
��550* ��� $
��297* �.+��� -   
 ��Name$
 image��$
��Left=�$	��144��$	��Top=�$	��246��$��Images/logo.png��$��10,0,0,0q��� -    ��Name$ lblCurrentPlayer��$
��Left=�$��
10,144,0,0q�$	��Top=�$��32��$	��148��$  ��$��Arial)�$��18.667(�$
��Bold"�$��Current Player:���� -    ��Name$ 
btnNewGame��$��New Game��$
��Left=�$��
10,296,0,0q�$	��Top=�$	��234��$��51��
�������$��Arial Black)�$��16(�����  �� ImageSource$ Images/btn_7.png���� -   $ btnExit��$
��Exit��$
��Left=�$��
10,473,0,0q�$	��Top=�$	��234��$��50��$��Arial Black)�$��16(����	#FF707070���� $ Images/btn_7.png���� -    ��Name$ canvasBoard��$��1��$
��Left=�$	��530��$��	10,10,0,0q�$	��Top=�$	��530��	� -    	�Name$ radTwoHuman��$��Two human plays��$
��Left=�$��
44,208,0,0q�$	��Top=�$��Times New Roman)�$��16(�	� -	   $ radHumanComputer��$��Human vs Computer��$
��Left=�$��
44,230,0,0q�$	��Top=�$��Times New Roman)�$��16(�	� -
   $ radHumanOnline��$��Human Online��$
��Left=�$��
44,252,0,0q�$	��Top=�$��Times New Roman)�$��16(�	� -   $ radComputerOnline��$��Computer Online��$
��Left=�$��
44,274,0,0q�$	��Top=�$��Times New Roman)�$��16(��� -    ��Name$ txtNickname��$
��Left=�$��22��$��
44,181,0,0q� �� TextWrapping$
 Wrap=�$��Bỏ đồ án��$	��Top=�$	��202��$��Times New Roman)�$��16(��� -   $ lblProgress��$	��0 %��$��185,149,37,369q�   Connect_5.Messager  -     Name$ mgChat��$��2��$
��Left=�$��	10,80,0,0q�$	��Top=�$	��460��$	��283�� J� RenderTransformOrigin$ 0.511,0.661&�