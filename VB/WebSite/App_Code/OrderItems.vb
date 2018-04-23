Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.ComponentModel
Imports System.Web.SessionState


Public Class OrderItemBase

    Private owner_Renamed As IList

    Public Sub New(ByVal owner As IList)
        Me.owner_Renamed = owner
    End Sub
    Protected ReadOnly Property Owner() As IList
        Get
            Return owner_Renamed
        End Get
    End Property
    Public ReadOnly Property Id() As Integer
        Get
            Return If(Owner IsNot Nothing, Owner.IndexOf(Me) + 1, -1)
        End Get
    End Property
End Class

Public Class OrderProduct
    Inherits OrderItemBase


    Private name_Renamed As String = String.Empty

    Private price_Renamed As Decimal = 0
    Public Sub New(ByVal owner As IList)
        MyBase.New(owner)
    End Sub
    Public Property Name() As String
        Get
            Return name_Renamed
        End Get
        Set(ByVal value As String)
            name_Renamed = value
        End Set
    End Property
    Public Property Price() As Decimal
        Get
            Return price_Renamed
        End Get
        Set(ByVal value As Decimal)
            price_Renamed = value
        End Set
    End Property
End Class

Public Class OrderItem
    Inherits OrderItemBase


    Private productId_Renamed As Integer

    Private quantity_Renamed As Integer
    Private products As IList

    Private product_Renamed As OrderProduct

    Public Sub New(ByVal owner As IList, ByVal products As IList)
        MyBase.New(owner)
        Me.products = products
        Me.quantity_Renamed = 1
    End Sub
    Public ReadOnly Property Product() As OrderProduct
        Get
            Return product_Renamed
        End Get
    End Property
    Public Property ProductId() As Integer
        Get
            Return productId_Renamed
        End Get
        Set(ByVal value As Integer)
            productId_Renamed = value
            SetProduct()
        End Set
    End Property
    Public Property Quantity() As Integer
        Get
            Return quantity_Renamed
        End Get
        Set(ByVal value As Integer)
            quantity_Renamed = value
        End Set
    End Property
    Public ReadOnly Property Price() As Decimal
        Get
            Return If(Product IsNot Nothing, Product.Price, 0)
        End Get
    End Property
    Public ReadOnly Property Total() As Decimal
        Get
            Return Price * Quantity
        End Get
    End Property
    Private Sub SetProduct()
        Me.product_Renamed = Nothing
        For Each item As OrderProduct In Me.products
            If item.Id = ProductId Then
                Me.product_Renamed = item
                Exit For
            End If
        Next item
    End Sub
End Class

Public Class OrderItemsProvider
    Private ReadOnly Property Session() As HttpSessionState
        Get
            Return HttpContext.Current.Session
        End Get
    End Property

    Public Function GetProducts() As BindingList(Of OrderProduct)
        Dim products As BindingList(Of OrderProduct) = TryCast(Session("OrderProducts"), BindingList(Of OrderProduct))
        If products Is Nothing Then
            products = CreateProducts()
            Session("OrderProducts") = products
        End If
        Return products
    End Function
    Public Function GetItems() As BindingList(Of OrderItem)
        Dim items As BindingList(Of OrderItem) = TryCast(Session("OrderItems"), BindingList(Of OrderItem))
        If items Is Nothing Then
            items = CreateOrderItems()
            Session("OrderItems") = items
        End If
        Return items
    End Function
    Public Sub ItemDelete(ByVal id As Integer)
        Dim item As OrderItem = GetItemById(id)
        If item IsNot Nothing Then
            GetItems().Remove(item)
        End If
    End Sub
    Public Sub ItemUpdate(ByVal productId As Integer, ByVal quantity As Integer, ByVal id As Integer)
        Dim item As OrderItem = GetItemById(id)
        If item IsNot Nothing Then
            ItemUpdateCore(item, productId, quantity)
        End If
    End Sub
    Public Sub ItemInsert(ByVal productId As Integer, ByVal quantity As Integer)
        Dim item As New OrderItem(GetItems(), GetProducts())
        ItemUpdateCore(item, productId, quantity)
        GetItems().Add(item)
    End Sub
    Private Sub ItemUpdateCore(ByVal item As OrderItem, ByVal productId As Integer, ByVal quantity As Integer)
        item.Quantity = quantity
        item.ProductId = productId
    End Sub
    Public Sub ProductUpdate(ByVal name As String, ByVal price As Decimal, ByVal id As Integer)
        Dim item As OrderProduct = GetProductById(id)
        If item IsNot Nothing Then
            ProductUpdateCore(item, name, price)
        End If
    End Sub
    Public Sub ProductInsert(ByVal name As String, ByVal price As Decimal)
        Dim item As New OrderProduct(GetProducts())
        ProductUpdateCore(item, name, price)
        GetProducts().Add(item)
    End Sub
    Private Sub ProductUpdateCore(ByVal item As OrderProduct, ByVal name As String, ByVal price As Decimal)
        item.Name = name
        item.Price = price
    End Sub
    Public Function GetItemById(ByVal id As Integer) As OrderItem
        For Each item As OrderItem In GetItems()
            If item.Id = id Then
                Return item
            End If
        Next item
        Return Nothing
    End Function
    Public Function GetProductById(ByVal id As Integer) As OrderProduct
        For Each item As OrderProduct In GetProducts()
            If item.Id = id Then
                Return item
            End If
        Next item
        Return Nothing
    End Function
    Private Function CreateProducts() As BindingList(Of OrderProduct)
        Dim res As New BindingList(Of OrderProduct)()
        Dim r As New Random()
        For i As Integer = 0 To 4
            Dim item As New OrderProduct(res)
            item.Name = String.Format("Product {0}", i + 1)
            item.Price = r.Next(10, 200)
            res.Add(item)
        Next i
        Return res
    End Function
    Private Function CreateOrderItems() As BindingList(Of OrderItem)
        Dim res As New BindingList(Of OrderItem)()
        Dim products As BindingList(Of OrderProduct) = GetProducts()
        Dim r As New Random()
        For i As Integer = 0 To 6
            Dim item As New OrderItem(res, products)
            item.ProductId = (i Mod products.Count) + 1
            item.Quantity = r.Next(1, 100)
            res.Add(item)
        Next i
        Return res
    End Function
End Class

