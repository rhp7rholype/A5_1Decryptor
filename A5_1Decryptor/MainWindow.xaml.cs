using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace A5_1Decryptor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A5/1 Encryption/decryption powered by\nsona gevorg9n 303b 'МАИ ФИИТ'");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var filePath = openFileDialog.FileName;

                    var result = A5Encyptor(File.ReadAllBytes(filePath),
                        new byte[] { 0x12, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF });

                    File.WriteAllBytes(filePath, result);

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("OOOPs error, try again");
            }

        }

        private byte[] A5Encyptor(byte[] msg, byte[] key)
        {
            var st = new Stopwatch();
            st.Start();

            var msgbits = new BitArray(msg);
            A5Enc a5 = new A5Enc();
            int[] frame = new int[1];
            bool[] resbits = new bool[msgbits.Count];
            int framesCount = msgbits.Length / 228;
            
            if ((msgbits.Count % 228) != 0)
                framesCount++;
            for (int i = 0; i < framesCount; i++)
            {
                frame[0] = i;
                a5.KeySetup(key, frame);
                bool[] KeyStream = a5.A5(true);
                for (int j = 0; j < 228 && j < msgbits.Count; j++)
                {
                    var msgIndex = i * 228 + j;
                    if(msgIndex >= msgbits.Count)
                        break;
                    resbits[msgIndex] = msgbits[msgIndex] ^ KeyStream[j];
                }
            }

            st.Stop();
            Time.Content = st.ElapsedMilliseconds + " mc";
            return a5.FromBoolToByte(resbits, false);
        }
    }
}
