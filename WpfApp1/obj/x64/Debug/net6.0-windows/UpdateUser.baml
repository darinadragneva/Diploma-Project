   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.UpdateUser   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	   -   5          � Title$	  VSK��5   	   $	��540��6   $	��960��6"    � WindowStartupLocation$ CenterScreen=�6.   .��5	      � .+�5
   
   �� $����C:\UNIVERSITY\4 kurs\2 semestur\Дипломна работа\МОЯТА СЛАДУРСКА ДИПЛОМНА РАБОТА\WpfApp1\WpfApp1\money_background.jpg��6   $��UniformToFill=�6�   �� 5   
   -   5         ��Name$ label2��5      
����·�$(��"Потребителско име:��6:   $
��Left=�6W   $��
63,128,0,0q�6r   $	��Top=�6�   $��34��6�   $	��170��6�   $��16(�6�   �� 5   
   -   5         ��Name$ textBox1��5      
����·�$��14(�6>   $
��Left=�6L   $��245,130,0,0q�6g   $�� ��6|    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label3��5      
����·�$��Нова парола:��6:   $
��Left=�6Q   $��
63,208,0,0q�6l   $	��Top=�6�    J� RenderTransformOrigin$ 0.058,0.225&�6�   $��34��6�   $	��170��6�   $��16(�6�   G� 5   
   -   5         G�Name$ passwordBox1��5      
����·� G� PasswordChar$ *��6F   $��14(�6W   $
��Left=�6e   $��245,210,0,0q�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label4��5      
����·�$&�� Повтори паролата:��6:   $
��Left=�6V   $��
63,247,0,0q�6q   $	��Top=�6�   $��36��6�   $	��170��6�   $��16(�6�   G� 5   
   -   5        $ passwordBox2��5      
����·�$ *��6F   $��14(�6W   $
��Left=�6e   $��245,250,0,0q�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label5��5      
����·�$��Имейл:��6:   $
��Left=�6K   $��
63,287,0,0q�6f   $	��Top=�6z   $��36��6�   $	��170��6�   $��16(�6�   �� 5   
   -	   5        $ textBox2��5      $��14(�6$   
����·�$
��Left=�6L   $��245,290,0,0q�6g   $�� ��6|   $
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -
   5        $ label6��5      $#��Дата на раждане:��6    
����·�$
��Left=�6U   $��498,128,0,0q�6p   $	��Top=�6�   $��34��6�   $	��170��6�   $��16(�6�   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ dateTimePicker1��5      
����·�$��14(�6H   $
��Left=�6V   $��686,130,0,0q�6q   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label7��5      $��Телефон:��6    
����·�$
��Left=�6M   $��498,168,0,0q�6h   $	��Top=�6}   $��35��6�   $	��170��6�   $��16(�6�   �� 5   
   -   5        $ textBox3��5      $��14(�6$   
����·�$
��Left=�6L   $��686,170,0,0q�6g   $�� ��6|   $
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label8��5      $��Адрес:��6    
����·�$
��Left=�6K   $��498,208,0,0q�6f   $	��Top=�6{   $��34��6�   $	��170��6�   $��16(�6�   �� 5   
   -   5        $ textBox4��5      $��14(�6$   
����·�$
��Left=�6L   $��686,210,0,0q�6g   $�� ��6|   $
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label9��5      $��Име:��6    
����·�$
��Left=�6I   $��	498,0,0,0q�6d   $��Center=�6w   $��34��6�   $	��170��6�   $��16(�6�   �� 5   
   -   5        $ textBox5��5      $��14(�6$   
����·�$
��Left=�6L   $��686,250,0,0q�6g   $�� ��6|   $
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label10��5      $��Фамилия:��6!   
����·�$
��Left=�6N   $��498,288,0,0q�6i   $	��Top=�6~   $��34��6�   $	��170��6�   $��16(�6�   �� 5   
   -   5        $ textBox6��5      $��14(�6$   
����·�$
��Left=�6L   $��686,290,0,0q�6g   $�� ��6|   $
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   �� 5   
   -   5        $ label11��5      $��Позиция:��6!   
����·�$
��Left=�6N   $��498,326,0,0q�6i   $	��Top=�6~   $��16(�6�   $��35��6�   $	��170��6�   �� 5   
   -   5        	 ��Name$	 	comboBox1��5      
����·�$��14(�6b   $
��Left=�6p   $��686,330,0,0q�6�   $	��Top=�6�   $	��200��6�   $��30��6�   ��6&   �� *	DataItems+6�   �� 5   
   -   5        
 ��Name$
 button1��5      
����·�$��Обнови��6<   $��Center=�6M   $��39��6j   $��	0,429,0,0q�6v   $	��Top=�6�   $	��138��6�   $��18(�6�   6�   �� 5    
   -   5        $ label3_Copy��5       
����·�$ ��Текуща парола:��6?   $
��Left=�6X   $��
63,168,0,0q�6s   $	��Top=�6�   $ 0.058,0.225&�6�   $��34��6�   $	��170��6�   $��16(�6�   G� 5!   
   -   5        $ passwordBox3��5!      
����·�$ *��6F   $��14(�6W   $
��Left=�6e   $��245,170,0,0q�6�   $	��Top=�6�   $	��200��6�   $��30��6�   6�   5#      5$      