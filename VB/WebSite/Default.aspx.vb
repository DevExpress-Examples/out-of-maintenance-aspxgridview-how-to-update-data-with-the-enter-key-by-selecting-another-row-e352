Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web.ASPxEditors

Partial Public Class Grid_Editing_FastEdit_Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

	End Sub
	Protected Sub ASPxGridView1_CustomJSProperties(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewClientJSPropertiesEventArgs)
		e.Properties("cpIsEditing") = ASPxGridView1.IsEditing
	End Sub
	Protected Sub ASPxGridView1_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs)
		Dim box As ASPxTextBox = TryCast(e.Editor, ASPxTextBox)
		If box Is Nothing Then
			Return
		End If
		box.ClientSideEvents.KeyDown = "editor_KeyDown"
	End Sub
End Class
