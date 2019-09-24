using LX.Commerce.Extensions.Annotation;

using LX.Plugin.SellableItem.StiboAttributes.Policies;
using Serilog;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.EntityViews
{
    public class AddSellableItemProperties
    {
        public static void AddPropertiesToViewWithSection<T>(EntityView entityView, T component, bool isReadOnly, string name, object rawValue, string displayName)
        {
            entityView.Properties.Add
                (
                    new ViewProperty
                    {
                        Name = name,
                        RawValue = rawValue,
                        IsReadOnly = isReadOnly,
                        IsRequired = false,
                        DisplayName = displayName
                    }
                  );
        }
        public static void AddLinkPropertiesToViewWithSection<T>(EntityView entityView, T component, bool isReadOnly, string name, object rawValue, string value, string displayName, string uiType)
        {
            entityView.Properties.Add(
               new ViewProperty
               {
                   Name = name,
                   IsHidden = false,
                   IsReadOnly = isReadOnly,
                   UiType = uiType,
                   OriginalType = uiType,
                   RawValue = rawValue,
                   DisplayName = displayName
               });
        }
    }
}
