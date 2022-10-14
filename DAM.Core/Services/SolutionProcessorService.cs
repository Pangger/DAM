using DAM.Core.Models.SolutionModels;
using DAM.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAM.Core.Services
{
    public class SolutionProcessorService : ISolutionProcessorService
    {
        private string _currentSolutionFolderPath;
        private Project _currentProject;
        public async Task<Solution> GetSolutionProjects(string solutionPath)
        {
            using StreamReader sr = new StreamReader(solutionPath);

            _currentSolutionFolderPath = Path.GetDirectoryName(solutionPath);

            string solutionContent = await sr.ReadToEndAsync();

            List<Item> items = new List<Item>();

            int index = solutionContent.IndexOf("Project", StringComparison.Ordinal);


            while (index != -1 && solutionContent.IndexOf("EndProject", StringComparison.Ordinal) != -1)
            {
                int endIndex = solutionContent.IndexOf("EndProject", StringComparison.Ordinal) + 10;

                _currentProject = GetProject(solutionContent.Substring(index, endIndex - index - 1));

                items.Add(GetCurrentProjectItems());

                solutionContent = solutionContent.Substring(endIndex, solutionContent.Length - endIndex - 1);
                index = solutionContent.IndexOf("Project", StringComparison.Ordinal);
            }

            return new Solution()
            {
                Name = Path.GetFileName(solutionPath),
                Path = solutionPath,
                Items = items.OrderBy(o => o.Name).ToList()
            };
        }

        private Project GetProject(string projectContent)
        {
            var projectParts = projectContent.Split(',');
            int nameStartIndex = projectParts[0].IndexOf('=') + 1;
            string projectPath = Path.Combine(_currentSolutionFolderPath, projectParts[1].Trim().Trim('"'));
            string projectFolderPath = Path.GetDirectoryName(projectPath);

            return new Project
            {
                Name = projectParts[0].Substring(nameStartIndex, projectParts[0].Length - nameStartIndex).Trim()
                    .Trim('"'),
                Path = projectParts[1].Trim().Trim('"'),
                FolderPath = projectFolderPath
            };
        }

        private ProjectDirectory GetCurrentProjectItems() => new ProjectDirectory()
        {
            Name = _currentProject.Name,
            Path = _currentProject.Path,
            Items = GetItems(_currentProject.FolderPath)
        };

        public List<Item> GetItems(string path)
        {
            var items = new List<Item>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.GetDirectories().Where(w => !w.Name.Equals("bin") && !w.Name.Equals("obj")))
            {
                var item = new ProjectDirectory
                {
                    Name = directory.Name,
                    Path = directory.FullName,
                    Items = GetItems(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in dirInfo.GetFiles("*.cs"))
            {
                var item = new Model
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Project = _currentProject
                };

                items.Add(item);
            }

            return items;
        }
    }
}
