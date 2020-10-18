using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using PCLStorage;

namespace wkwebviewSample.Helpers
{
    public class DataHelper
    {
        public async static Task<bool> UnzipFileInDirectory()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("wkwebviewSample.ressources.zip");

            // Create folder version
            IFolder rootFolder = await PCLHelper.CreateFolder("1");

            ZipFile zipFile = null;

            try
            {
                zipFile = new ZipFile(stream);

                int filesOk = 0;

                foreach (ZipEntry zipEntry in zipFile)
                {
                    Stream zipEntryStream = zipFile.GetInputStream(zipEntry);
                    IFolder folder;

                    if (zipEntry.Name.EndsWith("/", StringComparison.Ordinal))
                    {
                        folder = await PCLHelper.CreateFolder(zipEntry.Name, rootFolder);
                        System.Diagnostics.Debug.WriteLine("Directory created : " + folder.Path);
                    }
                    else
                    {
                        folder = rootFolder;

                        // Create the file in file system and copy entry stream to it
                        IFile zipEntryFile = await rootFolder.CreateFileAsync(zipEntry.Name, CreationCollisionOption.FailIfExists);
                        using (Stream outPutFileStream = await zipEntryFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                        {
                            await zipEntryStream.CopyToAsync(outPutFileStream);
                            System.Diagnostics.Debug.WriteLine("File created : " + zipEntryFile.Path);
                        }
                    }

                    filesOk++;

                }

                await Task.Delay(1000);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error unzip file : " + ex);

                return false;
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }
            }
        }
    }
}
