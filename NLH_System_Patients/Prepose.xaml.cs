using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NLH_System_Patients
{
    /// <summary>
    /// Interaction logic for Prepose.xaml
    /// </summary>
    public partial class Prepose : Window
    {
        NLHSystemEntities context;
        public List<docteur> Docs { get; set; }
        public string user;
        private dossierAdmission dossier = null;
        private docteur doc = null;

        public Prepose()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context = new NLHSystemEntities();
            switch (user)
            {
                case "prep":
                    this.Title = "Préposé: Nouvelle Admission";
                    admitTitre.Content = "Nouvelle Admission";
                    btnNouvelleAdmit.Visibility = Visibility.Hidden;
                    selectDoc.DataContext = context.docteurs.ToList();
                    gridAdmit.DataContext = context.dossierAdmissions.ToList();
                    selectType.DataContext = context.typeChambres.ToList();
                    selectLit.DataContext = context.lits.ToList();
                    dateAdmit.SelectedDate = DateTime.Today;

                    // tab selection Preposer
                    tabPrepose.SelectedIndex = 0;
                    ((TabItem)tabPrepose.Items[2]).IsEnabled = false;
                    break;
                case "med":
                    this.Title = "Docteur: Donner congé";
                    selectDoc.DataContext = context.docteurs.ToList();

                    gridAdmit.DataContext = context.dossierAdmissions.ToList();
                    btnModifier.Visibility = Visibility.Hidden;

                    // tab selection Medecin
                    tabPrepose.SelectedIndex = 1;
                    ((TabItem)tabPrepose.Items[0]).IsEnabled = false;
                    ((TabItem)tabPrepose.Items[2]).IsEnabled = false;
                    break;
                case "admin":
                    this.Title = "Administrateur: Gestion des comptes";
                    gridDoc.DataContext = context.docteurs.ToList();
                    dropDepartement.DataContext = context.departements.ToList();
                    btnModMed.Visibility = Visibility.Hidden;
                    btnSuppMed.Visibility = Visibility.Hidden;
                    btnNouveauMed.Visibility = Visibility.Hidden;

                    tabPrepose.SelectedIndex = 2;
                    ((TabItem)tabPrepose.Items[0]).IsEnabled = false;
                    ((TabItem)tabPrepose.Items[1]).IsEnabled = false;
                    break;
            }
        }

        private void valider()
        {
            int nouveauDossier = 1;
            if (dossier == null)
            {
                dossier = new dossierAdmission();
                dossier.patient1 = new patient();
                nouveauDossier = 0;
            }

            try
                {
                dossier.patient1.nom = txtNom.Text;
                dossier.patient1.telephone = txtTelephone.Text;
                dossier.patient1.assurance = txtAssurance.Text;

                if (dateNaissance.SelectedDate.HasValue)
                    dossier.patient1.age = (DateTime)dateNaissance.SelectedDate;
                if (selectDoc.SelectedIndex > -1)
                    dossier.docteur1 = (docteur)selectDoc.SelectedItem;

                dossier.dateAdmission = dateAdmit.SelectedDate;
                
                if(nouveauDossier == 0)
                    context.dossierAdmissions.Add(dossier);

                context.SaveChanges();

                reinitialiser();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            // si nouvelle admission
            if ((string)admitTitre.Content == "Nouvelle Admission" & 
                txtNom.Text.Length>0 &
                (selectDoc.SelectedIndex > -1))
            {
                dossier = null;
                valider();
            }
            else if ((string)admitTitre.Content == "Modifier Admission")
            {
                valider();
            }

        }

        private void reinitialiser()
        {
            txtAdresse.Text = "";
            txtNo.Text = "";
            txtNom.Text = "";
            txtTelephone.Text = "";
            txtAssurance.Text = "";
            dateNaissance.SelectedDate = null;
            dateAdmit.SelectedDate = null;
            dateConge.SelectedDate = null;
            selectDoc.SelectedIndex = -1;

            dossier = null;
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            dossierAdmission dossier = (dossierAdmission)gridAdmit.SelectedItem;
            load_Modifier(dossier);
        }

        private void BtnNouvelleAdmit_Click(object sender, RoutedEventArgs e)
        {
            admitTitre.Content = "Nouvelle Admission";
            btnNouvelleAdmit.Visibility = Visibility.Hidden;
            reinitialiser();
        }

        private void load_Modifier(dossierAdmission admission)
        {
            dossier = admission;
            admitTitre.Content = "Modifier Admission";
            btnNouvelleAdmit.Visibility = Visibility.Visible;
            tabPrepose.SelectedIndex = 0;

            txtAdresse.Text = admission.patient1.adresse;
            txtNo.Text = Convert.ToString(admission.patient1.num);
            txtNom.Text = admission.patient1.nom;
            txtTelephone.Text = admission.patient1.telephone;
            txtAssurance.Text = admission.patient1.assurance;
            dateNaissance.SelectedDate = admission.patient1.age;
            dateAdmit.SelectedDate = admission.dateAdmission;
            dateConge.SelectedDate = admission.dateSortie;
            selectDoc.Text = admission.docteur1.utilisateur1.nom;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (user == "med")
            {
                DataGridRow row = sender as DataGridRow;
                
                if(MessageBox.Show("Voulez-vous donner conge à" + ((dossierAdmission)gridAdmit.SelectedItem).patient1.nom
                    + "?", "Conge", MessageBoxButton.YesNoCancel) 
                    == MessageBoxResult.Yes)
                {
                    ((dossierAdmission)gridAdmit.SelectedItem).dateSortie = DateTime.Today;
                    context.SaveChanges();
                }
            }

            if (user == "prep")
            {
                // load le dossier dans le formulaire
                load_Modifier((dossierAdmission)gridAdmit.SelectedItem);
                e.Handled = true;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            user = "null";
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void BtnChercher_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtRecherche.Text))
            {
                var listAdmin = from s in context.dossierAdmissions select s;
                listAdmin = listAdmin.Where(s=> s.patient1.nom.Contains(txtRecherche.Text)
                            || s.patient1.num.ToString().Contains(txtRecherche.Text)
                            || s.docteur1.utilisateur1.nom.Contains(txtRecherche.Text));

                gridAdmit.DataContext = listAdmin.ToList();
            }
        }

        private void BtnTout_Click(object sender, RoutedEventArgs e)
        {
            gridAdmit.DataContext = context.dossierAdmissions.ToList();
        }

        private void TxtRecherche_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                BtnChercher_Click(this, new RoutedEventArgs());

            }
        }

        private void SelectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectLit.DataContext = 
                (from l in context.lits where l.chambre1.typeChambre.ID == 
                ((typeChambre)selectType.SelectedItem).ID
                select l).ToList();

            var prix = Math.Round
                ( Convert.ToDecimal( ( (typeChambre)selectType.SelectedItem).tarifs), 2 );

            if (!String.IsNullOrEmpty(dossier.noAssurance))
                prix = 0;

            txtPrix.Content = (Math.Round
            (Convert.ToDecimal(((typeChambre)selectType.SelectedItem).tarifs), 2)).ToString() + "$";
            
        }

        private void BtnAjoutMed_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text) || String.IsNullOrEmpty(txtNomMed.Text))
            {
                MessageBox.Show("Vous devez remplir tout les champs pour ajouter un médecin.",
                    "nope", MessageBoxButton.OK);
            }
            else
            {
                utilisateur user = new utilisateur();
                doc = new docteur();
                user.nom = txtNomMed.Text;
                user.username = txtUsername.Text;
                user.password = passwordBox.Password;
                user.role = "Docteur";
                doc.utilisateur1 = user;
                doc.departement1 = (departement)dropDepartement.SelectedItem;
                context.docteurs.Add(doc);
                context.SaveChanges();
                gridDoc.DataContext = context.docteurs.ToList();
            }
        }

        private void GridDoc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // changer la visibilite des boutons
            btnAjoutMed.Visibility = Visibility.Hidden;
            btnNouveauMed.Visibility = Visibility.Visible;
            btnModMed.Visibility = Visibility.Visible;
            btnSuppMed.Visibility = Visibility.Visible;

            //DataGridRow row = sender as DataGridRow;
            doc = (docteur)gridDoc.SelectedItem;

            txtNomMed.Text = doc.utilisateur1.nom;
            txtUsername.Text = doc.utilisateur1.username;
            
            if(doc.departement1 != null)
                dropDepartement.Text = doc.departement1.nom;
        }

        private void BtnNouveauMed_Click(object sender, RoutedEventArgs e)
        {
            btnAjoutMed.Visibility = Visibility.Visible;
            btnNouveauMed.Visibility = Visibility.Hidden;
            btnModMed.Visibility = Visibility.Hidden;
            btnSuppMed.Visibility = Visibility.Hidden;
            doc = null;

            txtNomMed.Text = "";
            txtUsername.Text = "";
            dropDepartement.Text = "";
        }
        

        private void BtnSuppMed_Click(object sender, RoutedEventArgs e)
        {
            // verifier si le docteur apparait au dossier d'un patient
            var listDossier = (from l in context.dossierAdmissions
                               where l.docteur == doc.id
                               select l).ToArray();
            if (listDossier.Length > 0)
                MessageBox.Show("Vous ne pouvez pas supprimer un medecin qui voit des patients.",
                    "nope", MessageBoxButton.OK);
            else
                context.docteurs.Remove(doc);

            context.SaveChanges();
            gridDoc.DataContext = context.docteurs.ToList();
        }

        private void BtnModMed_Click(object sender, RoutedEventArgs e)
        {
            doc.utilisateur1.nom = txtNomMed.Text;
            doc.utilisateur1.username = txtUsername.Text;
            doc.departement1 = (departement)dropDepartement.SelectedItem;
            gridDoc.DataContext = context.docteurs.ToList();

            txtNomMed.Text = "";
            txtUsername.Text = "";
            dropDepartement.Text = "";
        }
    }
}
