using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Windows.Documents;
using WAVE.Objects;

namespace WAVE.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BackgroundWorker _bgWorker = new BackgroundWorker();

        private List<string> _knownProcesses;

        public List<string> KnownProcesses
        {
            get => _knownProcesses;

            set
            {
                _knownProcesses = value;

                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<string> _findings;

        public ObservableCollection<string> Findings
        {
            get => _findings;

            set
            {
                _findings = value;

                OnPropertyChanged();
            }
        }

        private Rootobject cves;

        private string DataFile = Path.Combine(AppContext.BaseDirectory, "Data/nvdcve-1.1-2019.json");

        public MainViewModel()
        {
            Findings = new ObservableCollection<string>();

            KnownProcesses = new List<string>();

            cves = JsonSerializer.Deserialize<Rootobject>(File.ReadAllText(DataFile));

            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            
            _bgWorker.RunWorkerAsync();
        }

        private void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(60000);

            _bgWorker.RunWorkerAsync();
        }

        private void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                try
                {
                    using (var sha1Managed = SHA1.Create())
                    {
                        var hash = BitConverter.ToString(sha1Managed.ComputeHash(File.ReadAllBytes(process.MainModule.FileName)));

                        if (KnownProcesses.Contains(hash))
                        {
                            Findings.Add($"{hash} computed");

                            continue;
                        }


                        var results = cves.CVE_Items.Where(a =>
                            a.cve.description.description_data.ToString().Contains(process.MainModule.ModuleName)).Select(b => b.cve.description.description_data.ToString()).ToList();

                        if (!results.Any())
                        {
                            continue;
                        }

                        Findings.Add($"{process.MainModule.ModuleName}: {string.Join(',', results)}");
                    }
                }
                catch
                {

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
