using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    partial class Chart: UserControl {
        public Chart() {
            InitializeComponent();
            Area.ScrollBar = ScrollBar;
        }

        [Category("Appearance")]
        [Description("The color of the chart line.")]
        [DefaultValue(typeof(Color), "65, 140, 240")]
        public Color LineColor {
            get => Area.ForeColor;
            set => Area.ForeColor = value;
        }
    }
}
