using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SysProgHW1;

public partial class MainWindow : Window
{
    public ObservableCollection<Process> Processes { get; set; }
    public ObservableCollection<Process> BlackList { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Processes = new(Process.GetProcesses());
        BlackList = new();

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchTxt.Text))
        {
            MessageBox.Show("Enter process name");
            return;
        }

        try
        {
            ProcessStartInfo startInfo = new(SearchTxt.Text);

            startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            Process.Start(startInfo);
            SearchTxt.Text = String.Empty;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            Dispatcher.Invoke(() =>
            {
                Processes.Clear();
                foreach (var p in Process.GetProcesses())
                    Processes.Add(p);
            }
            );

        }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if (List.SelectedItem is null)
            return;

        if (List.SelectedItem is Process process)
        {
            try
            {
                process.Kill();
                Processes.Remove(process);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    Processes.Clear();
                    foreach (var p in Process.GetProcesses())
                        Processes.Add(p);
                }
                );

            }

        }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        if (List.SelectedItem is null)
            return;

        if (List.SelectedItem is Process p)
        {
            if (!BlackList.Contains(p))
            {
                BlackList.Add(p);
                Thread.Sleep(1000);
                if (BlackList.Any(s => s == p))
                {
                    try
                    {
                        p.Kill();
                        Processes.Remove(p);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message); ;
                    }
                }

            }

        }
    }
}
