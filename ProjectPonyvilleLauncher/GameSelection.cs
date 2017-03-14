using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPonyvilleLauncher
{
    public partial class GameSelection : Form
    {
        public GameSelection()
        {
            InitializeComponent();
        }

        private void PPGameBtn_Click(object sender, EventArgs e)
        {
            Form1.currGame = Enums.Enums.Game.ProjectPonyville;
            this.Close();
        }

        private void GameSelection_Load(object sender, EventArgs e)
        {
        }

        private void CDGameBtn_Click(object sender, EventArgs e)
        {
            Form1.currGame = Enums.Enums.Game.CanterlotDefense;
            this.Close();
        }
    }
}