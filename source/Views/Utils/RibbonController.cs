using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Autodesk.Windows;
using Xceed.Wpf.AvalonDock.Controls;

namespace RevitPets.Views.Utils;

public class RibbonController
{
    private readonly System.Windows.Controls.Grid RootGrid;
    private ContentPresenter _panelPresenter;
    private readonly FrameworkElement InternalToolPanel;
    
    public RibbonController()
    {
        var mainWindow = GetMainWindow();
        if (mainWindow is null) throw new InvalidOperationException("Revit main window not found");

        InternalToolPanel = VisualUtils.FindVisualChild<LayoutDocumentPaneGroupControl>(mainWindow, string.Empty);
        if (InternalToolPanel is null)
            throw new InvalidOperationException("Cannot find LayoutDocumentPaneGroupControl in Revit UI");

        RootGrid = VisualUtils.FindVisualChild<System.Windows.Controls.Grid>(InternalToolPanel, string.Empty)
                   ?? throw new InvalidOperationException("Cannot find Grid inside LayoutDocumentPaneGroupControl");
    }
    
    private Window GetMainWindow()
    {
        var hwnd = ComponentManager.ApplicationWindow;
        return hwnd != IntPtr.Zero ? HwndSource.FromHwnd(hwnd)?.RootVisual as Window : null;
    }
    
    public void ShowOptionsBar(FrameworkElement content)
    {
        if (_panelPresenter is not null)
        {
            _panelPresenter.Content = content;
            if (_panelPresenter.Visibility != System.Windows.Visibility.Visible)
            {
                _panelPresenter.Visibility = System.Windows.Visibility.Visible;
            }
            return;
        }

        _panelPresenter = CreateOptionsBar();
        _panelPresenter.Content = content;
    }
    
    public void RemoveOptionsBar()
    {
        if (_panelPresenter is null) return;
        
        _panelPresenter.Content = null; // Очистка контента перед удалением
        RootGrid.Children.Remove(_panelPresenter);
        _panelPresenter = null;

        const int panelRow = 2;
        if (RootGrid.RowDefinitions.Count > panelRow)
        {
            bool isRowEmpty = true;
            foreach (UIElement child in RootGrid.Children)
            {
                if (System.Windows.Controls.Grid.GetRow(child) == panelRow)
                {
                    isRowEmpty = false;
                    break;
                }
            }

            if (isRowEmpty)
            {
                RootGrid.RowDefinitions.RemoveAt(panelRow);
            }
        }
    }
    
    private ContentPresenter CreateOptionsBar()
    {
        const int panelRow = 2;

        if (RootGrid.RowDefinitions.Count <= panelRow)
        {
            RootGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            });
        }

        var panelPresenter = new ContentPresenter();
        System.Windows.Controls.Grid.SetRow(panelPresenter, panelRow);
        RootGrid.Children.Add(panelPresenter);
        
        return panelPresenter;
    }

    public double GetBarActualWidth()
    {
        return _panelPresenter?.ActualWidth ?? 0;
    }
}