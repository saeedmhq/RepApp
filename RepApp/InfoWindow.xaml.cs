﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RepApp
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        private static string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private static string folderPath = appPath + "/نتایج";
        private static string filePath = folderPath + "/info.csv";
        private static int currId = 0;

        public InfoWindow()
        {
            InitializeComponent();
        }

        public static List<string> ReadInCSV(string filePath)
        {
            List<string> result = new List<string>();
            string value;
            using (TextReader fileReader = File.OpenText(filePath))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                {
                    
                    
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                    {
                        result.Add(value);
                    }
                }
            }
            return result;
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            
            //var csv = new StringBuilder();
            currId += 1;
            var name = tb_name.Text;
            var age = tb_age.Text;
            var gender = "";
            if (rb_female.IsChecked == true) gender = "زن";
            else if (rb_male.IsChecked == true) gender = "مرد";
            var height = tb_height.Text;
            var weight = tb_weight.Text;
            var education = cb_education.Text;
            
            var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                currId.ToString(), name, age, gender, height, weight, education);
            //csv.AppendLine(newLine);
            //File.AppendAllText(filePath, csv.ToString(), Encoding.UTF8);

            EvalWindow evalWin = new EvalWindow();
            EvalWindow.info = newLine;
            EvalWindow.info_filePath = filePath;
            EvalWindow.currId = currId;
            evalWin.Show();
            this.Close();
        }

        private void Win_Info_Loaded(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);

            if (File.Exists(filePath) == false)
            {
                var csv = new StringBuilder();
                var title = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                    "ID", "نام", "سن", "جنسیت", "قد", "وزن", "تحصیلات");
                csv.AppendLine(title);
                File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
            }
            else
            {
                using (TextReader fileReader = File.OpenText(filePath))
                {
                    var csv = new CsvReader(fileReader);
                    var records = csv.GetRecords<dynamic>();
                    currId = records.Count<dynamic>();
                }
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
