using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

public partial class Grid_Editing_FastEdit_Default : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {

	}
	protected void ASPxGridView1_CustomJSProperties(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewClientJSPropertiesEventArgs e) {
		e.Properties["cpIsEditing"] = ASPxGridView1.IsEditing;
	}
	protected void ASPxGridView1_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e) {
		ASPxTextBox box = e.Editor as ASPxTextBox;
		if(box == null) return;
		box.ClientSideEvents.KeyDown = "editor_KeyDown";
	}
}
