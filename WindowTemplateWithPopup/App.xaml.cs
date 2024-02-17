﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
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
            if (String.IsNullOrEmpty(culture))
                return;

            //Copy all MergedDictionarys into a auxiliar list.
            var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();

            //Search for the specified culture.     
            string requestedCulture = string.Format("/Strings/Strings.{0}.xaml", culture);

            try
            {
                var resourceDictionary = dictionaryList.
                    FirstOrDefault(d => d.Source.OriginalString == requestedCulture);

                if (resourceDictionary == null)
                {
                    //If not found, select our default language.             
                    requestedCulture = "/Strings/Strings.en-EN.xaml";
                    resourceDictionary = dictionaryList.
                        FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
                }

                //If we have the requested resource, remove it from the list and place at the end.     
                //Then this language will be our string table to use.      
                if (resourceDictionary != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                    Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                }
            }
            catch { }

            //Inform the threads of the new culture.     
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            MainWindow mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.systemLanguage = culture;
            mainWindow.UpdateLayout();
        }

    }
}
