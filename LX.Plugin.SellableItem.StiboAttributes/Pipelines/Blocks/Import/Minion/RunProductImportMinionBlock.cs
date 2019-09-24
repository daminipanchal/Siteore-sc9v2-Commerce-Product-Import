using LX.Plugin.SellableItem.StiboAttributes.Models;
using LX.Plugin.SellableItem.StiboAttributes.Pipelines.Arguments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Management;
using Sitecore.Framework.Pipelines;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LX.Plugin.SellableItem.StiboAttributes.Pipelines.Blocks.Import.Minion
{
    public class RunProductImportMinionBlock : PipelineBlock<MinionRunResultsModel, MinionRunResultsModel, CommercePipelineExecutionContext>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IIMportSellableItemsPipeline _iImportProuctPipeline;

        private string CompletedDirectory, FailedDirectory, BaseDirectory;
        private string Host, Username, Password;

        public RunProductImportMinionBlock(IHostingEnvironment hostingEnvironment, IIMportSellableItemsPipeline iIMportSellableItemsPipeline)
        {
            _hostingEnvironment = hostingEnvironment;
            _iImportProuctPipeline = iIMportSellableItemsPipeline;
        }

        public override async Task<MinionRunResultsModel> Run(MinionRunResultsModel arg, CommercePipelineExecutionContext context)
        {
            SitecoreConnectionManager connection = new SitecoreConnectionManager();
            var stiboImportSettingItem = await connection.GetItemByPathAsync(context.CommerceContext, ProductContsants.StiboImportSettingPath);
            if (stiboImportSettingItem == null)
            {
                context.Logger.LogInformation($"Minion-ProductImport: Could not find stibo import setting at location: {ProductContsants.StiboImportSettingPath}");
                return arg;
            }

            Host = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.SFTPHostField);
            Username = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.SFTPUsernameField);
            Password = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.SFTPPasswordField);

            BaseDirectory = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.BaseDirectoryField);
            CompletedDirectory = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.CompletedDirectoryField);
            FailedDirectory = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.FailedDirectoryField);

            var BrandRegions = (string)stiboImportSettingItem.GetFieldValue(ProductContsants.BrandRegionMappingField);
            if (string.IsNullOrWhiteSpace(BrandRegions))
            {
                context.Logger.LogInformation($"Minion-ProductImport: Brand settings could not found on stibo import setting: {ProductContsants.StiboImportSettingPath}");
                return arg;
            }

            foreach (var id in BrandRegions.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var brandRegionImportSettingItem = await connection.GetItemByIdAsync(context.CommerceContext, id);
                if (brandRegionImportSettingItem == null)
                {
                    context.Logger.LogInformation($"Minion-ProductImport: Could not find brand settings for stibo import. ID: {id}");
                    continue;
                }

                var CatalogName = (string)brandRegionImportSettingItem.GetFieldValue(ProductContsants.CatalogNameField);
                var BrandDirectory = (string)brandRegionImportSettingItem.GetFieldValue(ProductContsants.BrandDirectoryField);
                var RegionLanguagePattern = (string)brandRegionImportSettingItem.GetFieldValue(ProductContsants.RegionLanguagePatternField);

                context.Logger.LogInformation($"Minion-ProductImport: Product import started for Brand: {BrandDirectory}, CatalogName: {CatalogName}, Region/Language: {RegionLanguagePattern}");

                using (SftpClient sftp = new SftpClient(Host, Username, Password))
                {
                    try
                    {

                        context.Logger.LogInformation($"Minion-ProductImport: Download started");

                        sftp.Connect();

                        var files = sftp.ListDirectory($"{BaseDirectory}/{BrandDirectory}");

                        CleanupDirectory(BrandDirectory, context);

                        foreach (var file in files)
                        {
                            if (file.IsDirectory || file.IsSymbolicLink)
                                continue;

                            //Ignore other region/language files for same brand
                            if (!file.Name.ToLower().Contains(RegionLanguagePattern.ToLower()))
                                continue;
                            
                            ////Temp
                            //if (file.Name != "Grohe-Context1##1607423-1607845_0.xml")
                            //    continue;

                            try
                            {
                                using (Stream fileStream = File.OpenWrite(this.GetFullPath(BrandDirectory + "/" + file.Name)))
                                {
                                    sftp.DownloadFile(file.FullName, fileStream);
                                }
                            }
                            catch (Exception ex)
                            {
                                context.Logger.LogError($"Minion-ProductImport: Error while downloading file from SFTP {file.FullName}" + ex.Message);
                            }
                        }

                        sftp.Disconnect();

                        context.Logger.LogInformation($"Minion-ProductImport: Download completed");

                        var directoryPath = this.GetFullPath(BrandDirectory);
                        var filePaths = Directory.GetFiles(directoryPath).ToList();
                        foreach (var fileFullName in filePaths)
                        {
                            try
                            {
                                context.Logger.LogInformation($"Minion-ProductImport: Import started");

                                var model = new ImportSellableItemPipelineArgument("");
                                //Call selleble item import pipeline
                                model.XMlFilePath = fileFullName;
                                model.CatalogName = CatalogName;
                                model.CatalogDisplayName = CatalogName;
                                var result = await _iImportProuctPipeline.Run(model, context);

                                context.Logger.LogInformation($"Minion-ProductImport: Import completed");

                                context.Logger.LogInformation($"Minion-ProductImport: Moving files to completed/failed started");
                                //Move file to completed/failed folder
                                sftp.Connect();

                                files = sftp.ListDirectory($"{BaseDirectory}/{BrandDirectory}");
                                var fileName = fileFullName.Substring(fileFullName.LastIndexOf("\\") + 1);
                                foreach (var f in files)
                                {
                                    if (f.Name != fileName)
                                        continue;

                                    if (result.FileImportSuccess)
                                        f.MoveTo($"{BaseDirectory}/{BrandDirectory}/{CompletedDirectory}/{f.Name}");
                                    else
                                        f.MoveTo($"{BaseDirectory}/{BrandDirectory}/{FailedDirectory}/{f.Name}");
                                }

                                sftp.Disconnect();

                                context.Logger.LogInformation($"Minion-ProductImport: Moving files to completed/failed completed");
                            }
                            catch (Exception ex)
                            {
                                context.Logger.LogError($"Minion-ProductImport: Error while importing/moving file: {fileFullName}, CatalogName: {CatalogName}" + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Logger.LogError($"Minion-ProductImport: Error while downloading/importing/moving " + ex.Message);
                    }
                }

                context.Logger.LogInformation($"Minion-ProductImport: Product import completed for Brand: {BrandDirectory}, CatalogName: {CatalogName}, Region/Language: {RegionLanguagePattern}");

            }

            return arg;
        }

        private string GetFullPath(string fileName)
        {
            return Path.Combine(this._hostingEnvironment.WebRootPath, "ProductXMLs", fileName);
        }

        private void CleanupDirectory(string directory, CommercePipelineExecutionContext context)
        {
            var fullPath = this.GetFullPath(directory);

            try
            {
                if (Directory.Exists(fullPath))
                    Directory.Delete(fullPath, true);

                Directory.CreateDirectory(fullPath);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Minion-ProductImport: Error while deleting/creating directory {fullPath} " + ex.Message);
            }
        }
    }
}
