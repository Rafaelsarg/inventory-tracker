Table User {
  UserID int [primary key]
  FirstName varchar
  LastName varchar
  EmailAddress varchar
  LastLogin datetime
  Creation datetime
}

Table InternalAuthentication {
  AuthID int [primary key]
  UserID int 
  Salt varchar
  PasswordHash varchar
}

Table Inventory {
 InventoryID int [primary key]
 UserID int
 Name varchar
 Description text 
 Location varchar
 Creation datetime

}

Table ProductType {
  ProductTypeID int [primary key]
  Name varchar 
}

Table Product {
  ProductID int [primary key]
  ProductTypeID int 
  Name varchar 
  Creation datetime
}

Table AvailableProductInstance {
   ProductInstanceID int [primary key]
   ProductID int 
   InventoryID int
   RestockingTransactionID int  
   SKU varchar
      
}

Table SoldProductInstance {
  ProductInstanceID int [primary key]
  SalesTransactionID int
  SKU varchar

}

Table Supplier {
  SupplierID int [primary key]
  Name varchar
  ContactInfo varchar

}

Table Customer {
  CustomerID int [primary key]
  Name varchar
  ContactInfo varchar
}

Table SalesTransaction {
  ID int [primary key]
  CustomerID int
  TotalCost decimal
  RestockingDate datetime 
}

Table RestockingTransaction {
  ID int [primary key]
  SupplierID int 
  TotalCost decimal
  RestockingDate datetime
}

Ref: User.UserID - InternalAuthentication.UserID
Ref: ProductType.ProductTypeID < Product.ProductTypeID
Ref: Product.ProductID < AvailableProductInstance.ProductID
Ref: Inventory.InventoryID < AvailableProductInstance.InventoryID
Ref: User.UserID < Inventory.UserID
Ref: AvailableProductInstance.RestockingTransactionID > RestockingTransaction.ID
Ref: Supplier.SupplierID - RestockingTransaction.SupplierID
Ref: SalesTransaction.CustomerID - Customer.CustomerID
Ref: SoldProductInstance.SalesTransactionID > SalesTransaction.ID





