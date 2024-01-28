using Common.Utility.Connection;
using mcb.main.Logic;
using mcb.main.Model;
using System.Windows.Forms;

namespace mcb.main.Forms
{
    /// <summary>Setting configuration.</summary>
    public partial class frmConfiguration : Form
    {
        #region Private declarations

        #region Variables

        private clsSettingLogic objSettingLogic = null;
        private clsSettingModel objSettingModel = null;
        #endregion

        #endregion

        #region Constructors

        /// <summary>Constructor.</summary>
        public frmConfiguration()
        {
            InitializeComponent();
        }

        #endregion

        #region Public properties

        /// <summary>Connection to database.</summary>
        public clsConnection objConnection { get; set; }

        #endregion

        #region Private methods

        /// <summary>Choose folder to import.</summary>
        /// <param name="sPath">Current path.</param>
        /// <returns>Select folder.</returns>
        private string ChooseFolder(string sPath)
        {
            // Declarations
            FolderBrowserDialog objFolderBrowserDialog = null;

            try
            {
                // Intialisations
                objFolderBrowserDialog = new FolderBrowserDialog();

                if (!string.IsNullOrEmpty(sPath))
                    objFolderBrowserDialog.SelectedPath = sPath;

                if (objFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                    return objFolderBrowserDialog.SelectedPath;
                else
                    return string.Empty;
            }
            finally
            {
                if (objFolderBrowserDialog != null)
                {
                    objFolderBrowserDialog.Dispose();
                    objFolderBrowserDialog = null;
                }
            }
        }

        #endregion

        #region Events

        /// <summary>Cancel button clicked.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button4_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>Browse import path.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button1_Click(object sender, System.EventArgs e)
        {
            textBox1.Text = ChooseFolder(textBox1.Text);
        }

        /// <summary>Browse imported path.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button2_Click(object sender, System.EventArgs e)
        {
            textBox2.Text = ChooseFolder(textBox2.Text);
        }

        /// <summary>Form loading.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void frmConfiguration_Load(object sender, System.EventArgs e)
        {
            if (objConnection == null)
                return;

            // Initialisations
            objSettingModel = new clsSettingModel();
            objSettingLogic = new clsSettingLogic(objConnection);

            objSettingModel = objSettingLogic.SelectSetting();

            if (objSettingModel != null)
            {
                textBox1.Text = objSettingModel.sImportPath;
                textBox2.Text = objSettingModel.sImportedPath;
                maskedTextBox1.Text = objSettingModel.iExecuteEvery.ToString();
            }

        }

        /// <summary>Save button clicked.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (objSettingModel == null)
                objSettingModel = new clsSettingModel();

            objSettingModel.sImportPath = textBox1.Text;
            objSettingModel.sImportedPath = textBox2.Text;
            objSettingModel.iExecuteEvery = int.Parse(maskedTextBox1.Text);

            objSettingLogic.InsertUpdateSetting(objSettingModel);
            Close();
        }

        #endregion

    }
}
