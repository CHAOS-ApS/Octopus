namespace Geckon.Octopus.Data.Interface
{
    public interface IPluginInfo
    {
        int ID{get; set;}

        string Name{get; set;}

        string Version{get; set;}

        string Description{get; set;}

        string Classname{get; set;}

        string Assembly{get; set;}

        string Filename{get; set;}

        string WriteURL{get; set;}

        string ReadURL{get; set;}

        string AssemblyIdentifier { get; }

        string PluginIdentifier { get; }

        System.DateTime CreatedDate{get; set;}
    }
}