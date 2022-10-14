using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DAM.Core.Models.MapperModels.MapPairItems;

namespace DAM.UI.Helpers
{
    public class MapPairItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element)
            {
                if(item is SimpleMapPairItem)
                    return element.FindResource("SimpleMapPairItemDataTemplate") as DataTemplate;
                if(item is IgnoreMapPairItem)
                    return element.FindResource("IgnoreMapPairItemDataTemplate") as DataTemplate;
                if(item is OrderedMapPairItem)
                    return element.FindResource("OrderedMapPairItemDataTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
