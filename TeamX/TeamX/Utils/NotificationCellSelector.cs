using System;
using System.Collections;
using System.Collections.Generic;
using TeamX.Models;
using Xamarin.Forms;

namespace TeamX.Utils
{
    public class NotificationCellSelector : DataTemplateSelector
    {
        private static readonly IDictionary<int, Type> TypeXCell = new Dictionary<int, Type>
        {
            {1,typeof(NotificationCell1)},
            {2,typeof(NotificationCell2)},
            {3,typeof(NotificationCell3)},
            {4,typeof(NotificationCell4)},
            {5,typeof(NotificationCell5)}
        };


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is ViewNotification vntf)) return null;

            var cell_type = TypeXCell[vntf.Ntf.Type];
            return new DataTemplate(cell_type);
        }
    }
}
