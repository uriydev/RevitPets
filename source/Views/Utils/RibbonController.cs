using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Autodesk.Windows;
using Xceed.Wpf.AvalonDock.Controls;

namespace RevitPets.Views.Utils;

public class RibbonController
{
    private readonly System.Windows.Controls.Grid _rootGrid;
    private readonly FrameworkElement _internalToolPanel;
    private ContentPresenter _panelPresenter;
    
    public RibbonController()
    {
        var mainWindow = GetMainWindow();
        if (mainWindow is null) throw new InvalidOperationException("Revit main window not found");

        _internalToolPanel = VisualUtils.FindVisualChild<LayoutDocumentPaneGroupControl>(mainWindow, string.Empty);
        if (_internalToolPanel is null)
            throw new InvalidOperationException("Cannot find LayoutDocumentPaneGroupControl in Revit UI");

        _rootGrid = VisualUtils.FindVisualChild<System.Windows.Controls.Grid>(_internalToolPanel, string.Empty)
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
        _rootGrid.Children.Remove(_panelPresenter);
        _panelPresenter = null;

        const int panelRow = 2;
        if (_rootGrid.RowDefinitions.Count > panelRow)
        {
            bool isRowEmpty = true;
            foreach (UIElement child in _rootGrid.Children)
            {
                if (System.Windows.Controls.Grid.GetRow(child) == panelRow)
                {
                    isRowEmpty = false;
                    break;
                }
            }

            if (isRowEmpty)
            {
                _rootGrid.RowDefinitions.RemoveAt(panelRow);
            }
        }
    }
    
    private ContentPresenter CreateOptionsBar()
    {
        const int panelRow = 2;

        if (_rootGrid.RowDefinitions.Count <= panelRow)
        {
            _rootGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            });
        }

        var panelPresenter = new ContentPresenter();
        System.Windows.Controls.Grid.SetRow(panelPresenter, panelRow);
        _rootGrid.Children.Add(panelPresenter);
        
        return panelPresenter;
    }

    public double GetBarActualWidth()
    {
        return _panelPresenter?.ActualWidth ?? 0;
    }
}