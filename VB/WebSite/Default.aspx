<%-- BeginRegion Page setup --%>
<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="Grid_Editing_FastEdit_Default" %>
<%@ Register Assembly="DevExpress.Web.v13.1" Namespace="DevExpress.Web" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v13.1" Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%-- EndRegion --%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>Confirm a row edition with the enter key / by selecting another row</title>
<script type="text/javascript">
function grid_RowDblClick(s, e) {
	s.StartEditRow(e.visibleIndex);
}
function grid_FocusedRowChanged(s, e) {
	if(s.cpIsEditing)
		s.UpdateEdit();
}
function editor_KeyDown(s, e) {		
	switch(e.htmlEvent.keyCode) {
		case 13: 
			grid.UpdateEdit();
			break;
		case 27:
			grid.CancelEdit();
			break;
	}
}
function grid_EndCallback(s, e) {
	var edit = s.GetEditor(1);
	if(edit) {
		edit.SelectAll();
		edit.SetFocus();				
	}			
}
</script>    
</head>
<body>
    <form id="form1" runat="server">
		<dxwgv:ASPxGridView ID="ASPxGridView1" runat="server" ClientInstanceName="grid"
			DataSourceID="ObjectDataSource1" KeyFieldName="Id" OnCustomJSProperties="ASPxGridView1_CustomJSProperties" OnCellEditorInitialize="ASPxGridView1_CellEditorInitialize">
			<SettingsBehavior AllowFocusedRow="True" />
			<SettingsEditing Mode="Inline" />
			<Columns>
				<dxwgv:GridViewDataColumn FieldName="Id" />
				<dxwgv:GridViewDataColumn FieldName="ProductId" />
				<dxwgv:GridViewDataColumn FieldName="Quantity" />
				<dxwgv:GridViewDataColumn FieldName="Price" />
			</Columns>
			<ClientSideEvents
				EndCallback="grid_EndCallback"
				RowDblClick="grid_RowDblClick"
				FocusedRowChanged="grid_FocusedRowChanged" />
		</dxwgv:ASPxGridView>
	    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"
            SelectMethod="GetItems" TypeName="OrderItemsProvider" UpdateMethod="ItemUpdate" InsertMethod="ItemInsert" DeleteMethod="ItemDelete" ></asp:ObjectDataSource>
    </form>
</body>
</html>