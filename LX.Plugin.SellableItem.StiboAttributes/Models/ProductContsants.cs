using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Models
{
    public static class ProductContsants
    {
        public static readonly string CategoryMappingPath = "/sitecore/Commerce/Commerce Control Panel/Grohe/Product Categories";
        public static readonly string StiboSectionManagerPath = "/sitecore/Commerce/Commerce Control Panel/Stibo Section Manager";
        public static readonly string SAPStausPath = "/sitecore/Commerce/Commerce Control Panel/SAP/Code";
        public static readonly string MailSettings = "/sitecore/Commerce/Commerce Control Panel/Product Import Mail Setting";
        public static readonly string SAPStausItemPath = "/sitecore/Commerce/Commerce Control Panel/SAP/Status";
        public static readonly string StiboLanguages = "f85537eb-984f-4e56-a9a1-17dc7c00692c";
        public static readonly string SuperCategoryTemplateId = "11f9848c-60e4-46d8-944a-25c4ebecb1dd";
        public static readonly string CategoryTemplateId = "4ad006ed-f0bf-4443-9030-31a380d64d8f";
        public static readonly string SubCategoryTemplateId = "70df339a-e890-411f-9c04-ec5b80dd973f";

        //Product SAP Status
        public static readonly string Active = "Active";
        public static readonly string InActive = "InActive";
        public static readonly string Discontinued = "Discontinued";

        //Attribute Level
        public static readonly string ProductLevelAttribute = "{ADC29B32-C793-4C2E-B06D-BA225F500771}";
        public static readonly string VarinatLevelAttribute = "{BF867487-7872-4EE0-B50E-F9837C2A8AE0}";

        //DataType of Attribute
        public static readonly string YesNoField = "{E6B38825-A661-4488-A072-02EE904FACF8}";
        public static readonly string LOVField = "{9C796500-08F2-40B2-B362-B45466E130D3}";
        public static readonly string MultiValueField = "{5A1CD8B2-9DC5-453D-8A47-2142BBB729CF}";
        public static readonly string BoxURLField = "{2672CC23-BBE5-4BC8-B541-EDA0A0975366}";

        //Product Category Template
        public static readonly string CommerceNameField = "Commerce Name";
        public static readonly string StiboClassificationsField = "StiboClassifications";

        //Stibo Section Template
        public static readonly string SectionNameField = "Section Name";
        public static readonly string StiboAttributesField = "Stibo Attributes";

        //SAPCode Template
        public static readonly string CodeField = "Code";
        public static readonly string StausField = "Status";

        //Product Classification Template
        public static readonly string StiboNameField = "Stibo Name";
        public static readonly string StiboIdField = "Stibo ID";
        public static readonly string LevelField = "Level";

        //Stibo Attribute Master Template
        public static readonly string StiboAttributeNameField = "StiboName";
        public static readonly string TypeField = "ValueType";
        public static readonly string TransformedValueField = "TransformedValue";
        public static readonly string AttributeLevelField = "AttributeLevel";

        //Stibo Language Configuration
        public static readonly string ContextId = "ContextID";
        public static readonly string LanguageKey = "Language Key";

        //Product Tags
        public static readonly string SMOTag = "SMO";

        //Manage Readonly Values
        public static readonly bool EditableField = false;
        public static readonly bool NonEditableField = false;
        public static readonly string IsEditableFieldName = "IsEditableFields";

        //Sellable Item or Varint Link
        public static readonly string VariantURL = "<a href='/entityView/Variant/1/{0}/{1}'>{2}_{3}</a>";

        //Replacement Options
        public static readonly string ReplacementMaterial = "Replacement_Material";
        public static readonly string ReplacementParts = "Replacement_Parts";

        //Category Level
        public static readonly string SuperCategoryLevel = "LWT1";
        public static readonly string CategoryLevel = "LWT2";
        public static readonly string SubCategoryLevel = "LWT3";

        //Product Import Mail Settings
        public static readonly string SenderField = "Sender";
        public static readonly string RecipientsField = "Recipients";
        public static readonly string SubjectField = "Subject";
        public static readonly string BodyField = "Body";
        public static readonly string HostField = "MailServer";
        public static readonly string PortField = "MailServerPort";
        public static readonly string UsernameField = "MailServerUserName";
        public static readonly string PasswordField = "MailServerPassword";
        public static readonly string AttachmentField = "Attachment File Name";

        //Tokens For MailBody
        public static readonly string CurrentDate = "#CurrentDate#";
        public static readonly string FileName = "#FileName#";
        public static readonly string FolderName = "#FolderName#";

        public static readonly string CategorySuccessCount = "#CategorySuccessCount#";
        public static readonly string CategoryFailureCount = "#CategoryFailureCount#";
        public static readonly string ProductSuccessCount = "#ProductSuccessCount#";
        public static readonly string ProductUpdateCount = "#ProductUpdateCount#";
        public static readonly string ProductFailureCount = "#ProductFailureCount#";
        public static readonly string VariantSuccessCount = "#VariantSuccessCount#";
        public static readonly string VariantUpdateCount = "#VariantUpdateCount#";
        public static readonly string VariantFailureCount = "#VariantFailureCount#";

        public static readonly string CategorySuccessData = "#CategorySuccessData#";
        public static readonly string CategoryFailureData = "#CategoryFailureData#";
        public static readonly string ProductSuccessData = "#ProductSuccessData#";
        public static readonly string ProductFailureData = "#ProductFailureData#";
        public static readonly string ProductUpdateData = "#ProductUpdateData#";
        public static readonly string VariantSuccessData = "#VariantSuccessData#";
        public static readonly string VariantFailureData = "#VariantFailureData#";
        public static readonly string VariantUpdateData = "#VariantUpdateData#";

        //Stibo Import Settings
        public static readonly string StiboImportSettingPath = "/sitecore/Commerce/Commerce Control Panel/Stibo Import Setting";
        public static readonly string SFTPHostField = "Host";
        public static readonly string SFTPUsernameField = "Username";
        public static readonly string SFTPPasswordField = "Password";
        public static readonly string BaseDirectoryField = "BaseDirectory";
        public static readonly string CompletedDirectoryField = "CompletedDirectory";
        public static readonly string FailedDirectoryField = "FailedDirectory";
        public static readonly string BrandRegionMappingField = "BrandRegion";

        public static readonly string CatalogNameField = "CatalogName";
        public static readonly string BrandDirectoryField = "BrandDirectory";
        public static readonly string RegionLanguagePatternField = "RegionLanguagePattern";

        //Stibo XML File Attribute Name
        public static readonly string MaterialDescriptionMarketing = "MaterialDescriptionMarketing";
        public static readonly string MfgProductNumberSAP = "Mfg_Product_Number_SAP";
        public static readonly string Brand = "Brand";
        public static readonly string ListPrice = "list_price";
    }

    public static class CommerceUiType
    {
        public static readonly string Empty = "Empty";
        public static readonly string Autocomplete = "Autocomplete";
        public static readonly string DownloadCsv = "DownloadCsv";
        public static readonly string Dropdown = "Dropdown";
        public static readonly string EntityLink = "EntityLink";
        public static readonly string FullDateTime = "FullDateTime";
        public static readonly string Html = "Html";
        public static readonly string ItemLink = "ItemLink";
        public static readonly string List = "List";
        public static readonly string Multiline = "Multiline";
        public static readonly string Options = "Options";
        public static readonly string Product = "Product";
        public static readonly string RichText = "RichText";
        public static readonly string SelectList = "SelectList";
        public static readonly string Sortable = "Sortable";
        public static readonly string SubItemLink = "SubItemLink";
        public static readonly string Tags = "Tags";
    }
}
