using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace notepad
{
    /// <summary>
    /// documentWindow.xaml 的互動邏輯
    /// </summary>
    public partial class documentWindow : Window
    {
        private Color fontbackgroundc, fontcolor, backgroundc;
        private Brush currentfontBrush = new SolidColorBrush(Colors.Black);
        private Brush currentfontFillBrush = new SolidColorBrush(Colors.White);
        private Brush currentbackgroundBrush = new SolidColorBrush(Colors.White);
        private bool isFill = true;
        public documentWindow()
        {
            InitializeComponent();
            EnumerateSystemFonts();
            EnumerateSystemFontsSize();
            Titlelable.Content = Title;
            fontcolorlable.Content = $"fontcolor：{currentfontBrush}";
            fontbackgroundcolorlable.Content = $"fontbackgroundcolor：{currentfontFillBrush} "; ;
        }
        private void EnumerateSystemFonts() //字形模式
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                fontComboBox2.Items.Add(fontFamily.Source);
                fontComboBox.Items.Add(fontFamily.Source);
            }
            fontComboBox.SelectedIndex = 0;
            fontComboBox2.SelectedIndex = 0;
        }
        private void EnumerateSystemFontsSize()
        {
            for (int i = 4; i <= 80; i += 4)
            {
                fontsizeBox.Items.Add(i);
                fontsizeBox1.Items.Add(i);
            }
            fontsizeBox1.SelectedIndex = 0;
            fontsizeBox.SelectedIndex = 0;
        }
        private void openfile(object sender, ExecutedRoutedEventArgs e) //開啟資料
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new
           Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "讀取文字檔案";//設定標題名
            openFileDialog.DefaultExt = ".rtf";//設定只顯示.rtf檔
            openFileDialog.Filter = "rtf檔案(*.rtf)|*.rtf|全部檔案(*.*)| *.* "; //找尋顯示全部的.rtf檔
           if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName; //檔案路徑
                FileStream fs = new FileStream(path, FileMode.Open);
                TextRange range = new TextRange(writetextbox.Document.ContentStart,writetextbox.Document.ContentEnd);//開啟內容
                range.Load(fs, System.Windows.DataFormats.Rtf);
                Title = openFileDialog.SafeFileName;//檔名
            }
            Titlelable.Content = Title;
        }
        private void savefile(object sender, ExecutedRoutedEventArgs e) //儲存資料
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new
           Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Title = "儲存文字檔案";
            saveFileDialog.DefaultExt = "*.rtf";
            saveFileDialog.Filter = "文字檔案(*.rtf)|*.rtf|全部檔案(*.*)|*.* "; //指找尋.rtf檔
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                TextRange range = new TextRange(writetextbox.Document.ContentStart,writetextbox.Document.ContentEnd);
                range.Save(fs, System.Windows.DataFormats.Rtf);
                Title = saveFileDialog.SafeFileName;
            }
            Titlelable.Content = Title;
        }
        private void fontComboBox_SelectionChanged(object sender,
        SelectionChangedEventArgs e)//選字型
        {
            TextSelection selectedText = writetextbox.Selection;//將圈到的資料寫入變數
            if (!selectedText.IsEmpty)//判斷是否有圈到東西
            {
                selectedText.ApplyPropertyValue(FontFamilyProperty,
               fontComboBox.SelectedItem);
                //讀取foncomobox選取的字型，改變選取的文字
            }
        }
        private void clearfont(object sender, RoutedEventArgs e)//清除
        {
            writetextbox.Document.Blocks.Clear(); //清除writetextbox上面所有的文字
        }
        private void CommandBinding_Executed(object sender,
        ExecutedRoutedEventArgs e)//關閉視窗
        {
            this.Close(); //關閉視窗
        }
        private void CommandBinding_Executed_1(object sender,
        ExecutedRoutedEventArgs e) //開新視窗
        {
            documentWindow mywindow = new documentWindow(); //設定新視窗
            mywindow.Show(); //顯示新視窗
        }
        private void fontcolorbutton_Click(object sender, RoutedEventArgs e)//字體顏色
        {
            fontcolor = GetDialogColor();//讀取選取的顏色資料
            currentfontBrush = new SolidColorBrush(fontcolor);//將顏色資料寫入變數
            fontcolorbutton.Background = currentfontBrush;//改變backgroundcolor 的背景顏色
            TextSelection selectedText = writetextbox.Selection;
            selectedText.ApplyPropertyValue(TextElement.ForegroundProperty, currentfontBrush);//將反白的文字改變顏色
            fontcolorlable.Content = $"fontcolor：{currentfontBrush}";//StatusBar顯示字形顏色
        }
        private void fontbackgroundcolor_Click(object sender, RoutedEventArgs e)//字體背景
        {
            isFill = !isFill; //改變原有的數值
            if (isFill)//判斷是否有啟動
            {
                fontbackgroundc = GetDialogColor();//讀取選取的顏色資料
                currentfontFillBrush = new SolidColorBrush(fontbackgroundc);//將顏色資料寫入變數
                fontbackgroundcolor.Background = currentfontFillBrush; //改變backgroundcolor 的背景顏色
                TextSelection selectedText = writetextbox.Selection; //紀錄反白字串
                selectedText.ApplyPropertyValue(TextElement.BackgroundProperty, currentfontFillBrush);//將反白的文字改變顏色
                fontbackgroundcolorlable.Content = $"fontbackgroundcolor：{currentfontFillBrush}";//StatusBar顯示字形顏色
            }
            else
            {
                currentfontFillBrush = null; //將舊有的顏色變成透明，將顏色資料寫入變數
                TextSelection selectedText = writetextbox.Selection;//紀錄反白字串
                selectedText.ApplyPropertyValue(TextElement.BackgroundProperty, currentfontFillBrush);//將反白的文字改變顏色
                fontbackgroundcolor.Background = new SolidColorBrush(Colors.LightGray);//改變backgroundcolor 的背景顏色
                fontbackgroundcolorlable.Content = $"fontbackgroundcolor：{currentfontFillBrush}";//StatusBar顯示字形顏色
            }
        }
        private void fontComboBox2_SelectionChanged(object sender,
        SelectionChangedEventArgs e)//選字型
        {
            TextSelection selectedText = writetextbox.Selection;//將圈到的資料寫入變數
         if (!selectedText.IsEmpty)//判斷是否有圈到東西
            {
                selectedText.ApplyPropertyValue(FontFamilyProperty,fontComboBox2.SelectedItem); //讀取foncomobox2選取的字型，改變選取的文字
            }
        }
        private void fontsizeBox_SelectionChanged(object sender,
        SelectionChangedEventArgs e)//改變文字大小
        {
            TextSelection selectedText = writetextbox.Selection;//將圈到的資料寫入變數
            if (!selectedText.IsEmpty)
            {
                selectedText.ApplyPropertyValue(TextElement.FontSizeProperty,
                Convert.ToDouble(fontsizeBox.SelectedItem));
            }
        }
        private void fontsizeBox1_SelectionChanged(object sender,SelectionChangedEventArgs e)//改變文字大小
        {
            TextSelection selectedText = writetextbox.Selection; //將圈到的資料寫入變數
            if (!selectedText.IsEmpty)
            {
                selectedText.ApplyPropertyValue(TextElement.FontSizeProperty,
               Convert.ToDouble(fontsizeBox1.SelectedItem));
            }
        }
        private void backgroundcolor_Click(object sender, RoutedEventArgs e) //背景顏色
        {
            backgroundc = GetDialogColor(); //讀取選取的顏色資料
            currentbackgroundBrush = new SolidColorBrush(backgroundc);//將顏色資料寫入變數
            backgroundcolor.Background = currentbackgroundBrush;//改變backgroundcolor 的背景顏色
            writetextbox.Background = currentbackgroundBrush;// 改變背景顏色
        }
        private Color GetDialogColor() //調色盤
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            System.Drawing.Color dlgColor = colorDialog.Color;
            return Color.FromArgb(dlgColor.A, dlgColor.R, dlgColor.G,dlgColor.B);//回傳color值
        }
    }
}
