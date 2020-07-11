using System.Windows;
using System.Windows.Controls;
using FocusApp;

namespace FocusGUI
{
    public class DataBaseTemplateSelector<TTarget> : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            
            if (!(item is IFocusDataBase<TTarget> entry) )
                return null;
            var template = new DataTemplate();
            template.VisualTree = new FrameworkElementFactory();
            return null;// template.;
        }
    }
}