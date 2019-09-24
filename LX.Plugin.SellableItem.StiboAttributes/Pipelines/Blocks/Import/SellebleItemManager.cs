

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.Import
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Sitecore.Commerce.Core.Commands;
    using LX.Plugin.SellableItem.StiboAttributes.Components;
    using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
    using System.Text.RegularExpressions;
    using Sitecore.Commerce.Plugin.BusinessUsers;
    using System.Reflection;
    using Sitecore.Commerce.Plugin.Management;
    using Sitecore.Services.Core.Model;
    using Sitecore.Commerce.Plugin.Pricing;
    using System.Net.Mail;
    using Sitecore.Collections;

    [PipelineDisplayName(ImportSellableItemsConstants.Pipelines.Blocks.SellableItemManager)]
    public class SellableItemManager : PipelineBlock<ImportSellableItemPipelineArgument, ImportSellableItemPipelineArgument, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// Gets or sets the commander.
        /// </summary>
        /// <value>
        /// The commander.
        /// </value>
        protected CommerceCommander commander { get; set; }
        private readonly FindEntityCommand _findEntityCommand;
        private readonly CreateSellableItemCommand _createSellableItemCommand;
        private readonly IFindEntityPipeline _findEntityPipeline;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly ICreateSellableItemPipeline _createSellableItemPipeline;
        private readonly EditSellableItemPipeline _editSellableItemPipeline;
        private readonly AssociateSellableItemToParentCommand _associateSellableItemToParentCommand;
        private readonly AssociateCategoryToParentCommand _associateCategoryToParentCommand;
        private readonly CreateCategoryCommand _createCategoryCommand;
        private readonly LocalizeEntityPropertyCommand _localizeEntityPropertyCommand;
        private readonly PersistEntityPipeline _persistEntityPipeline;
        private readonly EditCategoryCommand _editCategoryCommand;
        private string productImportStatus;
        List<string> productImportStatusList = new List<string>();
        List<string> productSuccessList = new List<string>(); List<string> productFailureList = new List<string>();
        List<string> productUpdateList = new List<string>(); List<string> variantUpdateList = new List<string>();
        List<string> variantSuccessList = new List<string>(); List<string> variantFailureList = new List<string>();
        List<string> categorySuccessList = new List<string>(); List<string> categoryFailureList = new List<string>();

        public SellableItemManager(CommerceCommander commander, IFindEntityPipeline findEntityPipeline, IDoesEntityExistPipeline doesEntityExistPipeline, CreateCategoryCommand createCategoryCommand, ICreateSellableItemPipeline createSellableItemPipeline, EditSellableItemPipeline editSellableItemPipeline, AssociateSellableItemToParentCommand associateSellableItemToParentCommand, FindEntityCommand findEntityCommand, CreateSellableItemCommand createSellableItemCommand, LocalizeEntityPropertyCommand localizeEntityPropertyCommand, PersistEntityPipeline persistEntityPipeline, AssociateCategoryToParentCommand associateCategoryToParentCommand, EditCategoryCommand editCategoryCommand)
        {
            this.commander = commander;
            this._findEntityPipeline = findEntityPipeline;
            this._createCategoryCommand = createCategoryCommand;
            this._doesEntityExistPipeline = doesEntityExistPipeline;
            this._createSellableItemPipeline = createSellableItemPipeline;
            this._editSellableItemPipeline = editSellableItemPipeline;
            this._associateCategoryToParentCommand = associateCategoryToParentCommand;
            this._associateSellableItemToParentCommand = associateSellableItemToParentCommand;
            this._findEntityCommand = findEntityCommand;
            this._createSellableItemCommand = createSellableItemCommand;
            this._localizeEntityPropertyCommand = localizeEntityPropertyCommand;
            this._persistEntityPipeline = persistEntityPipeline;
            this._editCategoryCommand = editCategoryCommand;
        }

        public override async Task<ImportSellableItemPipelineArgument> Run(ImportSellableItemPipelineArgument arg, CommercePipelineExecutionContext context)
        {
            XDocument document = XDocument.Load(arg.XMlFilePath);
            var catalog = arg.Catalog;

            // To Do : Active Code & Discountinued Filter
            //Create Seperate Pipiline for Attchment
            //Create Seperate Pipeline For NotActive Products to update data if it is already created
            var selectedProducts = document.Descendants("Product").Where(r => r.Attribute("UserTypeID").Value == "Product");

            var selectedCategories = document.Descendants("Classifications");
            var language = GetCurrentLanguageItem(context.CommerceContext, document);

            if (selectedProducts == null || selectedCategories == null || language == null || language.Result == null) return null;

            var sections = GetStiboAttributeSectionMapping(context, language.Result);
            var statusMappingItems = GetProductStatusMapping(context, language.Result);

            foreach (var productItem in selectedProducts)
            {
                productImportStatus = string.Empty;

                if (productItem.HasElements)
                {
                    var productId = productItem.Attribute("ID").Value;
                    if (!string.IsNullOrWhiteSpace(productId))
                    {
                        //Category Creation by finding its mapping
                        var categoryList = await CreateCategoryStructure(productItem, selectedCategories, context, catalog, language.Result);
                        if (categoryList != null && categoryList.Count == 3)
                        {
                            var sellableItemId = GenerateFullSellableItemId(productId);
                            //Try to find given sellable item
                            SellableItem sellableEntity = await _findEntityCommand.Process(context.CommerceContext, typeof(SellableItem), sellableItemId) as SellableItem;

                            var variants = from variant in productItem.Descendants("Product").Where(r => (string)r.Attribute("UserTypeID").Value == "SKU") select variant;

                            var attributes = productItem.Descendants("Values").Elements("Value");

                            var productStatus = GetProductStaus(context, language.Result, attributes, statusMappingItems);

                            //TO DO: Field Mapping will come from Master List which is created by Daivagna
                            if (sellableEntity == null)
                            {
                                var productElement = productItem.Element("Values").Descendants().Where(x => x.HasAttributes);

                                string productName = string.Empty;

                                if (attributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MaterialDescriptionMarketing)) != null && productStatus != null)
                                {
                                    if (productStatus.Status.Equals(Models.ProductContsants.Active) || productStatus.Status.Equals(Models.ProductContsants.Discontinued))
                                    {
                                        productName = attributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MaterialDescriptionMarketing))?.Value;

                                        var Components = new List<Component>
                                                    {
                                                        new CatalogsComponent
                                                        {
                                                            ChildComponents = new List<Component>
                                                            {
                                                                new CatalogComponent
                                                                {
                                                                    Name = catalog.Name, Id = catalog.Id
                                                                }
                                                            }
                                                        },
                                                        new ProductOverview
                                                        {
                                                            Name = "ProductOverview", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "ProductOverview"
                                                        },
                                                        new CategoryAttributes
                                                        {
                                                            Name = "CategoryAttributes", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "CategoryAttributes"
                                                        },
                                                        new SellableItemFacets
                                                        {
                                                            Name = "SellableItemFacets", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "SellableItemFacets"
                                                        },
                                                        new ProductFeatures
                                                        {
                                                            Name = "ProductFeatures", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "ProductFeatures"
                                                        },
                                                        new AdditionalResources
                                                        {
                                                            Name = "AdditionalResources", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "AdditionalResources"
                                                        },
                                                        new GlobalStiboAttributes
                                                        {
                                                             Name = "GlobalStiboAttributes", Id = System.Guid.NewGuid().ToString("N"),
                                                             Comments = "GlobalStiboAttributes"
                                                        },
                                                        new InstallationAndInstructions
                                                        {
                                                            Name = "InstallationAndInstructions", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "InstallationAndInstructions"
                                                        },
                                                        new New
                                                        {
                                                            Name = "New", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "New"
                                                        },
                                                        new OtherSpecifications
                                                        {
                                                            Name = "OtherSpecifications", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "OtherSpecifications"
                                                        },
                                                        new Parts
                                                        {
                                                            Name = "Parts", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "Parts"
                                                        },
                                                        new ProductMedia
                                                        {
                                                            Name = "ProductMedia", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "ProductMedia"
                                                        },
                                                        new ProductVideoGallery
                                                        {
                                                            Name = "ProductVideoGallery", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "ProductVideoGallery"
                                                        },
                                                        new StyleSpecification
                                                        {
                                                            Name = "StyleSpecification", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "StyleSpecification"
                                                        },
                                                        new TechnicalSpecification
                                                        {
                                                            Name = "TechnicalSpecification", Id = System.Guid.NewGuid().ToString("N"),
                                                            Comments = "TechnicalSpecification"
                                                        },
                                                         new ContentManageableAttributes
                                                        {
                                                             Name = "ContentManageableAttributes", Id = System.Guid.NewGuid().ToString("N"),
                                                             Comments = "ContentManageableAttributes"
                                                        },
                                                        };

                                        var product = new SellableItem(Components, new List<Policy>())
                                        {
                                            Id = productElement.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MfgProductNumberSAP))?.Value,
                                            Name = Regex.Replace(productName, @"[^0-9a-zA-Z]+", " "),
                                            DisplayName = Regex.Replace(productName, @"[^0-9a-zA-Z]+", " "),
                                            Description = Regex.Replace(productName, @"[^0-9a-zA-Z]+", " "),
                                            ProductId = productElement.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MfgProductNumberSAP))?.Value,
                                            Brand = productElement.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.Brand))?.Value,
                                        };

                                        var sellableItem = await CreateSellableItemByCommand(product, catalog, categoryList.ElementAt(0), categoryList.ElementAt(1), categoryList.ElementAt(2), context, catalog.Name, productItem, variants, sections, language.Result, productStatus, statusMappingItems);

                                        //TO DO: Need to apply Clear cache every fifth iteration:https://doc.sitecore.com/developers/91/sitecore-experience-commerce/en/optimize-//the-commercecontext-cache-for-a-long-running-process.html
                                        //Refresh Commerce Cache
                                        context.CommerceContext = new CommerceContext(context.CommerceContext.Logger, context.CommerceContext.TelemetryClient)
                                        {
                                            GlobalEnvironment = context.CommerceContext.GlobalEnvironment,
                                            Environment = context.CommerceContext.Environment,
                                            Headers = context.CommerceContext.Headers
                                        };
                                    }
                                    else
                                    {
                                        InActiveProductMessage(productElement.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MfgProductNumberSAP))?.Value, attributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MaterialDescriptionMarketing))?.Value);
                                    }
                                }
                            }
                            else
                            {
                                context.Logger.LogInformation($"ProductImport: SellableItem found  _findEntityPipeline, catalog: {sellableEntity.Name}");

                                await AttributeMappingForSellableItem(context, sellableEntity, productItem, sections, Models.ProductContsants.ProductLevelAttribute, language.Result);

                                await MapProductStatusForSellableItem(context, sellableEntity, productStatus, language.Result);

                                await CreateVariants(context.CommerceContext, sellableEntity, variants, sections, language.Result, categoryList.ElementAt(1), categoryList.ElementAt(2), statusMappingItems);
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(productImportStatus))
                {
                    string startmessage = "Issues while imporing product: " + productItem.Attribute("ID").Value + System.Environment.NewLine;
                    productImportStatusList.Add(string.Format("{0}{1}{2}", startmessage, productImportStatus, System.Environment.NewLine));
                }
            }
            context.Logger.LogInformation("*****ProductImport:Total Product Created: " + productSuccessList.Count());
            context.Logger.LogInformation("*****ProductImport: Total Variant Created: " + variantSuccessList.Count());
            //TO DO: Need to apply Clear cache every fifth iteration: https://doc.sitecore.com/developers/91/sitecore-experience-commerce/en/optimize-the-commercecontext-cache-for-a-long-running-process.html
            //Refresh Commerce Cache
            context.CommerceContext = new CommerceContext(context.CommerceContext.Logger, context.CommerceContext.TelemetryClient)
            {
                GlobalEnvironment = context.CommerceContext.GlobalEnvironment,
                Environment = context.CommerceContext.Environment,
                Headers = context.CommerceContext.Headers
            };
            await AttachReplacemenetData(selectedProducts, context);
            arg.ProductImportStatusList = productImportStatusList;
            arg.ProductSuccessList = productSuccessList; arg.CategorySuccessList = categorySuccessList; arg.VariantSuccessList = variantSuccessList;
            arg.CategoryFailureList = categoryFailureList; arg.ProductFailureList = productFailureList; arg.VariantFailureList = variantFailureList;
            arg.ProductUpdateList = productUpdateList; arg.VariantUpdateList = variantUpdateList;

            if (!(productFailureList.Count() > 0 || categoryFailureList.Count() > 0 || variantFailureList.Count() > 0))
                arg.FileImportSuccess = true;
            return arg;
        }

        private Models.ProductStaus GetProductStaus(CommercePipelineExecutionContext context, string language, IEnumerable<XElement> productAttributes, Task<List<Models.ProductStaus>> statusMappingItems)
        {
            if (productAttributes == null && statusMappingItems == null && productAttributes.Count() == 0 && statusMappingItems.Result == null) return null;

            if (productAttributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals("SAP_SKU_Status_SAP")) != null)
            {
                var element = productAttributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals("SAP_SKU_Status_SAP"));
                if (element.HasAttributes && element.Attribute("ID") != null && !string.IsNullOrWhiteSpace(element.Attribute("ID").Value))
                {
                    return statusMappingItems.Result.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Status) && x.Code.Equals(element.Attribute("ID").Value));
                }
            }
            return null;
        }

        private async Task AttachReplacemenetData(IEnumerable<XElement> selectedProducts, CommercePipelineExecutionContext context)
        {
            try
            {
                foreach (var productItem in selectedProducts)
                {
                    if (productItem.HasElements)
                    {
                        var productId = productItem.Attribute("ID").Value;

                        var variants = from variant in productItem.Descendants("Product").Where(r => (string)r.Attribute("UserTypeID").Value == "SKU") select variant;

                        if (!string.IsNullOrWhiteSpace(productId) && variants != null && variants.Count() > 0)
                        {
                            var selectedVariants = variants.Where(x => x.Element("ProductCrossReference") != null && x.Element("ProductCrossReference").Attribute("Type") != null && x.Element("ProductCrossReference").Attribute("ProductID") != null && !string.IsNullOrWhiteSpace(x.Element("ProductCrossReference").Attribute("ProductID").Value));

                            foreach (var variantItem in selectedVariants)
                            {
                                string variationId = variantItem.Attribute("ID").Value;

                                var references = variantItem.Elements("ProductCrossReference");

                                var sellableItemId = GenerateFullSellableItemId(productId);

                                // Try to find given sellable item
                                Sitecore.Commerce.Plugin.Catalog.SellableItem sellableItem = await _findEntityCommand.Process(context.CommerceContext, typeof(Sitecore.Commerce.Plugin.Catalog.SellableItem), sellableItemId) as Sitecore.Commerce.Plugin.Catalog.SellableItem;

                                if (sellableItem != null)
                                {
                                    var itemVariation = sellableItem.GetComponent<ItemVariationsComponent>().ChildComponents.FirstOrDefault(y => y.Id == variationId) as ItemVariationComponent;

                                    if (itemVariation != null)
                                    {
                                        var component = itemVariation.GetComponent<Parts>();

                                        var material = MapMaterials(references, Models.ProductContsants.ReplacementMaterial, selectedProducts, context);

                                        if (material != null && material.Result != null)
                                        {
                                            if (material.Result.MaterialURL != null && material.Result.MaterialId != null && material.Result.MaterialURL.Count() != 0 && material.Result.MaterialId.Count() != 0)
                                            {
                                                component.Replacement_Material = string.Join("|", material.Result.MaterialId.ToArray());
                                                component.Replacement_Material_URL = string.Join("|", material.Result.MaterialURL.ToArray());
                                            }
                                        }

                                        var parts = MapMaterials(references, Models.ProductContsants.ReplacementParts, selectedProducts, context);

                                        if (parts != null && parts.Result != null)
                                        {
                                            if (parts.Result.MaterialURL != null && parts.Result.MaterialId != null && parts.Result.MaterialURL.Count() != 0 && parts.Result.MaterialId.Count() != 0)
                                            {
                                                component.Replacement_Parts = string.Join("|", parts.Result.MaterialId.ToArray());
                                                component.Replacement_Parts_URL = string.Join("|", parts.Result.MaterialURL.ToArray());
                                            }
                                        }

                                        itemVariation.SetComponent(component);

                                        await this._persistEntityPipeline.Run(new PersistEntityArgument(sellableItem), context);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogInformation($"ProductImport: Error while attaching replacement material" + ex);
            }
        }

        public async Task<Models.ReplacementMaterial> MapMaterials(IEnumerable<XElement> references, string replacementOptions, IEnumerable<XElement> selectedProducts, CommercePipelineExecutionContext context)
        {
            Models.ReplacementMaterial material = new Models.ReplacementMaterial();
            List<string> ids = new List<string>(); List<string> urls = new List<string>();
            foreach (var reference in references)
            {
                if (reference.Attribute("Type").Value == replacementOptions && !string.IsNullOrWhiteSpace(reference.Attribute("ProductID").Value))
                {
                    string materialId = reference.Attribute("ProductID").Value;

                    //Try to find Material Item from XML File
                    var materialItemVariant = selectedProducts.Elements("Product").Where(x => x.HasAttributes && x.Attribute("ID") != null && x.Attribute("ID").Value.Equals(materialId) && x.Attribute("UserTypeID") != null && x.Attribute("UserTypeID").Value.Equals("SKU"));

                    var materialItemProduct = materialItemVariant.Ancestors("Product").Where(x => x.HasAttributes && x.Attribute("ID") != null && x.Attribute("UserTypeID") != null && x.Attribute("UserTypeID").Value.Equals("Product"));

                    if (materialItemProduct != null && materialItemProduct.Count() != 0)
                    {
                        var materialItemId = GenerateFullSellableItemId(materialItemProduct.FirstOrDefault().Attribute("ID").Value);

                        Sitecore.Commerce.Plugin.Catalog.SellableItem materialItem = await _findEntityCommand.Process(context.CommerceContext, typeof(Sitecore.Commerce.Plugin.Catalog.SellableItem), materialItemId) as Sitecore.Commerce.Plugin.Catalog.SellableItem;

                        if (materialItem != null)
                        {
                            var materialVariant = materialItem.GetComponent<ItemVariationsComponent>().ChildComponents.FirstOrDefault(y => y.Id == materialId) as ItemVariationComponent;

                            if (materialVariant != null)
                            {
                                if (!string.IsNullOrWhiteSpace(materialItem.Id) && !string.IsNullOrWhiteSpace(materialVariant.Id))
                                {
                                    urls.Add(string.Format(Models.ProductContsants.VariantURL, materialItem.Id, materialVariant.Id, materialVariant.Id, materialItem.ProductId));
                                    ids.Add(materialVariant.Id + "_" + materialItem.ProductId);
                                }
                            }
                        }
                    }
                }
            }
            material.MaterialId = ids; material.MaterialURL = urls;
            return material;
        }

        private async Task<List<Category>> CreateCategoryStructure(XElement product, IEnumerable<XElement> categoryElements, CommercePipelineExecutionContext context, Catalog catalog, string language)
        {
            var categoryMapping = product.Descendants("ClassificationReference").FirstOrDefault(r => r.Attribute("ClassificationID") != null);
            List<Category> categoryList = new List<Category>();
            Category LWT1 = null, LWT2 = null, LWT3 = null;
            Models.CategoryMapping stiboSuperCategoryMapping = null, stiboCategoryMapping = null, stiboSubCategoryMapping = null;
            if (categoryMapping != null)
            {
                SitecoreConnectionManager connection = new SitecoreConnectionManager();
                var mapping = categoryMapping.Attribute("ClassificationID").Value;
                var subCategory = categoryElements.Descendants("Classification").Where(r => r.Attribute("ID").Value == mapping).FirstOrDefault();
                var category = subCategory.Ancestors("Classification").Where(r => r.Attribute("UserTypeID").Value == Models.ProductContsants.CategoryLevel).FirstOrDefault();
                var supercategory = subCategory.Ancestors("Classification").Where(r => r.Attribute("UserTypeID").Value == Models.ProductContsants.SuperCategoryLevel).FirstOrDefault();

                var productSuperCategories = await connection.GetItemsByPathAsync(context.CommerceContext, Models.ProductContsants.CategoryMappingPath, language);

                if (productSuperCategories != null && productSuperCategories.Count > 0)
                {
                    if (supercategory.HasAttributes && !string.IsNullOrWhiteSpace(supercategory.Attribute("ID").Value))
                    {
                        //Create Super Category by reading Configuration
                        stiboSuperCategoryMapping = await GetCategoryItem(productSuperCategories, catalog, Models.ProductContsants.SuperCategoryTemplateId, supercategory.Attribute("ID").Value, language, context);
                    }

                    if (category.HasAttributes && !string.IsNullOrWhiteSpace(category.Attribute("ID").Value) && stiboSuperCategoryMapping != null && stiboSuperCategoryMapping.Category != null)
                    {
                        var productCategories = await connection.GetItemsByPathAsync(context.CommerceContext, stiboSuperCategoryMapping.CategoryMappingPath, language);
                        LWT1 = stiboSuperCategoryMapping.Category;
                        //Create Category by reading Configuration
                        stiboCategoryMapping = await GetCategoryItem(productCategories, catalog, Models.ProductContsants.CategoryTemplateId, category.Attribute("ID").Value, language, context, LWT1.Id);
                    }
                    else
                    {
                        NotFoundCategoryMappingMessage(supercategory.Attribute("ID").Value, supercategory.Element("Name").Value, Models.ProductContsants.SuperCategoryLevel);
                    }

                    if (subCategory.HasAttributes && !string.IsNullOrWhiteSpace(subCategory.Attribute("ID").Value) && stiboCategoryMapping != null && stiboCategoryMapping.Category != null)
                    {

                        var productSubCategories = await connection.GetItemsByPathAsync(context.CommerceContext, stiboCategoryMapping.CategoryMappingPath, language);
                        LWT2 = stiboCategoryMapping.Category;
                        //Create Sub Category by reading Configuration
                        stiboSubCategoryMapping = await GetCategoryItem(productSubCategories, catalog, Models.ProductContsants.SubCategoryTemplateId, subCategory.Attribute("ID").Value, language, context, LWT2.Id);

                        if (stiboSubCategoryMapping != null && stiboSubCategoryMapping.Category != null)
                        {
                            LWT3 = stiboSubCategoryMapping.Category;
                        }
                        else
                        {
                            NotFoundCategoryMappingMessage(subCategory.Attribute("ID").Value, subCategory.Element("Name").Value, Models.ProductContsants.SubCategoryLevel);
                        }
                    }
                    else
                    {
                        NotFoundCategoryMappingMessage(category.Attribute("ID").Value, category.Element("Name").Value, Models.ProductContsants.CategoryLevel);
                    }
                }
                if (LWT1 != null && LWT2 != null && LWT3 != null)
                {
                    categoryList.Add(LWT1); categoryList.Add(LWT2); categoryList.Add(LWT3);
                }
            }
            return categoryList;
        }

        private async Task<Models.CategoryMapping> GetCategoryItem(List<ItemModel> productCategories, Catalog catalog, string templateId, string mappingValue, string language, CommercePipelineExecutionContext context, string parentCategoryId = null)
        {
            var categories = productCategories.Where(x => x.GetFieldValue("TemplateID").Equals(templateId));

            var categoryItems = categories.Select(x => GetCategoryObject(x, context.CommerceContext, language)).Where(x => x.Result != null && x.Result.ClassificationsItems != null && x.Result.ClassificationsItems.Count() > 0 && !string.IsNullOrWhiteSpace(x.Result.Commerce_Name) && !string.IsNullOrWhiteSpace(x.Result.ItemPath));

            foreach (var item in categoryItems)
            {
                var categotyMapping = item.Result.ClassificationsItems.SingleOrDefault(x => x.Stibo_ID.Equals(mappingValue));
                if (categotyMapping != null)
                {
                    Models.CategoryMapping mapping = new Models.CategoryMapping();
                    mapping.Category = await CreateOrGetCategory(context, catalog, item.Result.Commerce_Name, language, mappingValue, item.Result.ItemPath, templateId, item.Result.Commerce_Name, parentCategoryId);
                    mapping.CategoryMappingPath = item.Result.ItemPath;
                    return mapping;
                }
            }
            return null;
        }


        private async Task UpdateCategoryAttributes(Category category, string mappingPath, string templateId, CommercePipelineExecutionContext context)
        {
            if (category != null && !string.IsNullOrWhiteSpace(templateId) && !string.IsNullOrWhiteSpace(mappingPath))
            {
                var attribute = category.GetComponent<CategoryAttributes>();
                attribute.SitecoreMappingPath = mappingPath;
                if (templateId == Models.ProductContsants.SuperCategoryTemplateId)
                    attribute.ClassificationType = Models.ProductContsants.SuperCategoryLevel;
                if (templateId == Models.ProductContsants.SubCategoryTemplateId)
                    attribute.ClassificationType = Models.ProductContsants.SubCategoryLevel;
                if (templateId == Models.ProductContsants.CategoryTemplateId)
                    attribute.ClassificationType = Models.ProductContsants.CategoryLevel;
                category.SetComponent(attribute);
                await this._persistEntityPipeline.Run(new PersistEntityArgument(category), context);
            }
        }

        public async Task<Models.Category> GetCategoryObject(ItemModel model, CommerceContext commerceContext, string language)
        {
            Models.Category category = new Models.Category();
            var name = model.GetFieldValue(Models.ProductContsants.CommerceNameField).ToString();
            var classification = model.GetFieldValue(Models.ProductContsants.StiboClassificationsField).ToString();
            var itempath = model.GetFieldValue("ItemPath").ToString();
            if (!string.IsNullOrWhiteSpace(name))
            {
                category.Commerce_Name = name;
            }
            if (!string.IsNullOrWhiteSpace(itempath))
            {
                category.ItemPath = itempath;
            }
            if (!string.IsNullOrWhiteSpace(classification))
            {
                category.StiboClassifications = classification;
                var items = classification.Split('|');
                if (items != null && items.Count() > 0)
                {
                    List<Models.ProductClassification> classificationList = new List<Models.ProductClassification>();
                    foreach (var item in items)
                    {
                        SitecoreConnectionManager connection = new SitecoreConnectionManager();
                        var classificationItem = await connection.GetItemByPathAsync(commerceContext, item, language);
                        if (classificationItem != null)
                        {
                            var stiboName = classificationItem.GetFieldValue(Models.ProductContsants.StiboNameField).ToString();
                            var stiboId = classificationItem.GetFieldValue(Models.ProductContsants.StiboIdField).ToString();
                            var level = classificationItem.GetFieldValue(Models.ProductContsants.LevelField).ToString();
                            if (!string.IsNullOrWhiteSpace(stiboName) && !string.IsNullOrWhiteSpace(stiboId) && !string.IsNullOrWhiteSpace(level))
                            {
                                Models.ProductClassification productClassification = new Models.ProductClassification();
                                productClassification.Level = level;
                                productClassification.Stibo_ID = stiboId;
                                productClassification.Stibo_Name = stiboName;
                                classificationList.Add(productClassification);
                            }
                        }
                    }
                    category.ClassificationsItems = classificationList;
                }
            }
            return category;
        }
        /// <summary>
        /// Get Language for Import using sitecore configuration
        /// </summary>
        /// <param name="commerceContext"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task<string> GetCurrentLanguageItem(CommerceContext commerceContext, XDocument document)
        {
            if (document != null)
            {
                if (document.Element("STEP-ProductInformation").HasAttributes && document.Element("STEP-ProductInformation").Attribute("ContextID") != null && !string.IsNullOrWhiteSpace(document.Element("STEP-ProductInformation").Attribute("ContextID").Value))
                {
                    SitecoreConnectionManager connection = new SitecoreConnectionManager();
                    var stiboLanguages = await connection.GetItemChildrenAsync(commerceContext, Models.ProductContsants.StiboLanguages);
                    if (stiboLanguages != null && stiboLanguages.Count() > 0)
                    {
                        var languageItem = stiboLanguages.SingleOrDefault(x => x.GetFieldValue(Models.ProductContsants.ContextId) != null && x.GetFieldValue(Models.ProductContsants.ContextId).ToString().Equals(document.Element("STEP-ProductInformation").Attribute("ContextID").Value));
                        if (languageItem != null && languageItem.GetFieldValue(Models.ProductContsants.LanguageKey) != null)
                            return languageItem.GetFieldValue(Models.ProductContsants.LanguageKey).ToString();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Create Category in Business Tool
        /// </summary>
        /// <param name="context"></param>
        /// <param name="catalog"></param>
        /// <param name="categoryName"></param>
        /// <param name="categoryDisplayName"></param>
        /// <param name="parentCategoryId"></param>
        /// <returns></returns>
        private async Task<Category> CreateOrGetCategory(CommercePipelineExecutionContext context, Catalog catalog, string categoryName, string language, string categotyMapping, string mappingPath, string templateId, string categoryDisplayName = null, string parentCategoryId = null)
        {
            try
            {
                string categoryId = GenerateFullCategoryId(catalog.Name, Regex.Replace(categoryName, @"[^a-zA-Z]+", " "));
                if (string.IsNullOrWhiteSpace(categoryDisplayName)) categoryDisplayName = categoryName;

                //Check if category with given name already exists before trying to create a new one
                Category category = await _findEntityCommand.Process(context.CommerceContext, typeof(Category), categoryId) as Category;

                if (category == null)
                {
                    context.Logger.LogInformation($"ProductImport:Category Not Found, result is nill _getCategoryCommand:{categoryId}");
                    context.Logger.LogInformation($"ProductImport:Creating Category: {categoryId}");
                    category = await _createCategoryCommand.Process(context.CommerceContext, catalog.Id, categoryName, categoryDisplayName, categoryDisplayName) as Category;

                    CategoryAttributes attribute = new CategoryAttributes();
                    attribute.Name = "CategoryAttributes"; attribute.Id = System.Guid.NewGuid().ToString("N"); attribute.Comments = "CategoryAttributes";
                    category.AddComponents(attribute);
                    await UpdateCategoryAttributes(category, mappingPath, templateId, context);

                    await LocalizeCategory(context, category, language, categoryDisplayName);

                    //TO DO: Need to apply Clear cache every fifth iteration: https://doc.sitecore.com/developers/91/sitecore-experience-commerce/en/optimize-the-commercecontext-cache-for-a-long-running-process.html
                    //Refresh Commerce Cache
                    context.CommerceContext = new CommerceContext(context.CommerceContext.Logger, context.CommerceContext.TelemetryClient)
                    {
                        GlobalEnvironment = context.CommerceContext.GlobalEnvironment,
                        Environment = context.CommerceContext.Environment,
                        Headers = context.CommerceContext.Headers
                    };

                    if (string.IsNullOrWhiteSpace(parentCategoryId))
                        await _associateCategoryToParentCommand.Process(context.CommerceContext, catalog.Id, catalog.Id, category.Id);
                    else
                        await _associateCategoryToParentCommand.Process(context.CommerceContext, catalog.Id, parentCategoryId, category.Id);
                    categorySuccessList.Add(categotyMapping);
                    return category;
                }
                else
                {
                    context.Logger.LogInformation($"ProductImport: Category found  _findEntityPipeline, catalog: {categoryName}");
                    // Only update category if it was created previously
                    await this._editCategoryCommand.Process(context.CommerceContext, category, categoryDisplayName, categoryDisplayName);
                    await LocalizeCategory(context, category, language, categoryDisplayName);
                }
                return category;
            }
            catch (Exception ex)
            {
                context.Logger.LogInformation($"ProductImport: Error during Category process" + ex.Message);
                return null;
            }
        }

        public async Task LocalizeCategory(CommercePipelineExecutionContext context, Category category, string language, string categoryDisplayName)
        {
            //Adding Localization For Category
            LocalizationEntity localizationEntity = await this._localizeEntityPropertyCommand.GetLocalizationEntity(context.CommerceContext, category);

            // adding localizations
            localizationEntity.AddOrUpdatePropertyValue("DisplayName", GetLocalizationValueOfProperty(categoryDisplayName, language));
            localizationEntity.AddOrUpdatePropertyValue("Description", GetLocalizationValueOfProperty(categoryDisplayName, language));

            // saving localization entity to database
            await this._persistEntityPipeline.Run(new PersistEntityArgument(localizationEntity), context);
        }

        /// <summary>
        /// Create Selleable Item in Business Tool
        /// </summary>
        private async Task<SellableItem> CreateSellableItemByCommand(SellableItem item, Catalog catalog, Category category1, Category category2, Category category3, CommercePipelineExecutionContext context, string catalogName, XElement productItem, IEnumerable<XElement> variants, Task<List<Models.StiboSection>> sections, string language, Models.ProductStaus productStatus, Task<List<Models.ProductStaus>> statusMappingItems)
        {
            try
            {
                //Create new Sellable Item, pass model properties as parameters
                var sellableEntity = await _createSellableItemCommand.Process(context.CommerceContext, item.Id, item.Name, item.DisplayName, item.Description);

                if (sellableEntity != null)
                {
                    productSuccessList.Add(sellableEntity.ProductId);

                    foreach (var policy in item.EntityPolicies)
                    {
                        if (sellableEntity.HasPolicy(policy.GetType()))
                        {
                            sellableEntity.RemovePolicy(policy.GetType());
                        }
                        sellableEntity.SetPolicy(policy);
                    }
                    sellableEntity.Brand = item.Brand;
                    sellableEntity.ParentCatalogList = catalog.SitecoreId.Replace("{", "").Replace("}", "");
                    string[] data = { category1.SitecoreId.Replace("{", "").Replace("}", ""), category2.SitecoreId.Replace("{", "").Replace("}", ""), category3.SitecoreId.Replace("{", "").Replace("}", "") };
                    sellableEntity.ParentCategoryList = String.Join("|", data);

                    foreach (var comp in item.EntityComponents)
                    {
                        if (sellableEntity.HasComponent(comp.GetType()))
                        {
                            sellableEntity.RemoveComponent(comp.GetType());
                        }
                        sellableEntity.SetComponent(comp);
                    }
                    //Set Category Attributes for SellableItem
                    if (sellableEntity.HasComponent<CategoryAttributes>())
                    {
                        var sellableItemCategoryAttribute = sellableEntity.GetComponent<CategoryAttributes>();
                        if (category3.HasComponent<CategoryAttributes>())
                        {
                            var categoryAttribute = category3.GetComponent<CategoryAttributes>();
                            sellableItemCategoryAttribute.ClassificationType = categoryAttribute.ClassificationType;
                            sellableItemCategoryAttribute.SitecoreMappingPath = categoryAttribute.SitecoreMappingPath;
                            sellableEntity.SetComponent(sellableItemCategoryAttribute);
                        }
                    }
                    await this._persistEntityPipeline.Run(new PersistEntityArgument(sellableEntity), context);
                    await AttributeMappingForSellableItem(context, sellableEntity, productItem, sections, Models.ProductContsants.ProductLevelAttribute, language);
                    await MapProductStatusForSellableItem(context, sellableEntity, productStatus, language);
                    await _editSellableItemPipeline.Run(sellableEntity, context);
                    await AssociateSellableItemToParent(sellableEntity, catalog, category3, context);
                    await CreateVariants(context.CommerceContext, sellableEntity, variants, sections, language, category2, category3, statusMappingItems);


                    //TO DO: Need to apply Clear cache every fifth iteration: https://doc.sitecore.com/developers/91/sitecore-experience-commerce/en/optimize-the-commercecontext-cache-for-a-long-running-process.html
                    //Refresh Commerce Cache
                    context.CommerceContext = new CommerceContext(context.CommerceContext.Logger, context.CommerceContext.TelemetryClient)
                    {
                        GlobalEnvironment = context.CommerceContext.GlobalEnvironment,
                        Environment = context.CommerceContext.Environment,
                        Headers = context.CommerceContext.Headers
                    };

                }
                return sellableEntity;
            }
            catch (Exception e)
            {
                context.Logger.LogInformation("ProductImport:Error While creating sellable Item" + e.Message);
                productFailureList.Add(item.ProductId);
            }
            return null;
        }

        private async Task<SellableItem> AttributeMappingForSellableItem(CommercePipelineExecutionContext context, SellableItem sellableItem, XElement productItem, Task<List<Models.StiboSection>> sections, string attributeLevel, string language)
        {
            var attributes = productItem.Element("Values").Descendants().Where(x => x.HasAttributes && x.Attribute("AttributeID") != null);

            LocalizationEntity localizationEntity = await this._localizeEntityPropertyCommand.GetLocalizationEntity(context.CommerceContext, sellableItem);

            string name = attributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MaterialDescriptionMarketing))?.Value;

            if (!string.IsNullOrWhiteSpace(name))
            {
                localizationEntity.AddOrUpdatePropertyValue("DisplayName", GetLocalizationValueOfProperty(Regex.Replace(name, @"[^0-9a-zA-Z]+", " "), language));
                localizationEntity.AddOrUpdatePropertyValue("Description", GetLocalizationValueOfProperty(Regex.Replace(name, @"[^0-9a-zA-Z]+", " "), language));
            }

            foreach (var attribute in attributes)
            {
                int sectionCount = 0;
                var attributeName = attribute.Attribute("AttributeID").Value;

                if (attributeName.ToLower().Equals(Models.ProductContsants.ListPrice) && !string.IsNullOrWhiteSpace(attribute.Value))
                {
                    if (sellableItem.HasPolicy<ListPricingPolicy>())
                    {
                        var pricePolicy = sellableItem.GetPolicy<ListPricingPolicy>();
                        pricePolicy.AddPrice(new Money("USD", Convert.ToDecimal(attribute.Value)));
                    }
                }

                foreach (var stiboSection in sections.Result)
                {
                    var section = stiboSection.StiboAttributeList.SingleOrDefault(x => !string.IsNullOrWhiteSpace(x.StiboName) &&
                     x.StiboName.ToLower().Equals(attributeName.ToLower()));

                    if (section != null)
                    {
                        if (section.AttributeLevel.Contains(attributeLevel))
                        {
                            var commerceComponent = sellableItem.EntityComponents.Where(x => x.GetType().Name.ToLower().Equals(stiboSection.Section_Name.ToLower())).FirstOrDefault();

                            if (commerceComponent != null)
                            {
                                var property = commerceComponent.GetType().GetProperty(section.StiboName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                                if (property != null)
                                {
                                    string attributeValue = string.Empty;
                                    if (section.ValueType == Models.ProductContsants.YesNoField && attribute.HasAttributes && !string.IsNullOrWhiteSpace(section.TransformedValue))
                                    {
                                        attributeValue = GetValueOfYesNoField(attribute, section);
                                    }

                                    else if (section.ValueType == Models.ProductContsants.MultiValueField || section.ValueType == Models.ProductContsants.LOVField || section.ValueType == Models.ProductContsants.MultiValueField && attribute.Descendants() != null && attribute.Descendants().Count() > 0)
                                    {
                                        attributeValue = string.Join("|", attribute.Descendants().ToList().Select(x => x.Value)).Trim();
                                    }

                                    else
                                    {
                                        attributeValue = attribute?.Value;
                                    }

                                    localizationEntity.AddOrUpdateComponentValue(stiboSection.Section_Name, commerceComponent.Id, section.StiboName, GetLocalizationValueOfProperty(Convert.ChangeType(attributeValue, property.PropertyType), language));

                                    // saving category entity to database
                                    await this._persistEntityPipeline.Run(new PersistEntityArgument(localizationEntity), context);
                                    break;
                                }
                                else
                                {
                                    InvalidStiboNameMessage(attributeName, sellableItem.ProductId, sellableItem.Name);
                                    break;
                                }
                            }
                            else
                            {
                                InvalidMappingMessage(attributeName, sellableItem.ProductId, sellableItem.Name);
                                break;
                            }
                        }
                        else
                        {
                            InvalidAttributeLevelMessage(attributeName, sellableItem.ProductId, sellableItem.Name);
                            break;
                        }
                    }
                    else
                    {
                        sectionCount++;
                        if (sectionCount == sections.Result.Count)
                            NotFoundAttributeMappingMessage(attributeName, sellableItem.ProductId, sellableItem.Name);
                    }
                }
            }
            productUpdateList.Add(sellableItem.ProductId);
            if (!string.IsNullOrWhiteSpace(productImportStatus)) productImportStatus += "------------------------------------------" + System.Environment.NewLine;
            return sellableItem;
        }

        private async Task MapProductStatusForSellableItem(CommercePipelineExecutionContext context, SellableItem sellableItem, Models.ProductStaus productStatus, string language)
        {
            if (productStatus != null && !string.IsNullOrWhiteSpace(productStatus.Code) && !string.IsNullOrWhiteSpace(productStatus.Status))
            {
                LocalizationEntity localizationEntity = await this._localizeEntityPropertyCommand.GetLocalizationEntity(context.CommerceContext, sellableItem);

                var globalStiboComponent = sellableItem.GetComponent<GlobalStiboAttributes>();
                var otherSpecifications = sellableItem.GetComponent<OtherSpecifications>();

                localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "Product_Status", GetLocalizationValueOfProperty(productStatus.Status, language));
                localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "SAP_SKU_Status_SAP", GetLocalizationValueOfProperty(productStatus.Code, language));

                if (productStatus.Status == Models.ProductContsants.Discontinued)
                {
                    localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "Is_Discontinued", GetLocalizationValueOfProperty(Convert.ChangeType("true", globalStiboComponent.Is_Discontinued.GetType()), language));
                }
                if (productStatus.Status == Models.ProductContsants.InActive)
                {
                    localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "Is_Discontinued", GetLocalizationValueOfProperty(Convert.ChangeType("false", globalStiboComponent.Is_Discontinued.GetType()), language));

                    localizationEntity.AddOrUpdateComponentValue(otherSpecifications.Name, otherSpecifications.Id, "Is_Sellable", GetLocalizationValueOfProperty(Convert.ChangeType("false", otherSpecifications.Is_Sellable.GetType()), language));
                }
                if (productStatus.Status == Models.ProductContsants.Active)
                {
                    localizationEntity.AddOrUpdateComponentValue(otherSpecifications.Name, otherSpecifications.Id, "Is_Sellable", GetLocalizationValueOfProperty(Convert.ChangeType("true", otherSpecifications.Is_Sellable.GetType()), language));
                }

                // saving localization entity to database
                await this._persistEntityPipeline.Run(new PersistEntityArgument(localizationEntity), context);
            }
        }

        private async Task MapProductStatusForVariant(CommercePipelineExecutionContext context, SellableItem sellableItem, Models.ProductStaus productStatus, string variantId, ItemVariationComponent itemVariationComponent, string language)
        {
            if (productStatus != null && !string.IsNullOrWhiteSpace(productStatus.Code) && !string.IsNullOrWhiteSpace(productStatus.Status))
            {
                LocalizationEntity localizationEntity = await this._localizeEntityPropertyCommand.GetLocalizationEntity(context.CommerceContext, sellableItem);

                var globalStiboComponent = itemVariationComponent.GetComponent<GlobalStiboAttributes>();
                var otherSpecifications = itemVariationComponent.GetComponent<OtherSpecifications>();

                if (productStatus.Status == Models.ProductContsants.Discontinued)
                {
                    localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "Is_Discontinued", GetLocalizationValueOfProperty(Convert.ChangeType("true", globalStiboComponent.Is_Discontinued.GetType()), language));
                    localizationEntity.AddOrUpdateComponentValue(otherSpecifications.Name, otherSpecifications.Id, "Is_Sellable", GetLocalizationValueOfProperty(Convert.ChangeType("false", otherSpecifications.Is_Sellable.GetType()), language));
                }

                if (productStatus.Status == Models.ProductContsants.InActive)
                {
                    localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "Is_Discontinued", GetLocalizationValueOfProperty(Convert.ChangeType("false", globalStiboComponent.Is_Discontinued.GetType()), language));
                    localizationEntity.AddOrUpdateComponentValue(otherSpecifications.Name, otherSpecifications.Id, "Is_Sellable", GetLocalizationValueOfProperty(Convert.ChangeType("false", otherSpecifications.Is_Sellable.GetType()), language));
                }
                if (productStatus.Status == Models.ProductContsants.Active)
                {
                    localizationEntity.AddOrUpdateComponentValue(globalStiboComponent.Name, globalStiboComponent.Id, "Is_Discontinued", GetLocalizationValueOfProperty(Convert.ChangeType("false", globalStiboComponent.Is_Discontinued.GetType()), language));
                    localizationEntity.AddOrUpdateComponentValue(otherSpecifications.Name, otherSpecifications.Id, "Is_Sellable", GetLocalizationValueOfProperty(Convert.ChangeType("true", otherSpecifications.Is_Sellable.GetType()), language));
                }
                // saving category entity to database
                await this._persistEntityPipeline.Run(new PersistEntityArgument(localizationEntity), context);
            }
        }

        private async Task<SellableItem> AttributeMappingForVarinat(CommercePipelineExecutionContext context, SellableItem sellableItem, string variantId, ItemVariationComponent itemVariationComponent, XElement variant, Task<List<Models.StiboSection>> sections, string attributeLevel, string language)
        {
            var attributes = variant.Element("Values").Descendants().Where(x => x.HasAttributes && x.Attribute("AttributeID") != null);

            LocalizationEntity localizationEntity = await this._localizeEntityPropertyCommand.GetLocalizationEntity(context.CommerceContext, sellableItem);

            string name = attributes.FirstOrDefault(r => r.Attribute("AttributeID").Value.Equals(Models.ProductContsants.MaterialDescriptionMarketing))?.Value;

            if (!string.IsNullOrWhiteSpace(name))
            {
                localizationEntity.AddOrUpdatePropertyValue("DisplayName", GetLocalizationValueOfProperty(Regex.Replace(name, @"[^0-9a-zA-Z]+", " "), language));
                localizationEntity.AddOrUpdatePropertyValue("Description", GetLocalizationValueOfProperty(Regex.Replace(name, @"[^0-9a-zA-Z]+", " "), language));
            }

            foreach (var attribute in attributes)
            {
                var attributeName = attribute.Attribute("AttributeID").Value;
                int sectionCount = 0;

                if (attributeName.ToLower().Equals(Models.ProductContsants.ListPrice) && !string.IsNullOrWhiteSpace(attribute.Value))
                {
                    if (itemVariationComponent.HasPolicy<ListPricingPolicy>())
                    {
                        var pricePolicy = itemVariationComponent.GetPolicy<ListPricingPolicy>();
                        pricePolicy.AddPrice(new Money("USD", Convert.ToDecimal(attribute.Value)));
                    }
                }

                foreach (var stiboSection in sections.Result)
                {
                    var section = stiboSection.StiboAttributeList.SingleOrDefault(x => !string.IsNullOrWhiteSpace(x.StiboName) &&
                     x.StiboName.ToLower().Equals(attributeName.ToLower()));

                    if (section != null)
                    {
                        if (section.AttributeLevel.Contains(attributeLevel))
                        {
                            var commerceComponent = itemVariationComponent.ChildComponents.Where(x => x.GetType().Name.ToLower().Equals(stiboSection.Section_Name.ToLower())).FirstOrDefault();

                            if (commerceComponent != null)
                            {
                                var property = commerceComponent.GetType().GetProperty(section.StiboName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (property != null)
                                {
                                    string attributeValue = string.Empty;
                                    if (section.ValueType == Models.ProductContsants.YesNoField && attribute.HasAttributes && !string.IsNullOrWhiteSpace(section.TransformedValue))
                                    {
                                        attributeValue = GetValueOfYesNoField(attribute, section);
                                    }
                                    else if (section.ValueType == Models.ProductContsants.MultiValueField || section.ValueType == Models.ProductContsants.LOVField || section.ValueType == Models.ProductContsants.BoxURLField && attribute.Descendants() != null && attribute.Descendants().Count() > 0)
                                    {
                                        attributeValue = string.Join("|", attribute.Descendants().ToList().Select(x => x.Value)).Trim();
                                    }
                                    else
                                    {
                                        attributeValue = attribute?.Value;
                                    }
                                    property.SetValue(commerceComponent, Convert.ChangeType(attributeValue, property.PropertyType), null);

                                    localizationEntity.AddOrUpdateComponentValue(stiboSection.Section_Name, commerceComponent.Id, section.StiboName, GetLocalizationValueOfProperty(attributeValue, language));

                                    await this._persistEntityPipeline.Run(new PersistEntityArgument(localizationEntity), context);
                                    break;
                                }
                                else
                                {
                                    InvalidStiboNameMessage(attributeName, sellableItem.ProductId, sellableItem.Name, itemVariationComponent.Id, itemVariationComponent.Name);
                                }
                            }
                            else
                            {
                                InvalidMappingMessage(attributeName, sellableItem.ProductId, sellableItem.Name, itemVariationComponent.Id, itemVariationComponent.Name);
                            }
                        }
                        else
                        {
                            InvalidAttributeLevelMessage(attributeName, sellableItem.ProductId, sellableItem.Name, itemVariationComponent.Id, itemVariationComponent.Name);
                        }
                    }
                    else
                    {
                        sectionCount++;
                        if (sectionCount == sections.Result.Count)
                            NotFoundAttributeMappingMessage(attributeName, sellableItem.ProductId, sellableItem.Name, itemVariationComponent.Id, itemVariationComponent.Name);
                    }

                }
            }
            variantUpdateList.Add(variantId);
            return sellableItem;
        }
        /// <summary>
        /// Add Product Tags
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sku"></param>
        private void ManageProductTags(SellableItem sellableItem, string sku, Category category2, Category category3)
        {
            if (!string.IsNullOrWhiteSpace(sku))
            {
                if (sku.Length == 6)
                {
                    Tag productTag = new Tag(Models.ProductContsants.SMOTag);
                    Tag productCategoryLWT2 = new Tag(category2.Name);
                    Tag productCategoryLWT3 = new Tag(category3.Name);
                    sellableItem.Tags = new List<Tag> { productTag, productCategoryLWT2, productCategoryLWT3 };
                }
            }
        }

        private string GetValueOfYesNoField(XElement attribute, Models.StiboAttributeMaster section)
        {
            string yesNoValue = string.Empty;
            var id = attribute.Attribute("ID").Value;
            string[] array = section.TransformedValue.Replace("%20", " ").Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (array != null && array.Count() > 0)
            {
                foreach (var item in array)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var itemdata = item.Split('=');
                        if (itemdata.Count() >= 2)
                        {
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>(itemdata[0], itemdata[1]);
                            dictionary.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }

            if (id == "Y" | id == "y")
            { if (dictionary.ContainsKey("Yes")) yesNoValue = dictionary["Yes"]; }
            else if (id == "N" | id == "n")
            { if (dictionary.ContainsKey("No")) yesNoValue = dictionary["No"]; }
            else { }
            return yesNoValue;
        }

        private async Task<List<Models.StiboSection>> GetStiboAttributeSectionMapping(CommercePipelineExecutionContext context, string language)
        {
            SitecoreConnectionManager connection = new SitecoreConnectionManager();
            var sectionItemtemp = await connection.GetItemByPathAsync(context.CommerceContext, Models.ProductContsants.StiboSectionManagerPath, "de-DE");
            var stiboSections = await connection.GetItemChildrenAsync(context.CommerceContext, sectionItemtemp.GetFieldValue("ItemID").ToString(), "de-DE");
            List<Models.StiboSection> sectionList = new List<Models.StiboSection>();

            if (stiboSections != null && stiboSections.Count() > 0)
            {
                foreach (var section in stiboSections)
                {
                    Models.StiboSection sectionItem = new Models.StiboSection();
                    var name = section.GetFieldValue(Models.ProductContsants.SectionNameField).ToString();
                    var stiboAttributes = section.GetFieldValue(Models.ProductContsants.StiboAttributesField).ToString();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        sectionItem.Section_Name = name;
                    }
                    if (!string.IsNullOrWhiteSpace(stiboAttributes))
                    {
                        sectionItem.Stibo_Attributes = stiboAttributes;
                        var items = stiboAttributes.Split('|');
                        if (items != null && items.Count() > 0)
                        {
                            List<Models.StiboAttributeMaster> attributeList = new List<Models.StiboAttributeMaster>();
                            foreach (var item in items)
                            {
                                var stiboAttributeItem = await connection.GetItemByPathAsync(context.CommerceContext, item);
                                if (stiboAttributeItem != null)
                                {
                                    Models.StiboAttributeMaster attributeMaster = new Models.StiboAttributeMaster();

                                    if (stiboAttributeItem.GetFieldValue(Models.ProductContsants.StiboAttributeNameField) != null)
                                        attributeMaster.StiboName = stiboAttributeItem.GetFieldValue(Models.ProductContsants.StiboAttributeNameField).ToString();

                                    if (stiboAttributeItem.GetFieldValue(Models.ProductContsants.TypeField) != null)
                                        attributeMaster.ValueType = stiboAttributeItem.GetFieldValue(Models.ProductContsants.TypeField).ToString();

                                    if (stiboAttributeItem.GetFieldValue(Models.ProductContsants.TransformedValueField) != null)
                                        attributeMaster.TransformedValue = stiboAttributeItem.GetFieldValue(Models.ProductContsants.TransformedValueField).ToString();

                                    if (stiboAttributeItem.GetFieldValue(Models.ProductContsants.AttributeLevelField) != null)
                                        attributeMaster.AttributeLevel = stiboAttributeItem.GetFieldValue(Models.ProductContsants.AttributeLevelField).ToString();

                                    attributeList.Add(attributeMaster);
                                }
                            }
                            sectionItem.StiboAttributeList = attributeList;
                        }
                    }
                    sectionList.Add(sectionItem);
                }
            }
            return sectionList;
        }

        private async Task<List<Models.ProductStaus>> GetProductStatusMapping(CommercePipelineExecutionContext context, string language)
        {
            SitecoreConnectionManager connection = new SitecoreConnectionManager();
            var sapStatus = await connection.GetItemByPathAsync(context.CommerceContext, Models.ProductContsants.SAPStausPath, language);
            var status = await connection.GetItemChildrenAsync(context.CommerceContext, sapStatus.GetFieldValue("ItemID").ToString(), language);
            List<Models.ProductStaus> productStausList = new List<Models.ProductStaus>();
            var sapStausItems = GetProductStatusItem(context, language);

            if (status != null && status.Count() > 0 && sapStausItems != null && sapStausItems.Result != null)
            {
                foreach (var code in status)
                {
                    Models.ProductStaus statusItem = new Models.ProductStaus();
                    var codeFieldValue = code.GetFieldValue(Models.ProductContsants.CodeField).ToString();
                    var stausFieldValue = code.GetFieldValue(Models.ProductContsants.StausField).ToString();
                    if (!string.IsNullOrWhiteSpace(codeFieldValue))
                    {
                        statusItem.Code = codeFieldValue;
                    }
                    if (!string.IsNullOrWhiteSpace(stausFieldValue) && sapStausItems.Result.Count() > 0)
                    {
                        var sapStatusItem = sapStausItems.Result.Where(x => x.GetFieldValue("ItemID").ToString().Equals(stausFieldValue.Trim(new char[] { '{', '}' }).ToLower()));
                        if (sapStatusItem != null && sapStatusItem.FirstOrDefault() != null)
                            statusItem.Status = sapStatusItem.FirstOrDefault().GetFieldValue("ItemName").ToString();
                    }
                    productStausList.Add(statusItem);
                }
            }
            return productStausList;
        }

        private async Task<List<ItemModel>> GetProductStatusItem(CommercePipelineExecutionContext context, string language)
        {
            SitecoreConnectionManager connection = new SitecoreConnectionManager();
            var statusFolderItem = await connection.GetItemByPathAsync(context.CommerceContext, Models.ProductContsants.SAPStausItemPath, language);
            return await connection.GetItemChildrenAsync(context.CommerceContext, statusFolderItem.GetFieldValue("ItemID").ToString(), language);
        }

        private List<Parameter> GetLocalizationValueOfProperty(object propertyValue, string langauge)
        {
            // preparing translations
            List<Parameter> localizations = new List<Parameter> { new Parameter { Key = langauge, Value = $"{propertyValue}" }, };
            return localizations;
        }

        private async Task CreateVariants(CommerceContext context, SellableItem product, IEnumerable<XElement> variants, Task<List<Models.StiboSection>> sections, string language, Category category2, Category category3, Task<List<Models.ProductStaus>> statusMappingItems)
        {
            int count = 0;
            foreach (var variant in variants)
            {

                XElement variantNameElement = null, variantIdElement = null;
                string variantname = null, variantId = null;

                var attributes = variant.Element("Values").Descendants().Where(x => x.HasAttributes && x.Attribute("AttributeID") != null);

                var productStatus = GetProductStaus(context.PipelineContext, language, attributes, statusMappingItems);

                if (variant.Descendants("Values").Elements("Value").Where(x => x.HasAttributes && x.Attribute("AttributeID").Value == Models.ProductContsants.MaterialDescriptionMarketing) != null)
                {
                    variantNameElement = variant.Descendants("Values").Elements("Value").Where(x => x.HasAttributes && x.Attribute("AttributeID").Value == Models.ProductContsants.MaterialDescriptionMarketing).FirstOrDefault();
                }

                if (variant.Descendants("Values").Elements("Value").Where(x => x.HasAttributes && x.Attribute("AttributeID").Value == Models.ProductContsants.MfgProductNumberSAP) != null)
                {
                    variantIdElement = variant.Descendants("Values").Elements("Value").Where(x => x.HasAttributes && x.Attribute("AttributeID").Value == Models.ProductContsants.MfgProductNumberSAP).FirstOrDefault();
                }
                try
                {
                    if (variantNameElement != null && productStatus != null)
                    {
                        if (!string.IsNullOrWhiteSpace(productStatus.Status))
                        {
                            if (string.IsNullOrWhiteSpace(variantNameElement.Value) && string.IsNullOrWhiteSpace(variantIdElement.Value) || productStatus.Status.Equals(Models.ProductContsants.InActive))
                            {
                                InActiveProductMessage(product.ProductId, product.Name, variantIdElement.Value);
                                return;
                            }

                            variantname = Regex.Replace(variantNameElement.Value, @"[^0-9a-zA-Z]+", " ");
                            variantId = variantIdElement.Value;

                            //Retrieve Sellable Item Variant component from parent SellableItem
                            var itemVariationComponent = product.GetVariation(variantId);

                            if (itemVariationComponent == null)
                            {
                                product = await commander.Command<CreateSellableItemVariationCommand>().Process(context, product.Id, variantId, variantname, variantname);

                                itemVariationComponent = product.GetComponent<ItemVariationsComponent>()
                                .ChildComponents.FirstOrDefault(y => y.Id == variantId) as ItemVariationComponent;

                                itemVariationComponent.ChildComponents = new List<Component>
                                {
                                new ProductOverview
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.ProductOverview", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.ProductOverview"
                                },
                                 new ProductFeatures
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.ProductFeatures", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.ProductFeatures"
                                },
                                new CategoryAttributes
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.CategoryAttributes", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.CategoryAttributes"
                                },
                                new SellableItemFacets
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.SellableItemFacets", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.SellableItemFacets"
                                },
                                new AdditionalResources
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.AdditionalResources", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.AdditionalResources"
                                },
                                new GlobalStiboAttributes
                                {
                                     Name = "ItemVariationsComponent.ItemVariationComponent.GlobalStiboAttributes", Id = System.Guid.NewGuid().ToString("N"),
                                     Comments = "ItemVariationsComponent.ItemVariationComponent.GlobalStiboAttributes"
                                },
                                new InstallationAndInstructions
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.InstallationAndInstructions", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.InstallationAndInstructions"
                                },
                                new New
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.New", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.New"
                                },
                                new OtherSpecifications
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.OtherSpecifications", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.OtherSpecifications"
                                },
                                new Parts
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.Parts", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.Parts"
                                },
                                new ProductMedia
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.ProductMedia", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.ProductMedia"
                                },
                                new ProductVideoGallery
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.ProductVideoGallery", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.ProductVideoGallery"
                                },
                                new StyleSpecification
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.StyleSpecification", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.StyleSpecification"
                                },
                                new TechnicalSpecification
                                {
                                    Name = "ItemVariationsComponent.ItemVariationComponent.TechnicalSpecification", Id = System.Guid.NewGuid().ToString("N"),
                                    Comments = "ItemVariationsComponent.ItemVariationComponent.TechnicalSpecification"
                                },
                                  new ContentManageableAttributes
                                {
                                     Name = "ItemVariationsComponent.ItemVariationComponent.ContentManageableAttributes", Id = System.Guid.NewGuid().ToString("N"),
                                     Comments = "ItemVariationsComponent.ItemVariationComponent.ContentManageableAttributes"
                                },
                        };
                                count++;
                                variantSuccessList.Add(variantId);

                                //IF you found first varint is SMO product than add SMP Tag on Sellable Item
                                if (count == 1)
                                    ManageProductTags(product, variantId, category2, category3);

                                //Set Category Attributes for Variant
                                if (itemVariationComponent.HasComponent<CategoryAttributes>())
                                {
                                    var sellableItemCategoryAttribute = itemVariationComponent.GetComponent<CategoryAttributes>();
                                    if (category3.HasComponent<CategoryAttributes>())
                                    {
                                        var categoryAttribute = category3.GetComponent<CategoryAttributes>();
                                        sellableItemCategoryAttribute.ClassificationType = categoryAttribute.ClassificationType;
                                        sellableItemCategoryAttribute.SitecoreMappingPath = categoryAttribute.SitecoreMappingPath;
                                        itemVariationComponent.SetComponent(sellableItemCategoryAttribute);
                                    }
                                }

                                await this._persistEntityPipeline.Run(new PersistEntityArgument(product), context.PipelineContext);

                                await AttributeMappingForVarinat(context.PipelineContext, product, variantId, itemVariationComponent, variant, sections, Models.ProductContsants.VarinatLevelAttribute, language);

                                await MapProductStatusForVariant(context.PipelineContext, product, productStatus, variantId, itemVariationComponent, language);

                            }
                            else
                            {
                                context.Logger.LogInformation("ProductImport:Found Product variant: " + itemVariationComponent.Name);
                                await AttributeMappingForVarinat(context.PipelineContext, product, variantId, itemVariationComponent, variant, sections, Models.ProductContsants.VarinatLevelAttribute, language);
                                await MapProductStatusForVariant(context.PipelineContext, product, productStatus, variantId, itemVariationComponent, language);
                            }
                        }
                    }
                    var editResults = await _editSellableItemPipeline.Run(product, context.PipelineContext);
                }
                catch (Exception ex)
                {
                    context.Logger.LogInformation("ProductImport:Error While creating variant" + variantIdElement.Value + ex.Message);
                    variantFailureList.Add(variantIdElement.Value);
                }
                if (!string.IsNullOrWhiteSpace(productImportStatus)) productImportStatus += "--------------------------" + System.Environment.NewLine;
            }
        }

        private async Task AssociateSellableItemToParent(SellableItem sitem, Catalog catalog, Category category, CommercePipelineExecutionContext context)
        {
            var sItemId = sitem.Id;
            string catalogId = GenerateFullCatalogName(catalog.Name);
            string categoryId = GenerateFullCategoryId(catalog.Name, category.Name);
            var catalogReferenceArgument = await _associateSellableItemToParentCommand.Process(context.CommerceContext, catalogId, categoryId, sItemId);
        }

        private string NotFoundAttributeMappingMessage(string attributeName, string productId, string productName, string variantid = null, string varinatName = null)
        {
            if (string.IsNullOrWhiteSpace(variantid) && string.IsNullOrWhiteSpace(varinatName))
            {
                productImportStatus += "Not able to find mapping or section for attribute: " + attributeName + " for Product: " + productId + ", Product Name: " + productName + System.Environment.NewLine;
            }
            else
            {
                productImportStatus += "Not able to find mapping or section for attribute: " + attributeName + " for Variant: " + variantid + ", Variant Name: " + varinatName + " ,for Product: " + productId + System.Environment.NewLine;
            }
            return productImportStatus;
        }
        private string InvalidMappingMessage(string attributeName, string productId, string productName, string variantid = null, string varinatName = null)
        {
            if (string.IsNullOrWhiteSpace(variantid) && string.IsNullOrWhiteSpace(varinatName))
            {
                productImportStatus += "Found mapping but it contains some invalid configuraion for attribute: " + attributeName + " for Product: " + productId + ", Product Name: " + productName + System.Environment.NewLine;
            }
            else
            {
                productImportStatus += "Found mapping but it contains some invalid configuraion for attribute: " + attributeName + " for Variant: " + variantid + ", Variant Name: " + varinatName + " ,for Product: " + productId + System.Environment.NewLine;
            }
            return productImportStatus;
        }

        private string InvalidStiboNameMessage(string attributeName, string productId, string productName, string variantid = null, string varinatName = null)
        {
            if (string.IsNullOrWhiteSpace(variantid) && string.IsNullOrWhiteSpace(varinatName))
            {
                productImportStatus += "Found mapping but StiboName field data is not matching: " + attributeName + " for Product: " + productId + ", Product Name: " + productName + System.Environment.NewLine;
            }
            else
            {
                productImportStatus += "Found mapping but StiboName field data is not matching: " + attributeName + " for Variant: " + variantid + ", Variant Name: " + varinatName + " ,for Product: " + productId + System.Environment.NewLine;
            }
            return productImportStatus;
        }
        private string InvalidAttributeLevelMessage(string attributeName, string productId, string productName, string variantid = null, string varinatName = null)
        {
            if (string.IsNullOrWhiteSpace(variantid) && string.IsNullOrWhiteSpace(varinatName))
            {
                productImportStatus += "Found mapping but AttributeLevel field data is not matching: " + attributeName + " for Product: " + productId + ", Product Name: " + productName + System.Environment.NewLine;
            }
            else
            {
                productImportStatus += "Found mapping but AttributeLevel field data is not matching: " + attributeName + " for Variant: " + variantid + ", Variant Name: " + varinatName + " ,for Product: " + productId + System.Environment.NewLine;
            }
            return productImportStatus;
        }
        private string NotFoundCategoryMappingMessage(string categoryId, string categoryName, string categoryLevel)
        {
            productImportStatus += "Not able to find mapping for Category: " + categoryId + ",Category Name: " + categoryName + ",Category Level: " + categoryLevel + System.Environment.NewLine;
            categoryFailureList.Add(categoryId);
            return productImportStatus;
        }

        private string InActiveProductMessage(string productId, string productName, string variantId = null)
        {
            if (string.IsNullOrWhiteSpace(variantId))
            {
                productImportStatus += "Not able to import product as it has Inactive status. Product: " + productId + ", ProductName: " + productName + System.Environment.NewLine;
            }
            else
            {
                productImportStatus += "Not able to import variant as it has Inactive status. Variant: " + variantId + ", Product: " + productId + ",ProductName: " + productName + System.Environment.NewLine;
            }
            return productImportStatus;
        }

        private static string GenerateFullCatalogName(string catalogName)
        {
            return $"{CommerceEntity.IdPrefix<Catalog>()}{catalogName}";
        }
        private static string GenerateFullCategoryId(string catalogName, string categoryName)
        {
            return $"{CommerceEntity.IdPrefix<Category>()}{catalogName}-{categoryName}";
        }
        private static string GenerateFullSellableItemId(string sellableItemId)
        {
            return $"{CommerceEntity.IdPrefix<SellableItem>()}{sellableItemId}";
        }
    }
}