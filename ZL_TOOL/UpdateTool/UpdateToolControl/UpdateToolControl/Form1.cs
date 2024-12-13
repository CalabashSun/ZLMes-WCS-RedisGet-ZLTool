using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateToolControl
{
    public partial class Form1 : UIForm2
    {
        private static int isSuccess = 0;
        private static bool isUpdate = false;
        public Form1()
        {
            InitializeComponent();
            // 在窗体加载时启动线程来模拟加载过程
            Task.Run(() => {
               _=SimulateLoading();
            });


            Task.Run(() => {
               _=ExtractFile();
            });

        }

        public async Task ExtractFile()
        {
            string exePath = null;
            string dominPath2 = AppDomain.CurrentDomain.BaseDirectory;
            string dominPath = Directory.GetParent(Directory.GetParent(dominPath2).FullName).FullName + "\\";
            string filePath = Path.Combine(dominPath, "Update.zip");
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(filePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        string childPath = Path.Combine(dominPath, entry.FullName);
                        if (childPath.EndsWith(".exe"))
                        {
                            exePath = childPath;
                        }
                        entry.ExtractToFile(childPath, true);

                    }
                }
                isSuccess = 1;
            }
            catch (Exception ex)
            {
                await Task.Delay(3000);
                MessageBox.Show("升级失败" + ex.Message);
                // 关闭加载窗体
                this.Invoke(new Action(() =>
                {
                    this.Close();
                }));
                isSuccess = -1;
                return;
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (exePath == null)
            {
                await Task.Delay(3000);
                MessageBox.Show("找不到可执行的更新文件！");
                // 关闭加载窗体
                this.Invoke(new Action(() =>
                {
                    this.Close();
                }));
                isSuccess = -1;
                return;
            }
            while (!isUpdate)
            {
                await Task.Delay(500);
            }
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(exePath);
            process.Start();
        }
        private async Task SimulateLoading()
        {
            // 模拟加载过程
            for (int i = 0; i <= 100; i++)
            {
                if (isSuccess == -1)
                    break;
                while (isSuccess == 0&&i>=58)
                {
                    UpdateProgress(60);
                    await Task.Delay(500);
                }
                // 更新进度条
                UpdateProgress(i);
                // 模拟加载延迟
                Thread.Sleep(50);
            }
            isUpdate = true;
            Thread.Sleep(2000);
            // 关闭加载窗体
            this.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }

        private void UpdateProgress(int value)
        {
            uiProcessBar1.Value = value;
        }

    }
}
