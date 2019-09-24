using LX.Foundation.Email;
using LX.Plugin.SellableItem.StiboAttributes.Models;
using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Framework.Pipelines;
using Sitecore.Services.Core.Model;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.Import
{
    [PipelineDisplayName(ImportSellableItemsConstants.Pipelines.Blocks.ManageProductImportStatus)]
    public class ManageProductImportStatus : PipelineBlock<ImportSellableItemPipelineArgument, ImportSellableItemPipelineArgument, CommercePipelineExecutionContext>
    {

        public ManageProductImportStatus()
        {
        }

        public override Task<ImportSellableItemPipelineArgument> Run(ImportSellableItemPipelineArgument arg, CommercePipelineExecutionContext context)
        {
            context.Logger.LogInformation("---------Sending Email For Product Import Status------------");
            return Task.Run(() =>
            {
                var status = SendImportStatusEmail(arg, context);
                if (status == true)
                {
                    context.Logger.LogInformation("---------Sent Email For Product Import Status------------");
                }
                else
                {
                    context.Logger.LogInformation("---------Error while sending Email For Product Import Status------------");
                }
                return arg;
            });
        }

        /// Seperate Method in Foundation for SendEmail
        private bool SendImportStatusEmail(ImportSellableItemPipelineArgument arg, CommercePipelineExecutionContext context)
        {
            var emailConfigurationItem = GetEmailConfigurationItem(context);
            bool status = false;
            if (emailConfigurationItem != null && emailConfigurationItem.Result != null)
            {
                var emailConfigurationModel = GetEmailConfigurationModel(emailConfigurationItem.Result);

                string productSuccessData = string.Empty, productFailureData = string.Empty; int productSuccessCount = 0; int productFailureCount = 0;
                string categorySuccessData = string.Empty, categoryFailureData = string.Empty; int variantSuccessCount = 0; int variantFailureCount = 0;
                string variantSuccessData = string.Empty, variantFailureData = string.Empty; int categorySuccessCount = 0; int categoryFailureCount = 0;
                string productUpdateData = string.Empty, variantUpdateData = string.Empty; int productUpdateCount = 0; int variantUpdateCount = 0; string folderName = string.Empty;

                var importSettings = GetStiboImportSettings(context);
                if (importSettings != null && importSettings.Result != null)
                {
                    if (arg.FileImportSuccess)
                        folderName = (string)importSettings.Result.GetFieldValue(ProductContsants.CompletedDirectoryField);
                    else
                        folderName = (string)importSettings.Result.GetFieldValue(ProductContsants.FailedDirectoryField);
                }


                if (arg.ProductSuccessList != null && arg.ProductSuccessList.Count() > 0)
                {
                    productSuccessData = string.Join(",", arg.ProductSuccessList.Distinct().ToArray());
                    productSuccessCount = arg.ProductSuccessList.Distinct().Count();
                }
                if (arg.ProductFailureList != null && arg.ProductFailureList.Count() > 0)
                {
                    productFailureData = string.Join(",", arg.ProductFailureList.Distinct().ToArray());
                    productFailureCount = arg.ProductFailureList.Distinct().Count();
                }
                if (arg.ProductUpdateList != null && arg.ProductUpdateList.Count() > 0)
                {
                    productUpdateData = string.Join(",", arg.ProductUpdateList.Distinct().ToArray());
                    productUpdateCount = arg.ProductUpdateList.Distinct().Count();
                }
                if (arg.VariantUpdateList != null && arg.VariantUpdateList.Count() > 0)
                {
                    variantUpdateData = string.Join(",", arg.VariantUpdateList.Distinct().ToArray());
                    variantUpdateCount = arg.VariantUpdateList.Distinct().Count();
                }
                if (arg.CategorySuccessList != null && arg.CategorySuccessList.Count() > 0)
                {
                    categorySuccessData = string.Join(",", arg.CategorySuccessList.Distinct().ToArray());
                    categorySuccessCount = arg.CategorySuccessList.Distinct().Count();
                }
                if (arg.CategoryFailureList != null && arg.CategoryFailureList.Count() > 0)
                {
                    categoryFailureData = string.Join(",", arg.CategoryFailureList.Distinct().ToArray());
                    categoryFailureCount = arg.CategoryFailureList.Distinct().Count();
                }
                if (arg.VariantSuccessList != null && arg.VariantSuccessList.Count() > 0)
                {
                    variantSuccessData = string.Join(",", arg.VariantSuccessList.Distinct().ToArray());
                    variantSuccessCount = arg.VariantSuccessList.Distinct().Count();
                }
                if (arg.VariantFailureList != null && arg.VariantFailureList.Count() > 0)
                {
                    variantFailureData = string.Join(",", arg.VariantFailureList.Distinct().ToArray());
                    variantFailureCount = arg.VariantFailureList.Distinct().Count();
                }


                var emailService = new EmailService();
                if (!string.IsNullOrWhiteSpace(emailConfigurationModel.Sender) && !string.IsNullOrWhiteSpace(emailConfigurationModel.Recipients) && !string.IsNullOrWhiteSpace(emailConfigurationModel.Subject) && !string.IsNullOrWhiteSpace(emailConfigurationModel.Body))
                {
                    emailConfigurationModel.Subject = emailConfigurationModel.Subject.Replace(Models.ProductContsants.CurrentDate, System.DateTime.Now.ToShortDateString());


                    StringBuilder sb = new StringBuilder(emailConfigurationModel.Body);
                    emailConfigurationModel.Body = sb.Replace(Models.ProductContsants.FileName, arg.XMlFileName)
                                                     .Replace(Models.ProductContsants.FolderName, folderName)
                                                     .Replace(Models.ProductContsants.CategoryFailureCount, categoryFailureCount.ToString())
                                                     .Replace(Models.ProductContsants.CategorySuccessCount, categorySuccessCount.ToString())
                                                     .Replace(Models.ProductContsants.ProductSuccessCount, productSuccessCount.ToString())
                                                     .Replace(Models.ProductContsants.ProductFailureCount, productFailureCount.ToString())
                                                     .Replace(Models.ProductContsants.ProductUpdateCount, productUpdateCount.ToString())
                                                     .Replace(Models.ProductContsants.VariantSuccessCount, variantSuccessCount.ToString())
                                                     .Replace(Models.ProductContsants.VariantFailureCount, variantFailureCount.ToString())
                                                     .Replace(Models.ProductContsants.VariantUpdateCount, variantUpdateCount.ToString())
                                                     .Replace(Models.ProductContsants.CategorySuccessData, categorySuccessCount.ToString())
                                                     .Replace(Models.ProductContsants.CategoryFailureData, categoryFailureData)
                                                     .Replace(Models.ProductContsants.ProductSuccessData, productSuccessData)
                                                     .Replace(Models.ProductContsants.ProductFailureData, productFailureData)
                                                     .Replace(Models.ProductContsants.ProductUpdateData, productUpdateData)
                                                     .Replace(Models.ProductContsants.VariantSuccessData, variantSuccessData)
                                                     .Replace(Models.ProductContsants.VariantFailureData, variantFailureData)
                                                     .Replace(Models.ProductContsants.VariantUpdateData, variantUpdateData).ToString();

                    var attachment = GetAttchmentData(arg, emailConfigurationModel.AttachmentName);

                    var message = emailService.PropareMailContent(emailConfigurationModel.Sender, emailConfigurationModel.Recipients.Split('|'), emailConfigurationModel.Subject, emailConfigurationModel.Body, attachment);

                    if (!string.IsNullOrWhiteSpace(emailConfigurationModel.Host) && !string.IsNullOrWhiteSpace(emailConfigurationModel.Port) && !string.IsNullOrWhiteSpace(emailConfigurationModel.Username) && !string.IsNullOrWhiteSpace(emailConfigurationModel.Password) && message != null)
                    {
                        status = emailService.SendEmail(emailConfigurationModel.Host, int.Parse(emailConfigurationModel.Port), emailConfigurationModel.Username, emailConfigurationModel.Password, message);
                    }
                }
            }
            return status;
        }

        private Attachment GetAttchmentData(ImportSellableItemPipelineArgument arg, string fileName)
        {
            string fileData = string.Empty;
            if (arg.ProductImportStatusList != null)
            {
                foreach (var item in arg.ProductImportStatusList)
                {
                    fileData += item + System.Environment.NewLine;
                }
            }
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                byte[] data = Encoding.ASCII.GetBytes(fileData);
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(data);
                fileName = fileName + ".txt";
                Attachment attchment = new Attachment(memoryStream, fileName, "text/plain");
                return attchment;
            }
            return null;
        }

        private async Task<ItemModel> GetEmailConfigurationItem(CommercePipelineExecutionContext context)
        {
            SitecoreConnectionManager connection = new SitecoreConnectionManager();
            var stiboMailSetting = await connection.GetItemByPathAsync(context.CommerceContext, Models.ProductContsants.MailSettings);
            if (stiboMailSetting == null) return null;
            return stiboMailSetting;
        }

        private async Task<ItemModel> GetStiboImportSettings(CommercePipelineExecutionContext context)
        {
            SitecoreConnectionManager connection = new SitecoreConnectionManager();
            var stiboImportSettingItem = await connection.GetItemByPathAsync(context.CommerceContext, ProductContsants.StiboImportSettingPath);
            if (stiboImportSettingItem == null) return null;
            return stiboImportSettingItem;
        }

        private EmailConfiguration GetEmailConfigurationModel(ItemModel model)
        {
            EmailConfiguration configuration = new EmailConfiguration();
            configuration.Body = model.GetFieldValue(Models.ProductContsants.BodyField).ToString();
            configuration.Sender = model.GetFieldValue(Models.ProductContsants.SenderField).ToString();
            configuration.Recipients = model.GetFieldValue(Models.ProductContsants.RecipientsField).ToString();
            configuration.Subject = model.GetFieldValue(Models.ProductContsants.SubjectField).ToString();
            configuration.Host = model.GetFieldValue(Models.ProductContsants.HostField).ToString();
            configuration.Port = model.GetFieldValue(Models.ProductContsants.PortField).ToString();
            configuration.Username = model.GetFieldValue(Models.ProductContsants.UsernameField).ToString();
            configuration.Password = model.GetFieldValue(Models.ProductContsants.PasswordField).ToString();
            configuration.AttachmentName = model.GetFieldValue(Models.ProductContsants.AttachmentField).ToString();
            return configuration;
        }
    }
}
