using LINQPad.Extensibility.DataContext;
using System.Windows;

namespace NY.Dataverse.LINQPadDriver
{
    public partial class ConnectionDialog
    {
        private readonly IConnectionInfo _cxInfo;

        public ConnectionDialog(IConnectionInfo cxInfo)
        {
            _cxInfo = cxInfo;

            // ConnectionProperties is your view-model.
            DataContext = new ConnectionProperties(cxInfo);

            InitializeComponent();
        }

        public void ButtonOkOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
