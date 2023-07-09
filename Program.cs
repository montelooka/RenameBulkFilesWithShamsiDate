using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.WriteLine("Welcome to Rename Project!");
            Console.WriteLine("This app rename all the file in your folder based on creation shamsi date!");
            Console.WriteLine("Enter your folder address:");
           string folderDirectory= Console.ReadLine();
           
            try
            {

                // string strdirectory = @"G:\Hamideh\Project\SSIS\2022\destination2";
                while(!MainFunction(folderDirectory))
                {
                    Console.WriteLine("Directory Not Found! Enter anotther Directory:");
                    folderDirectory = Console.ReadLine();
                    MainFunction(folderDirectory);
                }
              
                Console.WriteLine("Successful!");
                Console.ReadLine(); 

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                // MessageBox.Show(e2.Message);
            }
            


            // Function to get the new file name        
            Boolean MainFunction(string strFolder)
            {
                if (Directory.Exists(strFolder))
                {
                    string[] fileList = Directory.GetFiles(strFolder, "*.*", SearchOption.AllDirectories);
                    int i = 1;
                    foreach (string strfile in fileList)
                    {
                        string newFileName = GetNewFileName(strfile, i);


                        File.Move(strfile, newFileName);
                        i = i + 1;
                    }
                    
                    return true;
                }
                else
                {
                    return false;
                       

                }
            }


            string GetNewFileName(string strfile, int i)
            {
                /*
                DateTime md = System.IO.File.GetLastWriteTime(strfile);
                string shortDate = System.IO.File.GetLastWriteTime(strfile).ToString ("yyyyMMdd");
                System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
               return string.Concat(Path.GetDirectoryName(strfile),"\\", shamsi.GetYear(md).ToString("0000"),shamsi.GetMonth(md).ToString("00"),shamsi.GetDayOfMonth(md).ToString("00"), "_",i.ToString(), Path.GetExtension(strfile));
               */
                string fname = System.IO.Path.GetFileName(strfile);
                // DateTime md = DateTime.ParseExact(fname.Substring(0, fname.IndexOf('_')),"yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                string rDate = GetImageDateTaken(strfile);
                DateTime md;
                if (rDate != null && rDate != "")
                {
                    md = DateTime.Parse(rDate);
                }
                else
                { md = System.IO.File.GetLastWriteTime(strfile); }

                System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
                return string.Concat(Path.GetDirectoryName(strfile), "\\", shamsi.GetYear(md).ToString("0000"), shamsi.GetMonth(md).ToString("00"), shamsi.GetDayOfMonth(md).ToString("00"), "_", i.ToString(), Path.GetExtension(strfile));




            }

             string GetImageDateTaken(string imagePath)
            {
                string dateTaken = string.Empty;

                try
                {
                    FileInfo fileInformation = new FileInfo(imagePath);
                    FileStream fileStream = new FileStream(fileInformation.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (Path.GetExtension(imagePath) != ".mp4")
                    {
                        System.Windows.Media.Imaging.BitmapSource imgeSource = BitmapFrame.Create(fileStream);
                        BitmapMetadata imageMetaData = (BitmapMetadata)imgeSource.Metadata;
                        dateTaken = imageMetaData.DateTaken;

                    }
                    else
                    {
                        ShellObject shell = ShellObject.FromParsingName(imagePath);

                        dateTaken = shell.Properties.System.Media.DateEncoded.Value.ToString();
                    }
                    fileStream.Dispose();

                }
                catch (Exception e)
                {
                  //  MessageBox.Show(e.Message);
                    return dateTaken;
                }

                return dateTaken;
            }
        }

       
  
    }
}
