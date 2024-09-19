CREATE TABLE AppUser (
    id SERIAL PRIMARY KEY,
    FirstName varchar(255),
    LastName varchar(255),
    EmailAddress varchar(255),
    LastLogin timestamp,
    Creation timestamp
);

CREATE TABLE InternalAuthentication (
    id SERIAL PRIMARY KEY,
    UserID int,
    Salt varchar(255),
    PasswordHash varchar(255),
    FOREIGN KEY (UserID) REFERENCES appuser(id)
);

CREATE TABLE inventory (
    id SERIAL PRIMARY KEY,
    userid int,
    name varchar(255),
    description text,
    location varchar(255),
    creation timestamp,
    FOREIGN KEY (userid) REFERENCES appuser(id)
);

CREATE TABLE producttype (
    id SERIAL PRIMARY KEY,
    name varchar(255),
	inventoryid int,
	FOREIGN KEY (inventoryid) REFERENCES inventory(id)
);

CREATE TABLE product (
    id SERIAL PRIMARY KEY,
    producttypeid int,
    name varchar(255),
    creation timestamp,
    FOREIGN KEY (producttypeid) REFERENCES producttype(id)
);

CREATE TABLE supplier (
    id SERIAL PRIMARY KEY,
    name varchar(255),
    contactinfo varchar(255),
	inventoryid int,
	FOREIGN KEY (inventoryid) REFERENCES inventory(id)
);

CREATE TABLE restockingtransaction (
    id SERIAL PRIMARY KEY,
    supplierid int,
    totalcost decimal(10, 2),
    restockingdate timestamp,
    FOREIGN KEY (supplierid) REFERENCES supplier(id)
);


CREATE TABLE availableproductinstance (
    id SERIAL PRIMARY KEY,
    productid int,
    inventoryid int,
    restockingtransactionid int,
    sku varchar(255),
    FOREIGN KEY (productid) REFERENCES product(id),
    FOREIGN KEY (inventoryid) REFERENCES inventory(id),
    FOREIGN KEY (restockingtransactionid) REFERENCES restockingtransaction(id)
);

CREATE TABLE customer (
    id SERIAL PRIMARY KEY,
    name varchar(255),
    contactinfo varchar(255),
	inventoryid int,
	FOREIGN KEY (inventoryid) REFERENCES inventory(id)
);

CREATE TABLE salestransaction (
    id SERIAL PRIMARY KEY,
    customerid int,
    totalcost decimal(10, 2),
    restockingdate timestamp,
    FOREIGN KEY (customerid) REFERENCES customer(id)
);

CREATE TABLE soldproductinstance (
    id SERIAL PRIMARY KEY,
    salestransactionid int,
    sku varchar(255),
    FOREIGN KEY (salestransactionid) REFERENCES salestransaction(id)
);



