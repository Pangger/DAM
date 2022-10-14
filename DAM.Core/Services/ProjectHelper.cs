using DAM.Core.Models.SolutionModels;
using DAM.Core.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DAM.Core.Services
{
    public class ProjectHelper : IProjectHelper
    {
        public List<Property> GetModelProperties(Model model)
        {
            string projectDllPath = Directory.GetFiles(model.Project.FolderPath, $"{model.Project.Name}.dll", SearchOption.AllDirectories).FirstOrDefault();
            List<Property> result = new List<Property>();
            if (!string.IsNullOrEmpty(projectDllPath))
            {
                var projectAssembly = Assembly.LoadFrom(projectDllPath);
                var modelName = Path.GetFileNameWithoutExtension(Path.Combine(model.Project.Name, Path.GetRelativePath(model.Project.FolderPath, model.Path)).Replace('\\', '.'));

                var type = projectAssembly.GetType(modelName, false, true);
                if (type != null)
                    result = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Select(s => new Property(s.Name, s.PropertyType)).ToList();
            }

            return result;
        }
    }
}
