using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SRWYEditorAvalonia;

public partial class BundleNamesWindow : Window
{
    public BundleNamesWindow()
    {
        InitializeComponent(); 
        this.Closing += BundleNamesWindow_Closing;
    }

    private void BundleNamesWindow_Closing(object sender, WindowClosingEventArgs e)
    {
        e.Cancel = true; // Prevent the window from actually closing
        ((Window)sender).Hide(); // Hide the window instead
    }
}