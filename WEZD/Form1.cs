using System.Diagnostics;
using System.Text;
#pragma warning disable CA1416

namespace WEZD
{
    public partial class Form1 : Form
    {
        public bool Install { get; private set; }
        private List<RadioButton> officeRB;
        //private Label statusLabel;

        public Form1()
        {
            InitializeComponent();
            InitializeRadioButtons();
        }

        private async void InstallButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("bouton start début install");

            // Désactiver tous les contrôles
            SetControlsEnabled(false);

            Functions func = new();
            await func.Install(this); // Appeler Install en respectant son asynchronisme

            // Réactiver tous les contrôles après l'installation
            SetControlsEnabled(true);
            UpdateStatusLabel("Installation terminée.");
        }

        private void CancelButton_Click_1(object sender, EventArgs e)
        {
            //Close();
            Application.Exit();
        }

        public void UpdateStatusLabel(string text)
        {
            if (labelStatus.InvokeRequired)
            {
                // Exécute la mise à jour sur le thread principal
                labelStatus.Invoke(new Action(() => labelStatus.Text = text));
            }
            else
            {
                // Si on est déjà sur le thread principal, on peut mettre à jour directement
                labelStatus.Text = text;
            }
        }

        private void SetControlsEnabled(bool isEnabled)
        {
            foreach (Control control in Controls)
            {
                if (control is Button || control is CheckBox) // Désactiver uniquement les boutons et checkboxes
                {
                    control.Enabled = isEnabled;
                }
            }
        }

        private void UseProdKey_CheckedChanged(object sender, EventArgs e)
        {
            ProductKey.Enabled = UseProdKey.Checked;
            if (!UseProdKey.Checked)
            {
                ProductKey.Text = ""; // Efface la clé si désactivé
            }
        }

        private void ProductKey_TextChanged(object sender, EventArgs e)
        {
            // Liste des caractères interdits dans les clés Microsoft
            string cprod = "ABCDEFGHJKLMPRTVWXY0123456789"; // Exclut I, O, Q, S, U, Z
            string rw = ProductKey.Text.Replace("-", ""); // Supprime les tirets existants

            // Filtre les caractères invalides
            StringBuilder ft = new StringBuilder();
            foreach (char c in rw.ToUpper())
            {
                if (cprod.Contains(c))
                {
                    ft.Append(c);
                }
            }

            // Limite la saisie à 25 caractères
            if (ft.Length > 25)
            {
                ft.Length = 25;
            }

            // Ajoute les tirets pour respecter le format XXXXX-XXXXX-XXXXX-XXXXX-XXXXX
            StringBuilder fk = new StringBuilder();
            for (int i = 0; i < ft.Length; i++)
            {
                if (i > 0 && i % 5 == 0)
                {
                    fk.Append("-");
                }
                fk.Append(ft[i]);
            }

            // Met à jour le champ texte avec le format correct
            ProductKey.Text = fk.ToString();
            ProductKey.SelectionStart = ProductKey.Text.Length; // Place le curseur à la fin
        }

        private void InitializeRadioButtons()
        {
            officeRB = new List<RadioButton>
            {
                Std2016, PPlus2016, Std2019, PPlus2019,
                Std2021, PPlus2021, Std2024, PPlus2024
            };

            // Ajoute un événement commun pour tous les RadioButton
            foreach (var rbi in officeRB)
            {
                rbi.CheckedChanged += OfficeRadioButton_CheckedChanged;
            }
        }

        private void OfficeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadio = sender as RadioButton;

            if (selectedRadio != null && selectedRadio.Checked)
            {
                // Désélectionne tous les autres boutons
                foreach (var rb in officeRB)
                {
                    if (rb != selectedRadio)
                    {
                        rb.Checked = false;
                    }
                }
            }
        }
    }
}
