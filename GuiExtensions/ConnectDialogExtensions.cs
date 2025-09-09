using VI.DB.Entities;
using System.Windows.Forms;

namespace GuiExtensions {
    public static class ConnectDialogExtensions {
        /// <summary>
        /// Opens dialog to get a session.
        /// </summary>
        /// <returns></returns>
        public static ISession? GetOneIMSessionFromDialog() {

            var cdlg = new VI.CommonDialogs.ConnectionDialog();
            if (cdlg.ShowDialog() != DialogResult.OK) {
                return null;
            }

            return cdlg.ConnectData.Connection.Session;
        }
    }
}
