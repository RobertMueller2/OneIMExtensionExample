using VI.DB.Entities;

namespace GuiExtensions {
    public static class ConnectDialogExtensions {
        /// <summary>
        /// Opens dialog to get a session.
        /// </summary>
        /// <returns></returns>
        public static ISession? GetOneIMSessionFromDialog() {

            var cdlg = new VI.CommonDialogs.ConnectionDialog();
            if (cdlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
                return null;
            }

            return cdlg.ConnectData.Connection.Session;
        }
    }
}
