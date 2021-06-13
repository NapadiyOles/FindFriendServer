using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace FindFriend.Data.Repositories
{
    public class PictureRepository
    {
        private string _path
        {
            get
            {
                var fullPath = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
                var projectPath = new StringBuilder();

                foreach (var folder in fullPath)
                {
                    projectPath.Append($@"{folder}\");
                    if(folder == "FindFriend") break;
                }

                projectPath.Append($@"FindFriend.Data\Pictures");

                var result = projectPath.ToString();

                if (!Directory.Exists(result)) Directory.CreateDirectory(result);

                return result;
            }
        }
        
        public void SaveImage(Image image, string uniqueId) => 
            image.Save($@"{_path}\{uniqueId}.jpg", ImageFormat.Jpeg);

        public Image LoadImage(string name) => new Bitmap($@"{_path}\{name}.jpg");

        public void DeleteImage(string name) => File.Delete($@"{_path}\{name}.jpg");

    }
}