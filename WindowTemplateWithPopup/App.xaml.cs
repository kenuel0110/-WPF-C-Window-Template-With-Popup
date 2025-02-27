﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace WindowTemplateWithPopup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        //Change Language
        public static void SelectCulture(string culture)
        {
            MainWindow mainWindow = App.Current.MainWindow as MainWindow;
            if (String.IsNullOrEmpty(culture))
                return;

            //Copy all MergedDictionarys into a auxiliar list.
            var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();

            //Search for the specified culture.     
            string requestedCulture = string.Format("/Strings/Strings.{0}.xaml", culture);

            var resourceDictionary = dictionaryList.FirstOrDefault(d =>
            d.Source != null &&
            !string.IsNullOrEmpty(d.Source.OriginalString) &&
            d.Source.OriginalString == requestedCulture);

            if (resourceDictionary == null)
            {
                // Если не найдено, выбираем наш язык по умолчанию.
                requestedCulture = "/Strings/Strings.en-EN.xaml";
                resourceDictionary = dictionaryList.FirstOrDefault(d =>
                    d.Source != null &&
                    !string.IsNullOrEmpty(d.Source.OriginalString) &&
                    d.Source.OriginalString == requestedCulture);
            }

            //If we have the requested resource, remove it from the list and place at the end.     
            //Then this language will be our string table to use.      
            if (resourceDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            //Inform the threads of the new culture.     
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);


            mainWindow.systemLanguage = culture;
        }

        public static List<string> availableCultures()
        {
            List<string> availableCultures = new List<string>();
            var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();
            try
            {
                foreach (ResourceDictionary dictionary in dictionaryList)
                {
                    string source = dictionary.Source.OriginalString;
                    if (source.StartsWith("/Strings/Strings.") && source.EndsWith(".xaml"))
                    {
                        string cultureCode = source.Substring(17, source.Length - 22); // Extract culture code
                        availableCultures.Add(cultureCode);
                    }
                }
            }
            catch { }
            return availableCultures;
        }
    }
}
