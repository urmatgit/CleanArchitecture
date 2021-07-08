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
            const string TagFieldLow = "<!FieldName>";
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

                    var subfullPath = CreateDir(fullPath, "Export");
                    WriteFile(subfullPath, "Export", "sQuery");

                    subfullPath = CreateDir(fullPath, "GetAll");
                    WriteFile(subfullPath, "GetAll", "sQuery");
                    WriteFile(subfullPath, "GetAll", "sResponse",otherResname:"GetAllResponse");

                    subfullPath = CreateDir(fullPath, "GetById");
                    WriteFile(subfullPath, "Get", "ByIdQuery");
                    WriteFile(subfullPath, "Get", "Response", otherResname: "GetResponse");

                    //Mapping
                    fullPath = Path.Combine(MainPath, appPath, "Mappings");
                    WriteFile(fullPath, "Profile",useResNameToFile: false,endPrefix: "Profile");

                    // Server
                    //Controller
                      fullPath = Path.Combine(MainPath, @"Server\Controllers\v1\Catalog");
                    WriteFile(fullPath, "Controller", useResNameToFile: false, endPrefix: "sController");

                    //Add Entity Class in DbContext
                    fullPath = Path.Combine(MainPath, @"Infrastructure\Contexts", "BlazorHeroContext.cs");
                    AddLineToFile(fullPath, "//TODO add entities", $"DbSet<{FieldName}> {FieldName}s "+ "{ get;set; }");

                    //Add Repository Changes (as per required Logic)
                      fullPath = Path.Combine(MainPath, appPath, @"Interfaces\Repositories");
                    WriteFile(fullPath, "I", endPrefix: "Repository");
                    fullPath = Path.Combine(MainPath, @"Infrastructure\Repositories");
                    WriteFile(fullPath, "Repository",useResNameToFile: false,  endPrefix: "Repository");

                    //Permissions
                    //E:\Git\Last\CleanArchitecture\src\Shared\Constants\Permission\Permissions.cs
                    fullPath = Path.Combine(MainPath, @"Shared\Constants\Permission\Permissions.cs");
                    AddLineToFile(fullPath, "//TODO Add permissions", GetStringFromRes("Permission"));
                    
                    // Add Cache key
                    //E:\Git\Last\CleanArchitecture\src\Shared\Constants\Application\ApplicationConstants.cs
                    fullPath = Path.Combine(MainPath, @"Infrastructure\Contexts", "BlazorHeroContext.cs");
                    AddLineToFile(fullPath, "//TODO Add cache key", $"public const string GetAll{FieldName}sCacheKey = \"all - {FieldName.ToLower()}s\";");

                //Client.Infrastructure
                //(Add Folder XyzManager in Manager Folder)
                //E:\Git\Last\CleanArchitecture\src\Client.Infrastructure\Managers\Catalog\Brand\BrandManager.cs
                fullPath = Path.Combine(MainPath, @"Client.Infrastructure\Managers\Catalog", FieldName);
                 CreateDir(fullPath);
                WriteFile(fullPath, "IManager", useResNameToFile: false, endPrefix: "Manager",startPrefix: "I");
                    WriteFile(fullPath, "Manager", useResNameToFile: false, endPrefix: "Manager");

                    //Add XyzEndPoint.cs in Routes Folder (http link to the controller) Link name should match the controller Name.
                    fullPath = Path.Combine(MainPath, @"Client.Infrastructure\Routes");
                    WriteFile(fullPath, "Endpoints", useResNameToFile: false, endPrefix: "sEndpoints");
                    //Add Link to SideBar in NavMenu.razor
                    //E:\Git\Last\CleanArchitecture\src\Client\Shared\NavMenu.razor
                    fullPath = Path.Combine(MainPath, @"Client\Shared\NavMenu.razor");
                    //TODO Add _canViewProperty
                    AddLineToFile(fullPath, "//TODO Add _canViewProperty", $"private bool _canView{FieldName}s;");
                    
                    AddLineToFile(fullPath, "//TODO Add _canView", $"_canView{FieldName}s = (await _authorizationService.AuthorizeAsync(_authenticationStateProviderUser, Permissions.{FieldName}s.View)).Succeeded;");

                    //@*//TODO add to menu*@
                    AddLineToFile(fullPath, "@*//TODO add to menu*@",
                       $"@if(_canViewUser{FieldName}s) \n" +"{"+
                       $"\n <MudNavLink Href = \"/catalog/user{ FieldName.ToLower()}s\" Icon = \"@Icons.Material.Outlined.CallToAction\">" +
                       $"@_localizer[\"User {FieldName.ToLower()}s\"] \n </ MudNavLink>"+"\n}");

                    //Client
                    //E:\Git\Last\CleanArchitecture\src\Client\Pages\Catalog
                    fullPath = Path.Combine(MainPath, @"Client\Pages\Catalog");
                    fullPath= CreateDir(fullPath, FieldName);
                    WriteFile(fullPath, "razor", useResNameToFile: false, endPrefix: "s",ext: ".razor");
                    WriteFile(fullPath, "razor.cs", useResNameToFile: false, endPrefix: "s", ext: ".razor.cs");

                    //@*//TODO add import*@
                    AddLineToFile(fullPath, "@*//TODO add import*@", $"@using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.{FieldName};");


                }
                catch (Exception er)
                {
                    Result = false;
                    Console.WriteLine(er.Message);
                }
                return Result;
            }
            private string GetStringFromRes(string resName)
            {
                var entity = Resource.ResourceManager.GetString( resName , Resource.Culture).Replace(TagField, FieldName);
                return entity;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <param name="afterLine"></param>
            /// <param name="newLine"></param>
            /// <returns></returns>
            private bool AddLineToFile(string path, string afterLine, string newLine)
            {
                bool Result = true;
                try
                {
                    string body = File.ReadAllText(path);
                    int index = body.IndexOf(afterLine);
                    if (index > 0)
                    {
                        body.Insert(index + afterLine.Length,$"{System.Environment.NewLine}{newLine}{System.Environment.NewLine}");

                        File.WriteAllText(path, body);
                    }
                }
                catch (Exception er)
                {
                    Result = false;
                    Console.WriteLine(er.Message);
                }
                return Result;
            }
            /// <summary>
            ///  create file {path}\{startPrefix}{resName}{FieldName}{endPrefix}{ext}
            /// </summary>
            /// <param name="path"></param>
            /// <param name="resName"></param>
            /// <param name="endPrefix"></param>
            /// <param name="startPrefix"></param>
            /// <param name="useResNameToFile"></param>
            /// <returns></returns>
            private bool WriteFile(string path,string resName,string endPrefix="",string startPrefix="", bool useResNameToFile=true,string otherResname="",string ext=".cs")
            {
                bool Result = true;
                try
                {
                    var entity = Resource.ResourceManager.GetString(string.IsNullOrEmpty(otherResname) ? resName : otherResname, Resource.Culture)
                        .Replace(TagField, FieldName)
                        .Replace(TagFieldLow,FieldName.ToLower());
                         
                    string filename = $"{startPrefix}{ (useResNameToFile ? resName : "")}{FieldName}{endPrefix}{ext}";
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

