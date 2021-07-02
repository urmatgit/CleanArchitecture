using System;
using System.IO;

namespace GeneratePage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Generate generator = new Generate("TestType");
            Console.WriteLine("Domain: "+ generator.Domain());
            Console.WriteLine("Xyz in Features: " + generator.Application());
            Console.ReadLine();
        }
        class Generate
        {
            const string MainPath = @"C:\Git\Last\CleanArchitecture\src\";
            const string TagField = "<FieldName>";
            string FieldName { get; set; }
            public Generate(string fieldname)
            {
                FieldName = fieldname;
            }

            public bool Domain()
            {
                string domainPath = @"Domain\Entities\Catalog\";
                return WriteFile(Path.Combine(MainPath, domainPath), "DomainEntity",useResNameToFile: false);
                 
            }
            private string FieldNameWithS
            {
                get
                {
                    return $"{FieldName }s";
                }
            }
            public bool Application()
            {
                string appPath = @"Application\";

                bool Result = true;
                try
                {
                    //Features
                    string fullPath = Path.Combine(MainPath, appPath, "Features", FieldNameWithS);
                    CreateDir(fullPath);
                     fullPath = CreateDir(fullPath, "Commands");

                    var PathCommand = CreateDir(fullPath, "AddEdit");
                    
                    //file AddEditCommand
                    WriteFile(PathCommand, "AddEdit", "Command");
                    //Delete
                    PathCommand = CreateDir(fullPath, "Delete");
                    
                    WriteFile(PathCommand, "Delete", "Command");
                    //Queries
                    fullPath = Path.Combine(MainPath, appPath, "Features", FieldNameWithS);
                    fullPath = CreateDir(fullPath, "Queries");

                }
                catch (Exception er)
                {
                    Result = false;
                    Console.WriteLine(er.Message);
                }
                return Result;
            }
            private bool WriteFile(string path,string resName,string prefix="",bool useResNameToFile=true)
            {
                bool Result = true;
                try
                {
                    var entity = Resource.ResourceManager.GetString(resName, Resource.Culture).Replace(TagField, FieldName);
                    string filename = $"{ (useResNameToFile ? resName : "")}{FieldName}{prefix}.cs";
                    File.WriteAllText(Path.Combine( path,filename), entity);
                }
                catch (Exception er)
                {
                    Result = false;
                    Console.WriteLine(er.Message);
                }
                return Result;
            }
            private string CreateDir(string fullPath,string subDir="")
            {
                if (!string.IsNullOrEmpty(subDir))
                    fullPath= Path.Combine(fullPath, subDir);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                return fullPath;
            }
        }


    }
}

