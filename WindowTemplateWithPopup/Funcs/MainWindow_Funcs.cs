using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace WindowTemplateWithPopup.Funcs
{
    class MainWindow_Funcs
    {
        public MainWindow mainWindow = App.Current.MainWindow as MainWindow;

        //Checking and automatically creating folders
        public void chkAndCreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        //Checking and automatically creating files
        public void chkAndCreateFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
        }
        //Checking that the program is running on the computer for the first time
        public void chkFirstStart(string folder, string file_name)
        {

            if (!File.Exists($"{folder}\\{file_name}"))
                create_json(file_name);
            else
            {
                chkAndCreateFolder(Path.GetDirectoryName($"{folder}\\{file_name}"));
                chkAndCreateFile($"{folder}\\{file_name}");
            }

        }

        //Saving a settings file
        public void saveJSONSetting(Classes.Data_Classes.Class_JSON_Setting json_Setting)
        {
            string settingsString_ = "";
            if (json_Setting.maximilize_window == Classes.Enums.WindowState.Normal)
                settingsString_ = JsonSerializer.Serialize(json_Setting, new JsonSerializerOptions { WriteIndented = true });
            else if (json_Setting.maximilize_window == Classes.Enums.WindowState.Maximilize)
            {
                settingsString_ = JsonSerializer.Serialize(
                    new Classes.Data_Classes.Class_JSON_Setting()
                    {
                        maximilize_window = json_Setting.maximilize_window,
                        size_window = new List<double>() { 450, 800 }
                    }, new JsonSerializerOptions { WriteIndented = true });
            }
            File.WriteAllText("settings\\settings.json", settingsString_);
        }

        //Generating a JSON file depending on the file
        public void create_json(string file_name)
        {
            switch (file_name)
            {
                case "settings.json":
                    Classes.Data_Classes.Class_JSON_Setting json_Setting = new Classes.Data_Classes.Class_JSON_Setting();

                    string settingsString = JsonSerializer.Serialize(json_Setting, new JsonSerializerOptions { WriteIndented = true });

                    chkAndCreateFolder("settings");
                    File.WriteAllText($"settings\\{file_name}", settingsString);
                    break;

                case "temp.json":
                    chkAndCreateFolder(Path.GetDirectoryName("Temp\\temp.json"));
                    Classes.Data_Classes.Class_JSON_Temp json_temp = new Classes.Data_Classes.Class_JSON_Temp
                    {
                        //Random path
                        path = $"{Path.Combine(Path.GetRandomFileName(), Path.GetRandomFileName())}",
                    };

                    string tempString = JsonSerializer.Serialize(json_temp, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText("Temp\\temp.json", tempString);
                    break;
            }
        }

        //Opening a settings file
        public Classes.Data_Classes.Class_JSON_Setting openJSONSetting()
        {
            FileStream file = null;
            try
            {
                file = new FileStream($"settings\\settings.json", FileMode.OpenOrCreate);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                Encoding.Default.GetString(buffer);
                Classes.Data_Classes.Class_JSON_Setting settings = JsonSerializer.Deserialize<Classes.Data_Classes.Class_JSON_Setting>(buffer);
                return settings;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { file?.Close(); }
            return null;
        }

        //Remove temp used to pass information between pages
        public void removeTemp()
        {
            Directory.Delete("Temp", true);
        }


        //Creating a temp file, for example, it remembers the last path
        public void saveTemp(Classes.Data_Classes.Class_JSON_Temp new_temp)
        {
            string temp = JsonSerializer.Serialize(new_temp, new JsonSerializerOptions { WriteIndented = true });
            using (StreamWriter sw = new StreamWriter("Temp\\temp.json"))
            {
                sw.Write(temp);
            }
        }
    }
}
