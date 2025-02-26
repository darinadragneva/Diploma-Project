using iText.Kernel.Pdf;
using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Report1.xaml
    /// </summary>
    public partial class Report1 : Window
    {
        //private List<Payment> payments;

        public Report1()
        {
            InitializeComponent();
            pdfViewer1.Load("C:\\UNIVERSITY\\Report\\Report1.pdf");
        }
    }
}
