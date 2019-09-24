
namespace LX.Plugin.SellableItem.StiboAttributes
{
    using System.Reflection;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.DoActions;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.EntityViews;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.Import;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.Import.Minion;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config =>
                config
                    .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                    {
                        c.Add<GetSellableItemStiboAttributeViewBlockForGlobalStiboAttributes>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForProductOverview>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForProductFeatures>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForProductMedia>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForProductVideoGallery>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForAdditionalResources>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForContentManageableAttributes>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForInstallationAndInstructions>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForNew>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForOtherSpecifications>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForParts>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForStyleSpecification>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForTechnicalSpecification>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForSellableItemFacets>().After<GetSellableItemDetailsViewBlock>();
                        c.Add<GetSellableItemStiboAttributeViewBlockForCategoryAttributes>().After<GetSellableItemDetailsViewBlock>();
                    })
                    .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                    {
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForGlobalStiboAttributes>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForProductOverview>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForProductFeatures>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForProductMedia>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForProductVideoGallery>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForAdditionalResources>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForContentManageableAttributes>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForInstallationAndInstructions>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForNew>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForOtherSpecifications>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForParts>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForStyleSpecification>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForTechnicalSpecification>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForSellableItemFacets>().After<InitializeEntityViewActionsBlock>();
                        c.Add<PopulateSellableItemStiboAttributesActionBlockForCategoryAttributes>().After<InitializeEntityViewActionsBlock>();
                    })
                    .ConfigurePipeline<IDoActionPipeline>(c =>
                    {
                        c.Add<DoActionEditSellableItemGlobalStiboAttributesBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemAdditionalResourcesBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemContentManageableAttributeBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemInstallationAndInstructionsBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemNewBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemOtherSpecificationsBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemPartsBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemProductFeaturesBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemProductMediaBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemProductOverviewBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemProductVideoGalleryBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemStyleSpecificationBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemTechnicalSpecificationBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemFacetsBlock>().After<ValidateEntityVersionBlock>();
                        c.Add<DoActionEditSellableItemCategoryAttributesBlock>().After<ValidateEntityVersionBlock>();
                    })
                     .AddPipeline<IIMportSellableItemsPipeline, IMportSellableItemsPipeline>(
                     configure =>
                     {
                         configure.Add<ValidateXMLFile>()
                         .Add<CatalogManager>()
                         .Add<SellableItemManager>()
                         .Add<ManageProductImportStatus>();
                     })
                     .AddPipeline<IIMportProductsMinionPipeline, IMportProductsMinionPipeline>(
                     configure =>
                     {
                         configure.Add<RunProductImportMinionBlock>();
                     })
                      .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>()));
            services.RegisterAllCommands(assembly);
        }
    }
}
