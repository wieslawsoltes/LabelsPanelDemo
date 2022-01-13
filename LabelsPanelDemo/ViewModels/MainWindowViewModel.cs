using System;
using System.Collections.Generic;
using System.Text;

namespace LabelsPanelDemo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<string> Labels { get; }

        public MainWindowViewModel()
        {
            Labels = new List<string>();

            for (var i = 0; i < 20; i++)
            {
                Labels.Add($"Label {i}");
            }
        }
    }
}
