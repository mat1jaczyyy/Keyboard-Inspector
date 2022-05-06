using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    partial class DeltaForm: Form {
        public DeltaForm(List<int> freq) {
            InitializeComponent();

            for (int i = 0; i < freq.Count; i++) {
                chart.Series[0].Points.AddXY(i, freq[i]);
            }

            chart.Titles.Add($"n = {freq.Sum()}");
        }
    }
}